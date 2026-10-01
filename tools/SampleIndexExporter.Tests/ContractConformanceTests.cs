// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CommunityToolkit.SampleIndex.Tests;

/// <summary>
/// The contract gate: the published file has to stay usable by consumers that already read it.
/// </summary>
/// <remarks>
/// The index is a public artifact fetched from this repository's main branch. Anything checked
/// here is something a consumer is entitled to rely on without defensive code: ids that are
/// stable and safe in a URL, names that are present, a version it can branch on.
/// </remarks>
[TestClass]
public class ContractConformanceTests
{
    [TestMethod]
    public void IndexDeclaresItsSourceAndVersion()
    {
        Assert.AreEqual(1, RepositoryIndex.Index.SchemaVersion);
        Assert.AreEqual("toolkit", RepositoryIndex.Index.Source);
    }

    [TestMethod]
    public void EntryIdsAreUniqueAndUrlSafe()
    {
        var ids = RepositoryIndex.Index.Controls.Select(c => c.Id).ToList();

        CollectionAssert.AllItemsAreUnique(ids, "Entry ids must be unique; a consumer keys on them.");

        var unsafeIds = ids
            .Where(id => !id.All(c => char.IsAsciiLetterLower(c) || char.IsAsciiDigit(c) || c == '-'))
            .ToList();

        Assert.AreEqual(
            0,
            unsafeIds.Count,
            "Entry ids must be lowercase letters, digits and hyphens:\n  " + string.Join("\n  ", unsafeIds));
    }

    [TestMethod]
    public void EntryIdDependsOnlyOnItsOwnDocumentFileName()
    {
        // An id is published as stable, so it has to be a function of the entry's own document
        // and nothing else. Deriving it from the set of entries present — qualifying whichever
        // of two colliding file names happened to be read second — would silently rename an
        // entry that already shipped the day an unrelated component was added. A collision is
        // reported as an error instead, so this invariant holds by construction.
        var unexpected = RepositoryIndex.Index.Controls
            .Where(c => c.Toolkit?.DocumentPath is { } path && c.Id != ExpectedId(path))
            .Select(c => $"{c.Id} (expected '{ExpectedId(c.Toolkit!.DocumentPath!)}' from {c.Toolkit!.DocumentPath})")
            .ToList();

        Assert.AreEqual(
            0,
            unexpected.Count,
            "Entry ids must be derived from their own documentation file name alone:\n  "
                + string.Join("\n  ", unexpected));
    }

    private static string ExpectedId(string documentPath) =>
        Regex.Replace(Path.GetFileNameWithoutExtension(documentPath).ToLowerInvariant(), "[^a-z0-9]+", "-").Trim('-');

    [TestMethod]
    public void EveryEntryHasANameAndSourceDocument()
    {
        var incomplete = RepositoryIndex.Index.Controls
            .Where(c => string.IsNullOrWhiteSpace(c.Name) || string.IsNullOrWhiteSpace(c.Toolkit?.DocumentPath))
            .Select(c => c.Id)
            .ToList();

        Assert.AreEqual(
            0,
            incomplete.Count,
            "Entries missing a name or source document:\n  " + string.Join("\n  ", incomplete));
    }

    [TestMethod]
    public void EveryEntryLinksToItsDocumentation()
    {
        var unlinked = RepositoryIndex.Index.Controls
            .Where(c => c.Docs is null || c.Docs.Count == 0 || c.Docs.Any(d => string.IsNullOrWhiteSpace(d.Uri)))
            .Select(c => c.Id)
            .ToList();

        Assert.AreEqual(0, unlinked.Count, "Entries with no documentation link:\n  " + string.Join("\n  ", unlinked));
    }

    [TestMethod]
    public void EverySampleHasAHeader()
    {
        // The header is what a consumer shows in a picker. Without it the entry is a body of
        // markup the reader has to decode before they can tell whether they want it.
        var headerless = RepositoryIndex.Samples
            .Where(s => string.IsNullOrWhiteSpace(s.Sample.Header))
            .Select(s => s.Sample.Toolkit!.SourcePath)
            .ToList();

        Assert.AreEqual(0, headerless.Count, "Samples with no header:\n  " + string.Join("\n  ", headerless));
    }

    [TestMethod]
    public void EverySampleRecordsWhereItCameFrom()
    {
        var untraceable = RepositoryIndex.Samples
            .Where(s => string.IsNullOrWhiteSpace(s.Sample.Toolkit?.SourcePath)
                        || string.IsNullOrWhiteSpace(s.Sample.Toolkit?.SampleId))
            .Select(s => $"{s.Control.Id}: {s.Sample.Header}")
            .ToList();

        Assert.AreEqual(
            0,
            untraceable.Count,
            "Samples that do not say which file they came from:\n  " + string.Join("\n  ", untraceable));
    }

    [TestMethod]
    public void EveryNuGetPackageNameLooksLikeAPackage()
    {
        var suspicious = RepositoryIndex.Index.Controls
            .Where(c => c.NuGetPackage is { } package
                        && !package.StartsWith("CommunityToolkit.", StringComparison.Ordinal))
            .Select(c => $"{c.Id}: {c.NuGetPackage}")
            .ToList();

        Assert.AreEqual(
            0,
            suspicious.Count,
            "Entries naming a package that is not a toolkit package:\n  " + string.Join("\n  ", suspicious));
    }

    [TestMethod]
    public void CuratedKeywordsAreTrimmedAndNonEmpty()
    {
        // Consumers weight curated keywords above generated ones, so a stray empty string or
        // untrimmed entry becomes a search term that matches nothing.
        var malformed = RepositoryIndex.Index.Controls
            .Where(c => c.CuratedKeywords is { } keywords
                        && keywords.Any(k => string.IsNullOrWhiteSpace(k) || k != k.Trim()))
            .Select(c => c.Id)
            .ToList();

        Assert.AreEqual(
            0,
            malformed.Count,
            "Entries with blank or untrimmed curated keywords:\n  " + string.Join("\n  ", malformed));
    }
}
