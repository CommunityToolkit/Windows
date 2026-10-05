// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CommunityToolkit.SampleIndex;

/// <summary>What extracting one sample's C# produced.</summary>
/// <param name="Code">The publishable members, or null when the sample has none.</param>
/// <param name="Usings">
/// Namespaces the published members need imported, in ordinal order. Published on the owning
/// control rather than inside <paramref name="Code"/>: the contract has consumers prepend them
/// as <c>using X;</c> lines, and asks that no sample's code repeat them.
/// </param>
internal sealed record SampleCodeExtraction(string? Code, List<string> Usings);

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
    ///
    /// <para>Shared with <see cref="ToolkitApi"/>, which reads the component sources for the
    /// same platform so that what a namespace is known to supply matches what a published
    /// sample is allowed to name.</para>
    /// </remarks>
    public static readonly string[] WinAppSdkSymbols = ["WINAPPSDK", "WINUI3", "NET"];

    /// <summary>
    /// Read the publishable C# for one sample, together with the namespaces it needs imported.
    /// </summary>
    public static SampleCodeExtraction Extract(string path, string sampleTypeName, ToolkitApi api)
    {
        var text = File.ReadAllText(path).Replace("\r\n", "\n");
        var tree = CSharpSyntaxTree.ParseText(
            text,
            new CSharpParseOptions(preprocessorSymbols: WinAppSdkSymbols),
            path: path);

        var root = (CompilationUnitSyntax)tree.GetRoot();
        var published = new List<SyntaxNode>();

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
                    published.AddRange(sample.Members.Where(IsWorthPublishing).Select(Clean));
                }

                continue;
            }

            // A type declared beside the sample - the enum a SwitchPresenter switches on, the
            // model an ItemsRepeater binds to - is part of what the markup needs to work.
            published.Add(Clean(type));
        }

        var parts = published.Select(Render).Where(part => part.Length > 0);
        var code = string.Join("\n\n", parts).Trim();

        // Nothing published needs nothing imported. Most samples land here: their code-behind
        // was only page scaffolding, and an import list for code that was never published would
        // be prepended by a consumer to samples that are pure markup.
        return code.Length == 0
            ? new SampleCodeExtraction(null, [])
            : new SampleCodeExtraction(code, Imports(root, published, api));
    }

    /// <summary>
    /// The namespaces the published members still need imported.
    /// </summary>
    /// <remarks>
    /// A sample file imports what the whole page needed, and most of the page is scaffolding
    /// that is not published. Publishing its imports unfiltered would hand the reader packages
    /// to reference for code they were never given, and — for the sample app's own namespaces —
    /// a <c>using</c> that cannot resolve in any project but this repository's.
    /// </remarks>
    private static List<string> Imports(
        CompilationUnitSyntax root,
        List<SyntaxNode> published,
        ToolkitApi api)
    {
        var names = published
            .SelectMany(node => node.DescendantNodesAndSelf().OfType<SimpleNameSyntax>())
            .Select(name => name.Identifier.ValueText)
            .ToHashSet(StringComparer.Ordinal);

        var pageNamespace = PageNamespace(root);
        var imports = new List<string>();

        foreach (var directive in root.DescendantNodes().OfType<UsingDirectiveSyntax>())
        {
            if (Import(directive, names) is not { } import
                || BelongsToTheSampleApp(import, pageNamespace)
                || (api.Declares(import) && !api.Supplies(import, names))
                || imports.Contains(import, StringComparer.Ordinal))
            {
                continue;
            }

            imports.Add(import);
        }

        imports.Sort(StringComparer.Ordinal);
        return imports;
    }

    /// <summary>
    /// The namespace one <c>using</c> directive asks a consumer to import, or
    /// <see langword="null"/> when it asks for something a namespace import cannot express.
    /// </summary>
    private static string? Import(UsingDirectiveSyntax directive, HashSet<string> names)
    {
        // 'using static X;' imports one type's members rather than a namespace, and the field
        // this feeds is a list of namespaces a consumer writes out as 'using X;'. There is no
        // form for it, so it is not published.
        if (directive.StaticKeyword != default || directive.Name is null)
        {
            return null;
        }

        if (directive.Alias is null)
        {
            return directive.Name.ToString();
        }

        // An alias is how a sample names one platform's type while multi-targeting:
        // 'using DispatcherQueue = Microsoft.UI.Dispatching.DispatcherQueue;'. The branch has
        // already been resolved to WinAppSDK by the time anything is published, so what the
        // reader needs is the type's own namespace - but only when the alias renames the type
        // to itself, which is what makes the published code read the same either way. An alias
        // to a different name is not expressible as a namespace import and is left out.
        var aliasName = directive.Alias.Name.Identifier.ValueText;

        return directive.Name is QualifiedNameSyntax qualified
               && qualified.Right.Identifier.ValueText == aliasName
               && names.Contains(aliasName)
            ? qualified.Left.ToString()
            : null;
    }

    /// <summary>The namespace the sample page itself is declared in.</summary>
    private static string PageNamespace(CompilationUnitSyntax root) =>
        root.DescendantNodes().OfType<BaseNamespaceDeclarationSyntax>().FirstOrDefault()?.Name.ToString()
        ?? string.Empty;

    /// <summary>
    /// True when an import names the sample app rather than the toolkit or the platform.
    /// </summary>
    /// <remarks>
    /// A sample page sometimes imports a sibling of its own namespace — the Connected Animations
    /// sample navigates to pages declared one folder down. Those namespaces exist only in this
    /// repository's sample app, so publishing one would be publishing a <c>using</c> that cannot
    /// resolve anywhere the snippet is actually pasted.
    /// </remarks>
    private static bool BelongsToTheSampleApp(string import, string pageNamespace) =>
        pageNamespace.Length > 0
        && (import == pageNamespace
            || import.StartsWith(pageNamespace + ".", StringComparison.Ordinal)
            || pageNamespace.StartsWith(import + ".", StringComparison.Ordinal));

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
    /// Remove from one declaration everything that belongs to the sample app rather than to the
    /// sample.
    /// </summary>
    private static SyntaxNode Clean(SyntaxNode node)
    {
        var cleaned = StripSampleAttributes(node);
        cleaned = StripPageWakeUp(cleaned);
        return StripConditionals(cleaned);
    }

    /// <summary>
    /// Render one cleaned declaration as standalone source.
    /// </summary>
    private static string Render(SyntaxNode cleaned) =>
        cleaned.NormalizeWhitespace(indentation: "    ", eol: "\n").ToFullString().Trim();

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
