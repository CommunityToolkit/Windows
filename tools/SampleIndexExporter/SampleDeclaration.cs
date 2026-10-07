// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CommunityToolkit.SampleIndex;

/// <summary>
/// Evaluates the small set of C# expressions that <c>[ToolkitSample]</c> arguments are written
/// with, without a compilation.
/// </summary>
/// <remarks>
/// Sample authors write display names three ways: a plain literal
/// (<c>"Implicit animations"</c>), a <c>nameof</c> (<c>nameof(AutoSelectBehavior)</c>) and an
/// interpolated string mixing the two (<c>$"{nameof(FocusBehavior)}: Lists"</c>). All three
/// resolve from syntax alone, because <c>nameof</c> yields the final identifier of the
/// expression it is given.
///
/// <para>Anything outside that set returns <see langword="null"/> rather than a guess. The
/// caller reports it, so a new form shows up as a build failure instead of as a sample
/// published under the wrong name.</para>
/// </remarks>
internal static class ConstantText
{
    /// <summary>Evaluate an expression to a string, or return null when it is not a form we model.</summary>
    public static string? Evaluate(ExpressionSyntax? expression)
    {
        switch (expression)
        {
            case null:
                return null;

            case LiteralExpressionSyntax literal when literal.IsKind(SyntaxKind.StringLiteralExpression):
                return literal.Token.ValueText;

            case InvocationExpressionSyntax { Expression: IdentifierNameSyntax { Identifier.ValueText: "nameof" } } nameOf:
                return NameOfValue(nameOf);

            case InterpolatedStringExpressionSyntax interpolated:
                return Interpolated(interpolated);

            case BinaryExpressionSyntax binary when binary.IsKind(SyntaxKind.AddExpression):
                var left = Evaluate(binary.Left);
                var right = Evaluate(binary.Right);
                return left is null || right is null ? null : left + right;

            case ParenthesizedExpressionSyntax parenthesized:
                return Evaluate(parenthesized.Expression);

            default:
                return null;
        }
    }

    /// <summary>Evaluate a boolean literal argument.</summary>
    public static bool? EvaluateBool(ExpressionSyntax? expression) => expression switch
    {
        LiteralExpressionSyntax literal when literal.IsKind(SyntaxKind.TrueLiteralExpression) => true,
        LiteralExpressionSyntax literal when literal.IsKind(SyntaxKind.FalseLiteralExpression) => false,
        ParenthesizedExpressionSyntax parenthesized => EvaluateBool(parenthesized.Expression),
        _ => null,
    };

    /// <summary>Evaluate a numeric literal argument, including a negated one.</summary>
    public static double? EvaluateNumber(ExpressionSyntax? expression) => expression switch
    {
        LiteralExpressionSyntax literal when literal.IsKind(SyntaxKind.NumericLiteralExpression) =>
            Convert.ToDouble(literal.Token.Value, System.Globalization.CultureInfo.InvariantCulture),
        PrefixUnaryExpressionSyntax unary when unary.IsKind(SyntaxKind.UnaryMinusExpression) =>
            EvaluateNumber(unary.Operand) is { } value ? -value : null,
        ParenthesizedExpressionSyntax parenthesized => EvaluateNumber(parenthesized.Expression),
        _ => null,
    };

    /// <summary><c>nameof(A.B.C)</c> is <c>"C"</c>: the last identifier wins.</summary>
    private static string? NameOfValue(InvocationExpressionSyntax invocation)
    {
        if (invocation.ArgumentList.Arguments.Count != 1)
        {
            return null;
        }

        return invocation.ArgumentList.Arguments[0].Expression switch
        {
            IdentifierNameSyntax identifier => identifier.Identifier.ValueText,
            MemberAccessExpressionSyntax member => member.Name.Identifier.ValueText,
            GenericNameSyntax generic => generic.Identifier.ValueText,
            _ => null,
        };
    }

    private static string? Interpolated(InterpolatedStringExpressionSyntax interpolated)
    {
        var builder = new System.Text.StringBuilder();

        foreach (var content in interpolated.Contents)
        {
            switch (content)
            {
                case InterpolatedStringTextSyntax text:
                    builder.Append(text.TextToken.ValueText);
                    break;

                case InterpolationSyntax { AlignmentClause: null, FormatClause: null } interpolation:
                    var value = Evaluate(interpolation.Expression);
                    if (value is null)
                    {
                        return null;
                    }

                    builder.Append(value);
                    break;

                default:
                    return null;
            }
        }

        return builder.ToString();
    }
}

/// <summary>A sample-app option declared by a <c>[ToolkitSample*Option]</c> attribute.</summary>
/// <param name="Name">The binding name, which is the generated member's name.</param>
/// <param name="DefaultValue">
/// The value the generated member holds before anyone touches the options pane, formatted as
/// XAML would write it, or <see langword="null"/> when the declaration gives no usable default.
/// </param>
internal sealed record SampleOption(string Name, string? DefaultValue);

