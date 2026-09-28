// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CommunityToolkit.SampleIndex.Tests;

/// <summary>
/// The coverage gate: anything the index leaves out is listed here on purpose.
/// </summary>
/// <remarks>
/// A generator that quietly drops what it cannot handle looks healthy while it is losing data.
/// Pinning the exclusions means a sample that stops being published fails a test with its own
/// name in the message, instead of just disappearing from the file.
/// </remarks>
[TestClass]
public class CoverageTests
{
    /// <summary>
    /// Documentation pages that present no runnable markup.
    /// </summary>
    /// <remarks>
    /// These are real pages and they stay in the index, because a consumer searching the toolkit
    /// should still find <c>ArrayExtensions</c>. They carry an empty samples array because the
    /// APIs they document are called from code, not declared in markup. If a page that used to
    /// show samples turns up here, its <c>[!SAMPLE]</c> markers broke.
    /// </remarks>
    private static readonly string[] DocumentsWithoutSamples =
    [
        "components/Extensions/samples/ArrayExtensions.md",
        "components/Extensions/samples/AttachedShadows.md",
        "components/Extensions/samples/DependencyObjectExtensions.md",
        "components/Extensions/samples/DispatcherQueueExtensions.md",
        "components/Extensions/samples/EnumValuesExtension.md",
        "components/Extensions/samples/HyperlinkExtensions.md",
        "components/Extensions/samples/IconMarkupExtensions.md",
        "components/Extensions/samples/MatrixExtensions.md",
        "components/Extensions/samples/NullableBoolExtension.md",
        "components/Extensions/samples/OnDeviceExtension.md",
        "components/Extensions/samples/ScrollViewerExtensions.md",
        "components/Extensions/samples/ShadowAnimations.md",
        "components/Extensions/samples/StringExtensions.md",
        "components/Extensions/samples/TransformExtensions.md",
        "components/Extensions/samples/VisualExtensions.md",
        "components/Helpers/samples/ColorHelper.md",
        "components/Helpers/samples/DesignTimeHelper.md",
        "components/Helpers/samples/ScreenUnitHelper.md",
        "components/Helpers/samples/WeakEventListener.md",
    ];

    [TestMethod]
    public void NothingIsWithheld()
    {
        // A sample is withheld when its markup cannot be made pasteable. There is nothing in
        // that state today, and this test is here so that the first one is noticed and either
        // fixed at the source or listed deliberately.
        var withheld = RepositoryIndex.Result.Withheld
            .Select(w => $"{w.SampleId}: {w.Reason}")
            .ToList();

        Assert.AreEqual(
            0,
            withheld.Count,
            "Samples excluded from the index:\n  " + string.Join("\n  ", withheld));
    }

    [TestMethod]
    public void OnlyTheExpectedDocumentsHaveNoSamples()
    {
        var actual = RepositoryIndex.Index.Controls
            .Where(c => c.Samples.Count == 0)
            .Select(c => c.Toolkit!.DocumentPath)
            .OrderBy(p => p, StringComparer.Ordinal)
            .ToList();

        var expected = DocumentsWithoutSamples.OrderBy(p => p, StringComparer.Ordinal).ToList();

        var lost = actual.Except(expected).ToList();
        Assert.AreEqual(
            0,
            lost.Count,
            "These pages used to publish samples and no longer do. Check their [!SAMPLE] markers:\n  "
            + string.Join("\n  ", lost));

        var gained = expected.Except(actual).ToList();
        Assert.AreEqual(
            0,
            gained.Count,
            "These pages now publish samples. Remove them from DocumentsWithoutSamples:\n  "
            + string.Join("\n  ", gained));
    }

    [TestMethod]
    public void EverySampleDeclarationIsPublished()
    {
        // The sample app renders one control per [ToolkitSample]. Any declaration missing from
        // the index is a sample a reader can see in the gallery and not find here.
        var declared = Directory
            .EnumerateFiles(Path.Combine(RepositoryIndex.Root, "components"), "*.xaml.cs", SearchOption.AllDirectories)
            .Where(path => path.Contains($"{Path.DirectorySeparatorChar}samples{Path.DirectorySeparatorChar}", StringComparison.Ordinal))
            .SelectMany(path => SampleDeclaration.Parse(path, Path.GetRelativePath(RepositoryIndex.Root, path)))
            .Select(declaration => declaration.Id)
            .ToHashSet(StringComparer.Ordinal);

        var published = RepositoryIndex.Samples
            .Select(s => s.Sample.Toolkit!.SampleId)
            .ToHashSet(StringComparer.Ordinal);

        var missing = declared.Except(published).OrderBy(id => id, StringComparer.Ordinal).ToList();

        Assert.AreEqual(
            0,
            missing.Count,
            "Samples the app renders but the index omits. A [!SAMPLE] marker is probably missing "
            + "from the matching documentation page:\n  " + string.Join("\n  ", missing));
    }

    [TestMethod]
    public void EveryComponentWithSamplesIsRepresented()
    {
        var components = Directory
            .EnumerateDirectories(Path.Combine(RepositoryIndex.Root, "components"))
            .Where(directory => Directory.Exists(Path.Combine(directory, "samples")))
            .Select(Path.GetFileName)
            .ToList();

        var represented = RepositoryIndex.Index.Controls
            .Select(c => c.Toolkit!.Component)
            .ToHashSet(StringComparer.Ordinal);

        var missing = components.Where(c => !represented.Contains(c!)).ToList();

        Assert.AreEqual(
            0,
            missing.Count,
            "Components with a samples folder that produced no index entries:\n  "
            + string.Join("\n  ", missing));
    }
}
