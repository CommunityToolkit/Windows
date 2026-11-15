// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text.RegularExpressions;

namespace CommunityToolkit.SampleIndex;

/// <summary>
/// One <c>components/*/samples/*.md</c> file: its YAML frontmatter, the <c>[!SAMPLE]</c>
/// markers it contains in document order, and the prose introducing each of them.
/// </summary>
/// <remarks>
/// The frontmatter and marker patterns are deliberately the ones
/// <c>ToolkitSampleMetadataGenerator</c> uses to build the sample app. Matching it exactly is
/// the point: an index that disagreed with the app about which samples a document presents
/// would be worse than no index, because nothing would reveal the disagreement.
/// </remarks>
internal sealed partial class MarkdownDocument
{
    private MarkdownDocument(string path, string relativePath)
    {
        Path = path;
        RelativePath = relativePath;
    }

    /// <summary>Absolute path on disk.</summary>
    public string Path { get; }

    /// <summary>Repository-relative path, always with forward slashes.</summary>
    public string RelativePath { get; }

    public string? Title { get; private init; }

    public string? Author { get; private init; }

    public string? Description { get; private init; }

    public string? Keywords { get; private init; }

    public string? Category { get; private init; }

    public string? Subcategory { get; private init; }

    public bool? Experimental { get; private init; }

    /// <summary>Sample ids referenced by <c>[!SAMPLE]</c> markers, in document order.</summary>
    public IReadOnlyList<string> SampleIds { get; private init; } = [];

    /// <summary>Prose introducing each sample id, where the document has any.</summary>
    public IReadOnlyDictionary<string, string> SampleProse { get; private init; } =
        new Dictionary<string, string>(StringComparer.Ordinal);

    [GeneratedRegex(@"^title:\s*(?<value>.*)$", RegexOptions.IgnoreCase | RegexOptions.Multiline)]
    private static partial Regex TitleRegex();

    [GeneratedRegex(@"^author:\s*(?<value>.*)$", RegexOptions.IgnoreCase | RegexOptions.Multiline)]
    private static partial Regex AuthorRegex();

    [GeneratedRegex(@"^description:\s*(?<value>.*)$", RegexOptions.IgnoreCase | RegexOptions.Multiline)]
    private static partial Regex DescriptionRegex();

    [GeneratedRegex(@"^keywords:\s*(?<value>.*)$", RegexOptions.IgnoreCase | RegexOptions.Multiline)]
    private static partial Regex KeywordsRegex();

    [GeneratedRegex(@"^category:\s*(?<value>.*)$", RegexOptions.IgnoreCase | RegexOptions.Multiline)]
    private static partial Regex CategoryRegex();

    [GeneratedRegex(@"^subcategory:\s*(?<value>.*)$", RegexOptions.IgnoreCase | RegexOptions.Multiline)]
    private static partial Regex SubcategoryRegex();

    [GeneratedRegex(@"^experimental:\s*(?<value>.*)$", RegexOptions.IgnoreCase | RegexOptions.Multiline)]
    private static partial Regex ExperimentalRegex();

    /// <summary>The generator's own marker pattern, verbatim.</summary>
    [GeneratedRegex(@"^>\s*\[!SAMPLE\s*(?<sampleid>.*)\s*\]\s*$", RegexOptions.IgnoreCase | RegexOptions.Multiline)]
    private static partial Regex SampleMarkerRegex();

    /// <summary>A markdown link, reduced to its text.</summary>
    [GeneratedRegex(@"\[([^\]]+)\]\([^)]+\)")]
    private static partial Regex MarkdownLinkRegex();

    /// <summary>Inline code, reduced to its contents.</summary>
    [GeneratedRegex(@"`([^`]+)`")]
    private static partial Regex InlineCodeRegex();

    [GeneratedRegex(@"\s+")]
    private static partial Regex WhitespaceRegex();