/// <summary>
/// A <c>[ToolkitSample]</c> declaration: the sample id the documentation refers to, the name
/// and description the sample app shows, and any options its markup can bind to.
/// </summary>
internal sealed class SampleDeclaration
{
    public required string Id { get; init; }

    public required string TypeName { get; init; }

    /// <summary>Repository-relative path of the declaring <c>.xaml.cs</c> file.</summary>
    public required string RelativePath { get; init; }

    public string? DisplayName { get; init; }

    public string? Description { get; init; }

    public IReadOnlyList<SampleOption> Options { get; init; } = [];

    /// <summary>
    /// Argument forms encountered that <see cref="ConstantText"/> does not model. Non-empty
    /// means a field was left unset rather than guessed.
    /// </summary>
    public IReadOnlyList<string> UnreadableArguments { get; init; } = [];

    /// <summary>
    /// Read every sample declaration in one <c>.xaml.cs</c> file.
    /// </summary>
    public static List<SampleDeclaration> Parse(string path, string relativePath)
    {
        var tree = CSharpSyntaxTree.ParseText(File.ReadAllText(path), path: path);
        var root = tree.GetRoot();
        var declarations = new List<SampleDeclaration>();

        foreach (var type in root.DescendantNodes().OfType<ClassDeclarationSyntax>())
        {
            var attributes = type.AttributeLists.SelectMany(list => list.Attributes).ToList();
            var sampleAttribute = attributes.FirstOrDefault(a => AttributeNameIs(a, "ToolkitSample"));
            if (sampleAttribute is null)
            {
                continue;
            }

            var unreadable = new List<string>();
            var arguments = sampleAttribute.ArgumentList?.Arguments ?? default;
            var bound = BindArguments(arguments, ["id", "displayName", "description"]);

            var id = ReadArgument(bound, "id", unreadable);
            var displayName = ReadArgument(bound, "displayName", unreadable);
            var description = ReadArgument(bound, "description", unreadable);

            // The id argument is always nameof(TheSampleClass), so the class name is the
            // authoritative fallback when the expression is one we do not model.
            id ??= type.Identifier.ValueText;

            declarations.Add(new SampleDeclaration
            {
                Id = id,
                TypeName = type.Identifier.ValueText,
                RelativePath = relativePath,
                DisplayName = displayName,
                Description = description,
                Options = ReadOptions(attributes),
                UnreadableArguments = unreadable,
            });
        }

        return declarations;
    }

    private static bool AttributeNameIs(AttributeSyntax attribute, string name)
    {
        var text = attribute.Name switch
        {
            IdentifierNameSyntax identifier => identifier.Identifier.ValueText,
            QualifiedNameSyntax qualified => qualified.Right.Identifier.ValueText,
            _ => attribute.Name.ToString(),
        };

        // Attribute syntax allows the "Attribute" suffix to be written out in full.
        return string.Equals(text, name, StringComparison.Ordinal)
            || string.Equals(text, name + "Attribute", StringComparison.Ordinal);
    }

    private static string? AttributeName(AttributeSyntax attribute) => attribute.Name switch
    {
        IdentifierNameSyntax identifier => identifier.Identifier.ValueText,
        QualifiedNameSyntax qualified => qualified.Right.Identifier.ValueText,
        _ => attribute.Name.ToString(),
    };

    private static string? ReadArgument(
        Dictionary<string, AttributeArgumentSyntax> bound,
        string name,
        List<string> unreadable)
    {
        if (!bound.TryGetValue(name, out var argument))
        {
            return null;
        }

        var value = ConstantText.Evaluate(argument.Expression);
        if (value is null)
        {
            unreadable.Add($"{name}: {argument.Expression}");
        }

        return value;
    }

    /// <summary>
    /// Match attribute arguments to the parameters they fill.
    /// </summary>
    /// <remarks>
    /// Sample authors mix the two calling styles freely — most write
    /// <c>[ToolkitSample(id: nameof(X), "Display name", description: "...")]</c>, where the
    /// display name is the only positional argument even though it is the second parameter.
    /// Binding by position alone would therefore miss it, so named arguments claim their
    /// parameter first and the positional ones fill whatever slots are left, in order. That is
    /// what the compiler does, and matching it is why the index agrees with the sample app.
    /// </remarks>
    private static Dictionary<string, AttributeArgumentSyntax> BindArguments(
        SeparatedSyntaxList<AttributeArgumentSyntax> arguments,
        string[] parameters)
    {
        var bound = new Dictionary<string, AttributeArgumentSyntax>(StringComparer.Ordinal);
        var positional = new List<AttributeArgumentSyntax>();

        foreach (var argument in arguments)
        {
            // NameEquals is the property-initializer form (Title = "..."), which sets a
            // property rather than filling a constructor parameter.
            if (argument.NameEquals is not null)
            {
                continue;
            }

            var name = argument.NameColon?.Name.Identifier.ValueText;
            if (name is null)
            {
                positional.Add(argument);
            }
            else if (Array.IndexOf(parameters, name) >= 0)
            {
                bound[name] = argument;
            }
        }

        var next = 0;
        foreach (var parameter in parameters)
        {
            if (bound.ContainsKey(parameter))
            {
                continue;
            }

            if (next < positional.Count)
            {
                bound[parameter] = positional[next++];
            }
        }

        return bound;
    }

