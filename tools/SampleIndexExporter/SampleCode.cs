// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CommunityToolkit.SampleIndex;

/// <summary>
/// Extracts the C# a sample actually demonstrates from the page that hosts it.
/// </summary>
/// <remarks>
/// A sample's <c>.xaml.cs</c> is mostly scaffolding: a license header, a namespace belonging
/// to the sample app, a page class, and a constructor that calls <c>InitializeComponent</c>.
/// None of that is the sample. What is the sample is the handlers its markup calls and the
/// types its markup binds to, so those are what gets published — the same shape the WinUI
/// Gallery index publishes, which is a body of members a reader drops into their own page.
///
/// <para>Most samples have nothing left once the scaffolding is removed, and those publish no
/// code at all rather than a constructor that tells the reader nothing.</para>
/// </remarks>
internal static class SampleCode
{
    /// <summary>
    /// Symbols that make the parser keep the WinAppSDK branch of a conditional.
    /// </summary>
    /// <remarks>
    /// Samples multi-target WinAppSDK, UWP and Uno. The reader is on WinAppSDK, and a snippet
    /// carrying all three branches asks them to work out which one is theirs — so the parser
    /// is told which platform this is and the branches for the others never enter the tree.
    /// </remarks>
    private static readonly string[] WinAppSdkSymbols = ["WINAPPSDK", "WINUI3", "NET"];

    /// <summary>
    /// Read the publishable C# for one sample, or <see langword="null"/> when it has none.
    /// </summary>
    public static string? Extract(string path, string sampleTypeName)
    {
        var text = File.ReadAllText(path).Replace("\r\n", "\n");
        var tree = CSharpSyntaxTree.ParseText(
            text,
            new CSharpParseOptions(preprocessorSymbols: WinAppSdkSymbols),
            path: path);

        var root = tree.GetRoot();
        var parts = new List<string>();

        foreach (var type in root.DescendantNodes().OfType<BaseTypeDeclarationSyntax>())
        {
            // Nested types travel with the member that declares them.
            if (type.Parent is not (CompilationUnitSyntax or BaseNamespaceDeclarationSyntax))
            {
                continue;
            }

            if (type.Identifier.ValueText == sampleTypeName)
            {
                if (type is ClassDeclarationSyntax sample)
                {
                    parts.AddRange(sample.Members.Where(IsWorthPublishing).Select(Render));
                }

                continue;
            }

            // A type declared beside the sample - the enum a SwitchPresenter switches on, the
            // model an ItemsRepeater binds to - is part of what the markup needs to work.
            parts.Add(Render(type));
        }

        var code = string.Join("\n\n", parts.Where(part => part.Length > 0)).Trim();
        return code.Length == 0 ? null : code;
    }

    private static bool IsWorthPublishing(MemberDeclarationSyntax member)
    {
        // A constructor that only calls InitializeComponent is the page waking up, not the
        // sample doing anything. One that does more is kept whole.
        if (member is ConstructorDeclarationSyntax constructor)
        {
            var statements = constructor.Body?.Statements ?? default;
            return statements.Count != 1 || !IsInitializeComponent(statements[0]);
        }

        // ConvertStringTo… exists to turn an option-pane string into a real value. The options
        // pane is part of the sample browser, so with it gone nothing calls these.
        if (member is MethodDeclarationSyntax method
            && method.Identifier.ValueText.StartsWith("ConvertString", StringComparison.Ordinal))
        {
            return false;
        }

        return true;
    }

    private static bool IsInitializeComponent(StatementSyntax statement) =>
        statement is ExpressionStatementSyntax
        {
            Expression: InvocationExpressionSyntax
            {
                Expression: IdentifierNameSyntax { Identifier.ValueText: "InitializeComponent" }
                    or MemberAccessExpressionSyntax
                    {
                        Expression: ThisExpressionSyntax,
                        Name.Identifier.ValueText: "InitializeComponent",
                    },
            },
        };

    /// <summary>
    /// Render one declaration as standalone source.
    /// </summary>
    private static string Render(SyntaxNode node)
    {
        var cleaned = StripSampleAttributes(node);
        cleaned = StripPageWakeUp(cleaned);
        cleaned = StripConditionals(cleaned);
        var rendered = cleaned.NormalizeWhitespace(indentation: "    ", eol: "\n").ToFullString();

        return rendered.Trim();
    }

    /// <summary>
    /// Remove the <c>InitializeComponent</c> call from a constructor that does more than make
    /// it.
    /// </summary>
    /// <remarks>
    /// The reader's own page already calls it. A constructor is only published when it also
    /// does something the sample needs — navigating a frame, wiring a collection — and that
    /// part is worth keeping, while the call that loads a different page's markup is not.
    /// </remarks>
    private static SyntaxNode StripPageWakeUp(SyntaxNode node)
    {
        var calls = node
            .DescendantNodesAndSelf()
            .OfType<ConstructorDeclarationSyntax>()
            .SelectMany(constructor => constructor.Body?.Statements ?? default)
            .Where(IsInitializeComponent)
            .ToList();

        return calls.Count == 0
            ? node
            : node.RemoveNodes(calls, SyntaxRemoveOptions.KeepNoTrivia) ?? node;
    }

    /// <summary>
    /// Remove the conditional directives left behind around a branch the parser already took.
    /// </summary>
    /// <remarks>
    /// Telling the parser which platform this is keeps the other platforms' code out of the
    /// tree, but the <c>#if</c> and <c>#endif</c> lines themselves are trivia and survive that.
    /// Published, they read as a choice the reader has to make, when the choice has already
    /// been made for them.
    /// </remarks>
    private static SyntaxNode StripConditionals(SyntaxNode node) =>
        node.ReplaceTrivia(
            node.DescendantTrivia(descendIntoTrivia: true).Where(IsConditional),
            (_, _) => default);

    private static bool IsConditional(SyntaxTrivia trivia) => trivia.Kind() is
        SyntaxKind.IfDirectiveTrivia or
        SyntaxKind.ElifDirectiveTrivia or
        SyntaxKind.ElseDirectiveTrivia or
        SyntaxKind.EndIfDirectiveTrivia or
        SyntaxKind.DisabledTextTrivia;

    /// <summary>
    /// Remove the attributes that exist for the sample browser rather than for the code.
    /// </summary>
    /// <remarks>
    /// <c>[ToolkitSample]</c> and its option attributes are read by a source generator that
    /// the reader's project does not reference, so leaving them in publishes code that does
    /// not compile and describes a UI the reader never sees.
    /// </remarks>
    private static SyntaxNode StripSampleAttributes(SyntaxNode node)
    {
        var lists = node
            .DescendantNodesAndSelf()
            .OfType<AttributeListSyntax>()
            .Where(list => list.Attributes.All(IsSampleAttribute))
            .ToList();

        if (lists.Count == 0)
        {
            return node;
        }

        return node.RemoveNodes(lists, SyntaxRemoveOptions.KeepNoTrivia) ?? node;
    }

    private static bool IsSampleAttribute(AttributeSyntax attribute)
    {
        var name = attribute.Name switch
        {
            IdentifierNameSyntax identifier => identifier.Identifier.ValueText,
            QualifiedNameSyntax qualified => qualified.Right.Identifier.ValueText,
            _ => attribute.Name.ToString(),
        };

        return name.StartsWith("ToolkitSample", StringComparison.Ordinal);
    }
}
