// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CommunityToolkit.SampleIndex.Tests;

/// <summary>
/// The staleness gate: the committed index has to match what the samples produce right now.
/// </summary>
/// <remarks>
/// Without this, the index rots quietly. Someone edits a sample, the committed JSON keeps
/// describing the old one, and nothing anywhere says so — consumers keep serving markup that no
/// longer exists in the repository. Running this in CI turns that into a failed build with a
/// one-line fix.
/// </remarks>
[TestClass]
public class IndexUpToDateTests
{
    [TestMethod]
    public void CommittedIndexExists()
    {
        var path = Path.Combine(RepositoryIndex.Root, IndexGenerator.IndexRelativePath);

        Assert.IsTrue(
            File.Exists(path),
            $"{IndexGenerator.IndexRelativePath} is missing. Run "
            + "'dotnet run --project tools/SampleIndexExporter -- generate' and commit the result.");
    }

    [TestMethod]
    public void CommittedIndexMatchesTheSamples()
    {
        var path = Path.Combine(RepositoryIndex.Root, IndexGenerator.IndexRelativePath);
        var committed = File.ReadAllText(path).Replace("\r\n", "\n");
        var regenerated = IndexGenerator.Serialize(RepositoryIndex.Index);

        Assert.AreEqual(
            regenerated,
            committed,
            $"{IndexGenerator.IndexRelativePath} does not match the samples in this branch. Run "
            + "'dotnet run --project tools/SampleIndexExporter -- generate' and commit the result.");
    }

    [TestMethod]
    public void GeneratingTwiceProducesTheSameBytes()
    {
        // Stable ordering, stated as a test rather than assumed. If generation were order-
        // dependent — on file enumeration order, say, or on a dictionary's iteration order —
        // the staleness check above would fail at random and teach everyone to ignore it.
        var first = IndexGenerator.Serialize(IndexGenerator.Generate(RepositoryIndex.Root).Index);
        var second = IndexGenerator.Serialize(IndexGenerator.Generate(RepositoryIndex.Root).Index);

        Assert.AreEqual(first, second, "Generating the index twice produced different output.");
    }

    [TestMethod]
    public void EntriesAreOrderedByDocumentPath()
    {
        var paths = RepositoryIndex.Index.Controls.Select(c => c.Toolkit!.DocumentPath).ToList();
        var sorted = paths.OrderBy(p => p, StringComparer.Ordinal).ToList();

        CollectionAssert.AreEqual(
            sorted,
            paths,
            "Entries must be ordered by document path so that adding a sample produces a small diff.");
    }

    [TestMethod]
    public void SourceDataHasNoErrors()
    {
        var errors = RepositoryIndex.Result.Errors.Select(e => e.ToString()).ToList();

        Assert.AreEqual(
            0,
            errors.Count,
            "The sample sources have errors that stop the index being published:\n  "
            + string.Join("\n  ", errors));
    }
}
