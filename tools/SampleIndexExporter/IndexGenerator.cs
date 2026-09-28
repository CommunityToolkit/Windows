// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text.RegularExpressions;

namespace CommunityToolkit.SampleIndex;

/// <summary>
/// Builds the published index from the repository's own sample sources.
/// </summary>
/// <remarks>
/// The index is keyed by documentation file. Each <c>components/*/samples/*.md</c> becomes one
/// entry carrying its frontmatter, and its <c>[!SAMPLE]</c> markers are resolved, in document
/// order, to the <c>[ToolkitSample]</c> declarations and XAML files they name.
///
/// <para>This is not a source generator. <c>ToolkitSampleMetadataGenerator</c> is an
/// incremental generator, and <c>AddSource</c> only emits C# into a compilation — there is no
/// way for it to write a JSON file into the repository. The frontmatter and marker patterns are
/// shared with it deliberately, so the index cannot disagree with the sample app about what a
/// document presents.</para>
/// </remarks>
internal static partial class IndexGenerator
{
    /// <summary>Where the generated index is committed, relative to the repository root.</summary>
    public const string IndexRelativePath = "catalog/toolkit-samples.json";

    private const string RepositoryBlobUrl = "https://github.com/CommunityToolkit/Windows/blob/main/";

    /// <summary>Characters that are not safe in an identifier a consumer may put in a URL.</summary>
    [GeneratedRegex("[^a-z0-9]+")]
    private static partial Regex NonSlugRegex();

    /// <summary>
    /// Walk up from a starting directory to the repository root.
    /// </summary>
    public static string FindRepoRoot(string start)
    {
        var directory = new DirectoryInfo(start);

        while (directory is not null)
        {
            if (Directory.Exists(Path.Combine(directory.FullName, "components"))
                && File.Exists(Path.Combine(directory.FullName, "Directory.Build.props")))
            {
                return directory.FullName;
            }

            directory = directory.Parent;
        }

        throw new DirectoryNotFoundException(
            $"Could not find the repository root above '{start}'. Pass --repo-root explicitly.");
    }

    /// <summary>Generate the index from the tree rooted at <paramref name="repoRoot"/>.</summary>
    public static GenerationResult Generate(string repoRoot)
    {
        var issues = new List<IndexIssue>();
        var withheld = new List<WithheldSample>();
        var index = new SampleIndex();
        var usedIds = new Dictionary<string, string>(StringComparer.Ordinal);

        var componentsRoot = Path.Combine(repoRoot, "components");
        var components = Directory.GetDirectories(componentsRoot)
            .OrderBy(Path.GetFileName, StringComparer.Ordinal);

        foreach (var componentPath in components)
        {
            var component = Path.GetFileName(componentPath);
            var samplesRoot = Path.Combine(componentPath, "samples");
            if (!Directory.Exists(samplesRoot))
            {
                continue;
            }

            var nugetPackage = NuGetPackage(componentPath);
            var declarations = ReadDeclarations(repoRoot, samplesRoot, issues);
            var referenced = new HashSet<string>(StringComparer.Ordinal);

            var documents = Directory.GetFiles(samplesRoot, "*.md", SearchOption.AllDirectories)
                .OrderBy(p => Relative(repoRoot, p), StringComparer.Ordinal);

            foreach (var documentPath in documents)
            {
                var relativePath = Relative(repoRoot, documentPath);
                var document = MarkdownDocument.Parse(documentPath, relativePath);
                if (document is null)
                {
                    // No frontmatter: a plain markdown file that happens to live in samples/.
                    continue;
                }

                if (string.IsNullOrWhiteSpace(document.Title))
                {
                    issues.Add(new IndexIssue(
                        IssueSeverity.Error,
                        relativePath,
                        "the frontmatter has no 'title' field, so the entry would have no name."));
                    continue;
                }

                var entry = BuildEntry(
                    repoRoot, component, nugetPackage, document, declarations, referenced, issues, withheld, usedIds);

                if (entry is not null)
                {
                    index.Controls.Add(entry);
                }
            }

            foreach (var declaration in declarations.Values.OrderBy(d => d.Id, StringComparer.Ordinal))
            {
                if (!referenced.Contains(declaration.Id))
                {
                    issues.Add(new IndexIssue(
                        IssueSeverity.Warning,
                        declaration.RelativePath,
                        $"sample '{declaration.Id}' is not referenced by any '> [!SAMPLE ...]' marker, so it is "
                        + "not published and does not appear in the sample app."));
                }
            }
        }

        withheld.Sort((a, b) => string.CompareOrdinal(a.SampleId, b.SampleId));

        return new GenerationResult(index, issues, withheld);
    }