    /// <summary>
    /// Read and parse a documentation file. Returns <see langword="null"/> when the file has no
    /// frontmatter section, which is how the generator distinguishes a documentation file from
    /// an ordinary markdown file that happens to sit in the samples folder.
    /// </summary>
    public static MarkdownDocument? Parse(string path, string relativePath)
    {
        // Normalize line endings once. Every pattern below is anchored with Multiline and the
        // prose splitter looks for blank lines, both of which behave differently under CRLF.
        var content = File.ReadAllText(path).Replace("\r\n", "\n").Replace('\r', '\n');

        var sections = content.Split(["---"], StringSplitOptions.RemoveEmptyEntries);
        if (sections.Length <= 1)
        {
            return null;
        }

        var frontmatter = sections[0];
        var experimentalText = Field(frontmatter, ExperimentalRegex());

        return new MarkdownDocument(path, relativePath)
        {
            Title = Field(frontmatter, TitleRegex()),
            Author = Field(frontmatter, AuthorRegex()),
            Description = Field(frontmatter, DescriptionRegex()),
            Keywords = Field(frontmatter, KeywordsRegex()),
            Category = Field(frontmatter, CategoryRegex()),
            Subcategory = Field(frontmatter, SubcategoryRegex()),
            Experimental = bool.TryParse(experimentalText, out var experimental) ? experimental : null,
            SampleIds = ReadSampleIds(content),
            SampleProse = ReadSampleProse(content),
        };
    }

    /// <summary>Split the <c>keywords</c> frontmatter field into individual terms.</summary>
    public static List<string> SplitKeywords(string? keywords) =>
        string.IsNullOrWhiteSpace(keywords)
            ? []
            : [.. keywords.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)];

    private static string? Field(string frontmatter, Regex pattern)
    {
        var match = pattern.Match(frontmatter);
        if (!match.Success)
        {
            return null;
        }

        var value = match.Groups["value"].Value.Trim();
        return value.Length == 0 ? null : value;
    }

    private static List<string> ReadSampleIds(string content)
    {
        var ids = new List<string>();
        foreach (Match match in SampleMarkerRegex().Matches(content))
        {
            var id = match.Groups["sampleid"].Value.Trim();
            if (id.Length > 0)
            {
                ids.Add(id);
            }
        }

        return ids;
    }

    /// <summary>
    /// Associate each marker with the prose that introduces it: the paragraphs between the
    /// previous marker and this one, starting after the most recent section heading.
    /// </summary>
    /// <remarks>
    /// The heading boundary matters for documents that present several controls in one file —
    /// <c>SizerControls.md</c>, for instance — where without it the introduction to one section
    /// would be attached to the first sample of the next.
    /// </remarks>
    private static Dictionary<string, string> ReadSampleProse(string content)
    {
        var body = content;
        var sections = content.Split(["---"], StringSplitOptions.RemoveEmptyEntries);
        if (sections.Length > 1)
        {
            // Drop the frontmatter block so its fields cannot be mistaken for prose.
            var frontmatterEnd = content.IndexOf("---", content.IndexOf("---", StringComparison.Ordinal) + 3, StringComparison.Ordinal);
            if (frontmatterEnd >= 0)
            {
                body = content[(frontmatterEnd + 3)..];
            }
        }

        var prose = new Dictionary<string, string>(StringComparer.Ordinal);
        var cursor = 0;

        foreach (Match match in SampleMarkerRegex().Matches(body))
        {
            var id = match.Groups["sampleid"].Value.Trim();
            var preceding = body[cursor..match.Index];
            cursor = match.Index + match.Length;

            if (id.Length == 0)
            {
                continue;
            }

            var paragraphs = preceding
                .Split("\n\n", StringSplitOptions.RemoveEmptyEntries)
                .Select(p => p.Trim())
                .Where(p => p.Length > 0)
                .ToList();

            var start = 0;
            for (var i = paragraphs.Count - 1; i >= 0; i--)
            {
                if (paragraphs[i].StartsWith('#'))
                {
                    start = i + 1;
                    break;
                }
            }

            var local = paragraphs
                .Skip(start)
                .Where(p => !p.StartsWith('#') && !p.StartsWith('>'))
                .ToList();

            if (local.Count == 0)
            {
                continue;
            }

            var text = CleanProse(string.Join(" ", local));
            if (text.Length > 0)
            {
                prose[id] = text;
            }
        }

        return prose;
    }

    /// <summary>
    /// Reduce markdown prose to plain text. Links keep their text and inline code its contents,
    /// because a consumer renders this as a sentence rather than as markdown.
    /// </summary>
    private static string CleanProse(string text)
    {
        var result = MarkdownLinkRegex().Replace(text, "$1");
        result = InlineCodeRegex().Replace(result, "$1");
        result = WhitespaceRegex().Replace(result, " ");
        return result.Trim();
    }
}
