// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text.Json.Serialization;

namespace CommunityToolkit.SampleIndex;

/// <summary>
/// The published index document.
/// </summary>
/// <remarks>
/// The shape is the shared WinUI sample index contract, so a consumer that already reads
/// another source's index reads this one with the same parser. Anything specific to this
/// repository lives under the <c>toolkit</c> extension objects, which the contract allows and
/// which a consumer may ignore entirely.
///
/// <para>Every collection is serialized in a deterministic order and no timestamp is emitted,
/// so regenerating on an unchanged tree produces a byte-identical file. That is what lets CI
/// compare the committed file against a fresh generate.</para>
/// </remarks>
internal sealed class SampleIndex
{
    [JsonPropertyName("schemaVersion")]
    public int SchemaVersion { get; set; } = 1;

    [JsonPropertyName("source")]
    public string Source { get; set; } = "toolkit";

    /// <summary>
    /// Documented entries, one per <c>components/*/samples/*.md</c>.
    /// </summary>
    /// <remarks>
    /// Named <c>controls</c> by the contract. Keyed by documentation file rather than by
    /// component because a component routinely documents several controls — SettingsControls
    /// documents SettingsCard and SettingsExpander, Sizers documents ContentSizer, GridSplitter
    /// and PropertySizer — and the documentation file is where an author already recorded which
    /// is which. Keying by component would push that split onto every consumer.
    /// </remarks>
    [JsonPropertyName("controls")]
    public List<IndexedControl> Controls { get; set; } = [];

    [JsonIgnore]
    public int ControlCount => Controls.Count;

    [JsonIgnore]
    public int SampleCount => Controls.Sum(c => c.Samples.Count);
}

/// <summary>One documentation file and the samples it presents, in document order.</summary>
internal sealed class IndexedControl
{
    /// <summary>
    /// Stable, URL-safe identifier, derived from the documentation file name
    /// (<c>SettingsCard.md</c> becomes <c>settingscard</c>).
    /// </summary>
    /// <remarks>
    /// Derived from the file name, not the title, because consumers key sample ids off this and
    /// a title is prose that gets reworded. Uniqueness across the repository is enforced when
    /// the index is generated.
    /// </remarks>
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    /// <summary>Display name: the documentation file's <c>title</c> frontmatter field.</summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>One-line summary: the documentation file's <c>description</c> frontmatter field.</summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    /// <summary>
    /// NuGet package that must be referenced, read from the component's
    /// <c>src/&lt;Package&gt;.csproj</c> file name.
    /// </summary>
    [JsonPropertyName("nugetPackage")]
    public string? NuGetPackage { get; set; }

    /// <summary>
    /// The documentation file's <c>keywords</c> frontmatter field, split on commas.
    /// </summary>
    /// <remarks>
    /// Published as <c>curatedKeywords</c> rather than <c>keywords</c> because the contract
    /// reserves that field for terms the sample's own author wrote, which is exactly what
    /// frontmatter keywords are.
    /// </remarks>
    [JsonPropertyName("curatedKeywords")]
    public List<string>? CuratedKeywords { get; set; }

    [JsonPropertyName("docs")]
    public List<IndexedDocLink>? Docs { get; set; }

    [JsonPropertyName("toolkit")]
    public ControlExtension? Toolkit { get; set; }

    [JsonPropertyName("samples")]
    public List<IndexedSample> Samples { get; set; } = [];
}

internal sealed class IndexedDocLink
{
    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("uri")]
    public string Uri { get; set; } = string.Empty;
}

/// <summary>
/// Repository-specific metadata that has no place in the shared contract. Consumers may ignore
/// all of it; nothing here is required to use a sample.
/// </summary>
internal sealed class ControlExtension
{
    /// <summary>Component directory name, e.g. <c>SettingsControls</c>.</summary>
    [JsonPropertyName("component")]
    public string Component { get; set; } = string.Empty;