    /// <summary>Serialize an index exactly as it is committed.</summary>
    public static string Serialize(SampleIndex index) => IndexJsonContext.Serialize(index);

    private static IndexedControl? BuildEntry(
        string repoRoot,
        string component,
        string? nugetPackage,
        MarkdownDocument document,
        Dictionary<string, SampleDeclaration> declarations,
        HashSet<string> referenced,
        List<IndexIssue> issues,
        List<WithheldSample> withheld,
        Dictionary<string, string> usedIds)
    {
        var id = UniqueId(document, component, issues, usedIds);
        if (id is null)
        {
            return null;
        }

        var entry = new IndexedControl
        {
            Id = id,
            Name = document.Title!,
            Description = document.Description,
            NuGetPackage = nugetPackage,
            CuratedKeywords = NullIfEmpty(MarkdownDocument.SplitKeywords(document.Keywords)),
            Docs =
            [
                new IndexedDocLink
                {
                    Title = document.Title,
                    Uri = RepositoryBlobUrl + document.RelativePath,
                },
            ],
            Toolkit = new ControlExtension
            {
                Component = component,
                DocumentPath = document.RelativePath,
                Category = document.Category,
                Subcategory = document.Subcategory,
                Author = document.Author,
                Experimental = document.Experimental,
            },
        };

        foreach (var sampleId in document.SampleIds)
        {
            if (!declarations.TryGetValue(sampleId, out var declaration))
            {
                issues.Add(new IndexIssue(
                    IssueSeverity.Error,
                    document.RelativePath,
                    $"'> [!SAMPLE {sampleId}]' names a sample that does not exist. No class in this component "
                    + $"declares [ToolkitSample(id: nameof({sampleId}), ...)], so the sample app renders nothing here."));
                continue;
            }

            referenced.Add(sampleId);

            foreach (var unreadable in declaration.UnreadableArguments)
            {
                issues.Add(new IndexIssue(
                    IssueSeverity.Error,
                    declaration.RelativePath,
                    $"the [ToolkitSample] {unreadable} argument is not a literal, nameof or interpolated string, "
                    + "so its value cannot be read without compiling. Rewrite it in one of those forms."));
            }

            var sample = BuildSample(repoRoot, declaration, document, issues, withheld);
            if (sample is not null)
            {
                entry.Samples.Add(sample);
            }
        }

        // Documentation pages that present no samples are kept. Several components — Extensions
        // and Helpers especially — document APIs that have no markup to show, and dropping them
        // would make the index disagree with the promise that it carries one entry per
        // documentation file. A consumer that only wants pasteable markup filters on a
        // non-empty samples array; one building search over the whole toolkit keeps them.
        return entry;
    }

    private static IndexedSample? BuildSample(
        string repoRoot,
        SampleDeclaration declaration,
        MarkdownDocument document,
        List<IndexIssue> issues,
        List<WithheldSample> withheld)
    {
        var xamlPath = Path.ChangeExtension(Path.Combine(repoRoot, declaration.RelativePath.Replace('/', Path.DirectorySeparatorChar)), null);
        if (!File.Exists(xamlPath))
        {
            withheld.Add(new WithheldSample(declaration.Id, "the sample declares no .xaml file"));
            return null;
        }

        var extraction = XamlFragment.Extract(File.ReadAllText(xamlPath), declaration.Options);
        if (extraction.Xaml is null)
        {
            withheld.Add(new WithheldSample(declaration.Id, extraction.Error ?? "its XAML could not be extracted"));
            return null;
        }

        // The contract promises pasteable markup, so a binding left pointing at something only
        // the sample app has is a defect in this exporter rather than something to publish.
        foreach (var problem in XamlFragment.UnsettledBindings(extraction.Xaml, declaration.Options))
        {
            issues.Add(new IndexIssue(
                IssueSeverity.Error,
                Relative(repoRoot, xamlPath),
                $"the published XAML is not pasteable: {problem}. The extractor must resolve or remove it."));
            return null;
        }

        return new IndexedSample
        {
            Header = declaration.DisplayName,
            Details = document.SampleProse.TryGetValue(declaration.Id, out var prose) ? prose : declaration.Description,
            Xaml = extraction.Xaml,
            XmlnsImports = NullIfEmpty(extraction.XmlnsImports),
            Toolkit = new SampleExtension
            {
                SampleId = declaration.Id,
                SourcePath = Relative(repoRoot, xamlPath),
                OptionsResolved = NullIfEmpty(extraction.OptionsResolved),
                OptionBindingsDropped = NullIfEmpty(extraction.OptionBindingsDropped),
            },
        };
    }