    /// <summary>
    /// Read the option attributes on a sample class, in declaration order.
    /// </summary>
    /// <remarks>
    /// Each one causes a member to be generated on the sample's page, which its XAML then binds
    /// to. Those members exist only inside the sample app, so the default recorded here is what
    /// lets the published XAML say what the reader would have seen on first opening the sample.
    /// </remarks>
    private static List<SampleOption> ReadOptions(List<AttributeSyntax> attributes)
    {
        var options = new List<SampleOption>();

        foreach (var attribute in attributes)
        {
            var name = AttributeName(attribute);
            if (name is null || !name.StartsWith("ToolkitSample", StringComparison.Ordinal))
            {
                continue;
            }

            var arguments = attribute.ArgumentList?.Arguments ?? default;
            var positional = arguments.Where(a => a.NameColon is null && a.NameEquals is null).ToList();
            if (positional.Count == 0 && arguments.Count == 0)
            {
                continue;
            }

            string? defaultValue;

            if (AttributeNameIs(attribute, "ToolkitSampleBoolOption"))
            {
                // (string bindingName, bool defaultState)
                var bound = BindArguments(arguments, ["bindingName", "defaultState"]);
                if (!TryBindingName(bound, out var boolName))
                {
                    continue;
                }

                defaultValue = bound.TryGetValue("defaultState", out var state)
                    && ConstantText.EvaluateBool(state.Expression) is { } value
                        ? (value ? "True" : "False")
                        : null;

                options.Add(new SampleOption(boolName, defaultValue));
                continue;
            }

            if (AttributeNameIs(attribute, "ToolkitSampleNumericOption"))
            {
                // (string bindingName, double initial = 0, ...)
                var bound = BindArguments(arguments, ["bindingName", "initial", "min", "max", "step", "showAsNumberBox"]);
                if (!TryBindingName(bound, out var numericName))
                {
                    continue;
                }

                var initial = bound.TryGetValue("initial", out var argument)
                    ? ConstantText.EvaluateNumber(argument.Expression)
                    : 0d;

                options.Add(new SampleOption(
                    numericName,
                    initial?.ToString(System.Globalization.CultureInfo.InvariantCulture)));
                continue;
            }

            if (AttributeNameIs(attribute, "ToolkitSampleTextOption"))
            {
                // (string bindingName, string? placeholderText = null). The generated member
                // starts out holding the placeholder, so that is what the reader first sees.
                var bound = BindArguments(arguments, ["bindingName", "placeholderText"]);
                if (!TryBindingName(bound, out var textName))
                {
                    continue;
                }

                defaultValue = bound.TryGetValue("placeholderText", out var placeholder)
                    ? ConstantText.Evaluate(placeholder.Expression)
                    : null;

                options.Add(new SampleOption(textName, defaultValue));
                continue;
            }

            if (AttributeNameIs(attribute, "ToolkitSampleMultiChoiceOption"))
            {
                // (string bindingName, params string[] choices), where a choice may be written
                // "Label : Value" and the first choice is the initial selection. The choices are
                // a params array, so they are always positional.
                if (positional.Count == 0)
                {
                    continue;
                }

                var choiceName = ConstantText.Evaluate(positional[0].Expression);
                if (string.IsNullOrEmpty(choiceName))
                {
                    continue;
                }

                defaultValue = positional.Count > 1
                    ? ChoiceValue(ConstantText.Evaluate(positional[1].Expression))
                    : null;

                options.Add(new SampleOption(choiceName, defaultValue));
            }

            // Anything else is not an option attribute (ToolkitSampleOptionsPane, for instance).
        }

        return options;
    }

    /// <summary>A multi-choice entry is either <c>"Value"</c> or <c>"Label : Value"</c>.</summary>
    private static string? ChoiceValue(string? choice)
    {
        if (choice is null)
        {
            return null;
        }

        var separator = choice.IndexOf(" : ", StringComparison.Ordinal);
        return separator < 0 ? choice : choice[(separator + 3)..].TrimStart();
    }

    /// <summary>Read an option attribute's binding name, which is also the generated member's name.</summary>
    private static bool TryBindingName(
        Dictionary<string, AttributeArgumentSyntax> bound,
        out string name)
    {
        name = string.Empty;

        if (!bound.TryGetValue("bindingName", out var argument))
        {
            return false;
        }

        var value = ConstantText.Evaluate(argument.Expression);
        if (string.IsNullOrEmpty(value))
        {
            return false;
        }

        name = value;
        return true;
    }
}
