// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CommunityToolkit.SampleIndex.Tests;

/// <summary>
/// Gates for the published C#.
/// </summary>
/// <remarks>
/// A snippet that carries the sample app with it is worse than no snippet: the reader pastes
/// it, it does not compile, and the error names a generator package and a namespace they have
/// never heard of.
/// </remarks>
[TestClass]
public class SampleCodeTests
{
    private static IEnumerable<(string Path, string Code)> PublishedCode =>
        RepositoryIndex.Samples
            .Where(s => s.Sample.Code is not null)
            .Select(s => (s.Sample.Toolkit!.SourcePath, s.Sample.Code!));

    [TestMethod]
    public void SomeSamplesPublishCode()
    {
        // Guards every test below. If extraction silently started returning nothing, they
        // would all pass while checking an empty set.
        Assert.IsTrue(
            PublishedCode.Count() > 20,
            $"Only {PublishedCode.Count()} samples published code, which suggests extraction is failing.");
    }

    [TestMethod]
    public void CodeIsAlwaysTaggedAsCSharp()
    {
        var mistagged = RepositoryIndex.Samples
            .Where(s => (s.Sample.Code is null) != (s.Sample.Language is null))
            .Select(s => s.Sample.Toolkit!.SourcePath)
            .ToList();

        Assert.AreEqual(
            0,
            mistagged.Count,
            "Samples whose code and language fields disagree:\n  " + string.Join("\n  ", mistagged));

        Assert.IsTrue(RepositoryIndex.Samples.All(s => s.Sample.Language is null or "csharp"));
    }

    [TestMethod]
    public void NoPublishedCodeCarriesSampleBrowserAttributes()
    {
        var leaked = PublishedCode
            .Where(c => c.Code.Contains("[ToolkitSample", StringComparison.Ordinal))
            .Select(c => c.Path)
            .ToList();

        Assert.AreEqual(
            0,
            leaked.Count,
            "Published code still carrying [ToolkitSample…] attributes:\n  " + string.Join("\n  ", leaked));
    }

    [TestMethod]
    public void NoPublishedCodeCarriesTheSampleAppNamespaceOrLicense()
    {
        var leaked = PublishedCode
            .Where(c => c.Code.Contains("namespace ", StringComparison.Ordinal)
                        || c.Code.Contains("Licensed to the .NET Foundation", StringComparison.Ordinal))
            .Select(c => c.Path)
            .ToList();

        Assert.AreEqual(
            0,
            leaked.Count,
            "Published code still carrying a namespace or license header:\n  " + string.Join("\n  ", leaked));
    }

    [TestMethod]
    public void NoPublishedCodeCarriesUnresolvedConditionals()
    {
        // The reader is on WinAppSDK. Publishing #if WINAPPSDK / #else asks them to decide
        // which half is theirs, and publishing the UWP half would not compile at all.
        var leaked = PublishedCode
            .Where(c => c.Code.Contains("#if", StringComparison.Ordinal)
                        || c.Code.Contains("#else", StringComparison.Ordinal)
                        || c.Code.Contains("#endif", StringComparison.Ordinal))
            .Select(c => c.Path)
            .ToList();

        Assert.AreEqual(
            0,
            leaked.Count,
            "Published code still carrying preprocessor branches:\n  " + string.Join("\n  ", leaked));
    }

    [TestMethod]
    public void NoPublishedCodeIsJustThePageWakingUp()
    {
        // InitializeComponent loads a different page's markup, and the reader's own page
        // already calls it. A constructor that does more than that is kept - that part is the
        // sample - but the call itself never is.
        var scaffolding = PublishedCode
            .Where(c => c.Code.Contains("InitializeComponent", StringComparison.Ordinal))
            .Select(c => c.Path)
            .ToList();

        Assert.AreEqual(
            0,
            scaffolding.Count,
            "Published code that is page scaffolding rather than the sample:\n  "
            + string.Join("\n  ", scaffolding));
    }

    [TestMethod]
    public void NoPublishedCodeCarriesTheOptionsPaneConverters()
    {
        // ConvertStringTo… turns an options-pane string into a real value. With the pane gone
        // nothing calls them, so publishing them is dead code the reader has to read and
        // discard.
        var leaked = PublishedCode
            .Where(c => c.Code.Contains("ConvertString", StringComparison.Ordinal))
            .Select(c => c.Path)
            .ToList();

        Assert.AreEqual(
            0,
            leaked.Count,
            "Published code still carrying option-pane converters:\n  " + string.Join("\n  ", leaked));
    }

    [TestMethod]
    public void PublishedCodeParsesAsCSharpMembers()
    {
        var malformed = new List<string>();

        foreach (var (path, code) in PublishedCode)
        {
            // Wrapped the way a reader would paste it: into a class in their own page.
            var wrapped = $"class Host {{\n{code}\n}}";
            var tree = Microsoft.CodeAnalysis.CSharp.CSharpSyntaxTree.ParseText(wrapped);

            var errors = tree.GetDiagnostics()
                .Where(d => d.Severity == Microsoft.CodeAnalysis.DiagnosticSeverity.Error)
                .ToList();

            // A type declared beside the sample is published alongside its members, and a type
            // cannot nest inside the host class in every form, so those are parsed standalone.
            if (errors.Count > 0)
            {
                var standalone = Microsoft.CodeAnalysis.CSharp.CSharpSyntaxTree.ParseText(code);
                errors = standalone.GetDiagnostics()
                    .Where(d => d.Severity == Microsoft.CodeAnalysis.DiagnosticSeverity.Error)
                    .ToList();
            }

            if (errors.Count > 0)
            {
                malformed.Add($"{path}: {errors[0].GetMessage()}");
            }
        }

        Assert.AreEqual(
            0,
            malformed.Count,
            "Published code that does not parse:\n  " + string.Join("\n  ", malformed));
    }

    [TestMethod]
    public void TypesDeclaredBesideASampleArePublishedWithIt()
    {
        // SwitchPresenterValueSample's markup switches on an enum declared in the same file.
        // Dropping it would publish markup that binds to a type the reader does not have.
        var sample = RepositoryIndex.Samples
            .Single(s => s.Sample.Toolkit!.SampleId == "SwitchPresenterValueSample")
            .Sample;

        Assert.IsNotNull(sample.Code);
        StringAssert.Contains(sample.Code, "enum Animal");
    }
}