    /// <summary>
    /// Read every <c>[ToolkitSample]</c> declaration in a component, keyed by sample id.
    /// </summary>
    private static Dictionary<string, SampleDeclaration> ReadDeclarations(
        string repoRoot,
        string samplesRoot,
        List<IndexIssue> issues)
    {
        var declarations = new Dictionary<string, SampleDeclaration>(StringComparer.Ordinal);
        var displayNames = new Dictionary<string, SampleDeclaration>(StringComparer.Ordinal);

        var files = Directory.GetFiles(samplesRoot, "*.xaml.cs", SearchOption.AllDirectories)
            .OrderBy(p => Relative(repoRoot, p), StringComparer.Ordinal);

        foreach (var file in files)
        {
            foreach (var declaration in SampleDeclaration.Parse(file, Relative(repoRoot, file)))
            {
                if (declarations.TryGetValue(declaration.Id, out var existing))
                {
                    issues.Add(new IndexIssue(
                        IssueSeverity.Error,
                        declaration.RelativePath,
                        $"sample id '{declaration.Id}' is already declared by {existing.RelativePath}. "
                        + "Ids must be unique within a component so a marker can name exactly one sample."));
                    continue;
                }

                declarations[declaration.Id] = declaration;

                if (declaration.DisplayName is { Length: > 0 } displayName)
                {
                    if (displayNames.TryGetValue(displayName, out var duplicate))
                    {
                        issues.Add(new IndexIssue(
                            IssueSeverity.Warning,
                            declaration.RelativePath,
                            $"[ToolkitSample] display name \"{displayName}\" is already used by "
                            + $"{duplicate.RelativePath}. Both samples render under that one title in the sample "
                            + "app, and both are published under it here."));
                    }
                    else
                    {
                        displayNames[displayName] = declaration;
                    }
                }
            }
        }

        return declarations;
    }

    /// <summary>
    /// The NuGet package a component ships as, taken from its source project's file name.
    /// </summary>
    private static string? NuGetPackage(string componentPath)
    {
        var sourceRoot = Path.Combine(componentPath, "src");
        if (!Directory.Exists(sourceRoot))
        {
            return null;
        }

        var projects = Directory.GetFiles(sourceRoot, "*.csproj", SearchOption.TopDirectoryOnly)
            .OrderBy(p => p, StringComparer.Ordinal)
            .ToList();

        return projects.Count == 1 ? Path.GetFileNameWithoutExtension(projects[0]) : null;
    }

    /// <summary>
    /// Derive the entry's identifier from the documentation file name, and keep it unique.
    /// </summary>
    /// <remarks>
    /// The file name rather than the title, because consumers key sample ids off this and a
    /// title is prose that gets reworded. When two components document the same name, the
    /// component qualifies the second one rather than either silently winning.
    /// </remarks>
    private static string? UniqueId(
        MarkdownDocument document,
        string component,
        List<IndexIssue> issues,
        Dictionary<string, string> usedIds)
    {
        var id = Slug(Path.GetFileNameWithoutExtension(document.Path));

        if (!usedIds.TryGetValue(id, out var owner))
        {
            usedIds[id] = document.RelativePath;
            return id;
        }

        var qualified = $"{Slug(component)}-{id}";
        if (!usedIds.TryGetValue(qualified, out owner))
        {
            usedIds[qualified] = document.RelativePath;
            return qualified;
        }

        issues.Add(new IndexIssue(
            IssueSeverity.Error,
            document.RelativePath,
            $"entry id '{qualified}' is already used by {owner}. Rename one of the documentation files."));

        return null;
    }

    private static string Slug(string value) =>
        NonSlugRegex().Replace(value.ToLowerInvariant(), "-").Trim('-');

    private static List<T>? NullIfEmpty<T>(List<T> values) => values.Count == 0 ? null : values;

    private static string Relative(string repoRoot, string path) =>
        Path.GetRelativePath(repoRoot, path).Replace('\\', '/');
}