    /// <summary>Repository-relative path of the documentation file this entry was built from.</summary>
    [JsonPropertyName("documentPath")]
    public string DocumentPath { get; set; } = string.Empty;

    [JsonPropertyName("category")]
    public string? Category { get; set; }

    [JsonPropertyName("subcategory")]
    public string? Subcategory { get; set; }

    [JsonPropertyName("author")]
    public string? Author { get; set; }

    /// <summary>True when the documentation file marks the component experimental.</summary>
    [JsonPropertyName("experimental")]
    public bool? Experimental { get; set; }
}

/// <summary>One sample: a <c>[!SAMPLE]</c> marker resolved to its declaration and its XAML.</summary>
internal sealed class IndexedSample
{
    /// <summary>The <c>displayName</c> argument of the sample's <c>[ToolkitSample]</c> attribute.</summary>
    [JsonPropertyName("header")]
    public string? Header { get; set; }

    /// <summary>
    /// Prose describing this sample: the paragraphs the documentation file places immediately
    /// before the sample's marker, falling back to the attribute's <c>description</c> argument.
    /// </summary>
    [JsonPropertyName("details")]
    public string? Details { get; set; }

    /// <summary>
    /// The sample's XAML as a pasteable fragment: the body of its root element, with the
    /// sample-app-only bindings resolved. Never the verbatim file, which carries an
    /// <c>x:Class</c> and a <c>&lt;Page&gt;</c> root that belong to the sample app.
    /// </summary>
    [JsonPropertyName("xaml")]
    public string? Xaml { get; set; }

    /// <summary>
    /// The C# the sample demonstrates: the handlers its markup calls and the types its markup
    /// binds to, as members a reader drops into their own page. Omitted when the sample's
    /// code-behind is only the page scaffolding, which is the common case.
    /// </summary>
    [JsonPropertyName("code")]
    public string? Code { get; set; }

    /// <summary>Language of <see cref="Code"/>. Always <c>csharp</c> when code is present.</summary>
    [JsonPropertyName("language")]
    public string? Language { get; set; }

    /// <summary>
    /// The XAML namespace declarations this fragment actually uses, emitted verbatim.
    /// </summary>
    /// <remarks>
    /// Per sample rather than per control: sibling samples of one control legitimately pull in
    /// different namespaces, and an import a sample does not use is an instruction to add a
    /// package reference it does not need.
    /// </remarks>
    [JsonPropertyName("xmlnsImports")]
    public List<string>? XmlnsImports { get; set; }

    [JsonPropertyName("toolkit")]
    public SampleExtension? Toolkit { get; set; }
}

/// <summary>
/// Per-sample extraction metadata. Informational: the published <c>xaml</c> is already
/// pasteable, and these fields only explain what had to change to make it so.
/// </summary>
internal sealed class SampleExtension
{
    /// <summary>The sample's <c>[ToolkitSample]</c> id, which is also its <c>[!SAMPLE]</c> marker.</summary>
    [JsonPropertyName("sampleId")]
    public string SampleId { get; set; } = string.Empty;

    /// <summary>Repository-relative path of the sample's <c>.xaml</c> file.</summary>
    [JsonPropertyName("sourcePath")]
    public string SourcePath { get; set; } = string.Empty;

    /// <summary>
    /// Sample-app option members whose bindings were replaced with the option's default value,
    /// as <c>Name=Value</c>.
    /// </summary>
    [JsonPropertyName("optionsResolved")]
    public List<string>? OptionsResolved { get; set; }

    /// <summary>
    /// Attributes removed because they bound to a sample-app option through an expression with
    /// no static value — a function binding, for instance. The contract requires published XAML
    /// to be pasteable, and an attribute is the one construct whose absence yields the target
    /// control's own default.
    /// </summary>
    [JsonPropertyName("optionBindingsDropped")]
    public List<string>? OptionBindingsDropped { get; set; }
}
