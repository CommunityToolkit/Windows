// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CommunityToolkit.SampleIndex.Tests;

/// <summary>
/// The command-line contract: what CI actually depends on.
/// </summary>
/// <remarks>
/// The <c>Sample-Index</c> job is a gate, and a gate is only worth having if it fails when it
/// should. Everything here runs against a fixture repository built in a temporary directory,
/// because the questions are about the exporter's own behaviour — exit codes, what it writes,
/// what it refuses to write — rather than about this repository's samples.
/// </remarks>
[TestClass]
public class ProgramTests
{
    private string _root = string.Empty;

    [TestInitialize]
    public void CreateFixture()
    {
        _root = Fixture.Create();
    }

    [TestCleanup]
    public void DeleteFixture()
    {
        if (Directory.Exists(_root))
        {
            Directory.Delete(_root, recursive: true);
        }
    }

    [TestMethod]
    public void NoArgumentsIsAUsageError()
    {
        Assert.AreEqual(2, Run(out var error));
        StringAssert.Contains(error, "Usage:");
    }

    [TestMethod]
    public void AnUnknownCommandIsAUsageError()
    {
        // Distinct from 1: a mistyped command is the caller's mistake, not a stale index, and a
        // CI step that confused the two would report drift that is not there.
        Assert.AreEqual(2, Run(out _, "verify", "--repo-root", _root));
    }

    [TestMethod]
    public void CheckFailsWhenTheIndexHasNeverBeenGenerated()
    {
        Assert.AreEqual(1, Run(out var error, "check", "--repo-root", _root));
        StringAssert.Contains(error, "does not exist");
        Assert.IsFalse(File.Exists(IndexPath), "check must not write anything.");
    }

    [TestMethod]
    public void GenerateWritesTheIndex()
    {
        Assert.AreEqual(0, Run(out _, "generate", "--repo-root", _root));
        Assert.IsTrue(File.Exists(IndexPath));
    }

    [TestMethod]
    public void CheckPassesOnAFreshlyGeneratedIndex()
    {
        Run(out _, "generate", "--repo-root", _root);

        Assert.AreEqual(0, Run(out _, "check", "--repo-root", _root));
    }

    [TestMethod]
    public void CheckFailsWhenASampleChangedAfterGenerating()
    {
        // The whole point of the gate. If this passed, the published index could describe
        // markup that no longer exists and nothing would say so.
        Run(out _, "generate", "--repo-root", _root);

        File.WriteAllText(
            Path.Combine(_root, "components", "Widgets", "samples", "WidgetSample.xaml"),
            Fixture.SampleXaml.Replace("Hello", "Goodbye", StringComparison.Ordinal));

        Assert.AreEqual(1, Run(out var error, "check", "--repo-root", _root));
        StringAssert.Contains(error, "out of date");
    }

    [TestMethod]
    public void CheckIgnoresLineEndingsTheWorkingCopyMayHave()
    {
        // The index is generated with LF and byte-compared by a Linux CI job, but a contributor
        // regenerates it on Windows. Without this normalization the check would report every
        // Windows working copy as stale, and .gitattributes alone cannot be relied on to have
        // been applied to a copy that was already on disk.
        Run(out _, "generate", "--repo-root", _root);

        var lf = File.ReadAllText(IndexPath);
        Assert.IsFalse(lf.Contains('\r'), "The generator must write LF.");

        File.WriteAllText(IndexPath, lf.Replace("\n", "\r\n", StringComparison.Ordinal));

        Assert.AreEqual(0, Run(out _, "check", "--repo-root", _root));
    }

    [TestMethod]
    public void AnErrorInTheSourcesFailsAndWritesNothing()
    {
        // Publishing a half-built index is worse than publishing none: a consumer cannot tell
        // the difference between a control that was dropped and one that never existed.
        File.WriteAllText(
            Path.Combine(_root, "components", "Widgets", "samples", "Broken.md"),
            "---\nauthor: nobody\n---\n\n> [!SAMPLE WidgetSample]\n");

        Assert.AreEqual(1, Run(out var error, "generate", "--repo-root", _root));
        StringAssert.Contains(error, "error:");
        Assert.IsFalse(File.Exists(IndexPath), "Nothing may be written when the sources have errors.");
    }

    [TestMethod]
    public void AWarningDoesNotFailTheBuild()
    {
        // An unreferenced sample is worth saying out loud and not worth blocking a merge over.
        // If warnings failed, the gate would be turned off.
        File.WriteAllText(
            Path.Combine(_root, "components", "Widgets", "samples", "OrphanSample.xaml.cs"),
            Fixture.Declaration("OrphanSample", "Orphan"));

        Assert.AreEqual(0, Run(out _, "generate", "--repo-root", _root));
    }

    [TestMethod]
    public void TheRepositoryRootIsFoundByWalkingUp()
    {
        var nested = Directory.CreateDirectory(Path.Combine(_root, "components", "Widgets", "samples")).FullName;

        Assert.AreEqual(
            new DirectoryInfo(_root).FullName,
            new DirectoryInfo(IndexGenerator.FindRepoRoot(nested)).FullName);
    }

    [TestMethod]
    public void AnUnrecognizableRootIsReportedRatherThanGuessedAt()
    {
        var stray = Directory.CreateTempSubdirectory("sample-index-stray-").FullName;

        try
        {
            Assert.ThrowsException<DirectoryNotFoundException>(() => IndexGenerator.FindRepoRoot(stray));
        }
        finally
        {
            Directory.Delete(stray, recursive: true);
        }
    }

    private string IndexPath =>
        Path.Combine(_root, IndexGenerator.IndexRelativePath.Replace('/', Path.DirectorySeparatorChar));

    /// <summary>Run the exporter in-process, capturing what it wrote to stderr.</summary>
    private static int Run(out string error, params string[] args)
    {
        var originalOut = Console.Out;
        var originalError = Console.Error;
        var captured = new StringWriter(new StringBuilder());

        try
        {
            Console.SetOut(TextWriter.Null);
            Console.SetError(captured);
            return Program.Main(args);
        }
        finally
        {
            Console.SetOut(originalOut);
            Console.SetError(originalError);
            error = captured.ToString();
        }
    }
}
