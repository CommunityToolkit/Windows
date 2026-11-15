// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CommunityToolkit.SampleIndex;

/// <summary>
/// What each namespace this repository ships puts in scope.
/// </summary>
/// <remarks>
/// A sample's imports have to be narrowed to the ones its published members still need, and
/// deciding that means knowing which namespace supplies a name the code uses. Binding the code
/// would answer it outright, but binding needs the built toolkit assemblies and this tool
/// deliberately never builds anything.
///
/// <para>The repository's own sources answer it for the namespaces where the answer matters. A
/// toolkit namespace stands for a package the reader has to reference, so publishing one the
/// sample does not use is an instruction to take on a dependency for nothing. Everything else a
/// sample imports is a platform namespace — <c>System</c>, <c>Windows</c>, <c>Microsoft.UI</c> —
/// which this repository does not declare and cannot enumerate, and which the reader's project
/// already has. Those are kept: an import that cannot be shown to be unused is not one to drop,
/// and the cost of being wrong runs one way. A missing import is a compile error in the reader's
/// file; a spare one is a hint the compiler discards.</para>
/// </remarks>
internal sealed class ToolkitApi
{
    /// <summary>
    /// One index per repository root. Generation runs several times over one tree — the tests
    /// alone generate for every gate — and the sources do not change underneath it.
    /// </summary>
    private static readonly Dictionary<string, ToolkitApi> Cache = new(StringComparer.Ordinal);

    private readonly Dictionary<string, HashSet<string>> _namesByNamespace;

    private ToolkitApi(Dictionary<string, HashSet<string>> namesByNamespace) =>
        _namesByNamespace = namesByNamespace;

    /// <summary>Read, once, what the components under <paramref name="repoRoot"/> declare.</summary>
    public static ToolkitApi ForRepository(string repoRoot)
    {
        lock (Cache)
        {
            if (!Cache.TryGetValue(repoRoot, out var api))
            {
                api = Build(repoRoot);
                Cache[repoRoot] = api;
            }

            return api;
        }
    }

    /// <summary>True when a component in this repository declares <paramref name="namespaceName"/>.</summary>
    public bool Declares(string namespaceName) => _namesByNamespace.ContainsKey(namespaceName);

    /// <summary>True when that namespace puts one of <paramref name="names"/> in scope.</summary>
    public bool Supplies(string namespaceName, IReadOnlySet<string> names) =>
        _namesByNamespace.TryGetValue(namespaceName, out var declared) && declared.Overlaps(names);

    private static ToolkitApi Build(string repoRoot)
    {
        var namesByNamespace = new Dictionary<string, HashSet<string>>(StringComparer.Ordinal);
        var componentsRoot = Path.Combine(repoRoot, "components");

        if (!Directory.Exists(componentsRoot))
        {
            return new ToolkitApi(namesByNamespace);
        }

        foreach (var componentPath in Directory.GetDirectories(componentsRoot))
        {
            var sourceRoot = Path.Combine(componentPath, "src");
            if (!Directory.Exists(sourceRoot))
            {
                continue;
            }

            foreach (var file in Directory.EnumerateFiles(sourceRoot, "*.cs", SearchOption.AllDirectories))
            {
                if (IsBuildOutput(file))
                {
                    continue;
                }

                // Read for the same platform the samples are published for, so a type that only
                // exists on WinAppSDK is in the index exactly when a published sample can name it.
                var tree = CSharpSyntaxTree.ParseText(
                    File.ReadAllText(file),
                    new CSharpParseOptions(preprocessorSymbols: SampleCode.WinAppSdkSymbols));

                Index(tree.GetRoot(), namesByNamespace);
            }
        }

        return new ToolkitApi(namesByNamespace);
    }

    private static bool IsBuildOutput(string path) =>
        path.Contains($"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}", StringComparison.Ordinal)
        || path.Contains($"{Path.DirectorySeparatorChar}obj{Path.DirectorySeparatorChar}", StringComparison.Ordinal);

    private static void Index(SyntaxNode root, Dictionary<string, HashSet<string>> namesByNamespace)
    {
        foreach (var node in root.DescendantNodes())
        {
            switch (node)
            {
                case BaseTypeDeclarationSyntax type:
                    Add(namesByNamespace, Namespace(type), type.Identifier.ValueText);
                    break;

                case DelegateDeclarationSyntax @delegate:
                    Add(namesByNamespace, Namespace(@delegate), @delegate.Identifier.ValueText);
                    break;

                // An extension method enters scope through its namespace rather than through the
                // class declaring it: a sample that calls .Debounce() names the method and never
                // names DispatcherQueueTimerExtensions, so indexing types alone would read that
                // sample as having no use for CommunityToolkit.WinUI.
                case MethodDeclarationSyntax method
                    when method.ParameterList.Parameters is [{ } first, ..]
                         && first.Modifiers.Any(SyntaxKind.ThisKeyword):
                    Add(namesByNamespace, Namespace(method), method.Identifier.ValueText);
                    break;
            }
        }
    }

    private static void Add(
        Dictionary<string, HashSet<string>> namesByNamespace,
        string namespaceName,
        string name)
    {
        if (namespaceName.Length == 0)
        {
            return;
        }

        if (!namesByNamespace.TryGetValue(namespaceName, out var names))
        {
            names = new HashSet<string>(StringComparer.Ordinal);
            namesByNamespace[namespaceName] = names;
        }

        names.Add(name);

        // An attribute is written without its suffix, so the name in the code and the name of
        // the type never match for the one construct where the type is never spelled out.
        if (name.EndsWith("Attribute", StringComparison.Ordinal) && name.Length > "Attribute".Length)
        {
            names.Add(name[..^"Attribute".Length]);
        }
    }

    private static string Namespace(SyntaxNode node)
    {
        var parts = new List<string>();

        for (var ancestor = node.Parent; ancestor is not null; ancestor = ancestor.Parent)
        {
            if (ancestor is BaseNamespaceDeclarationSyntax declaration)
            {
                parts.Insert(0, declaration.Name.ToString());
            }
        }

        return string.Join('.', parts);
    }
}
