// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text.Json;
using System.Text.RegularExpressions;
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
public partial class SampleCodeTests
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
        // Read back from the committed file: the language tag is what a consumer hands to a
        // syntax highlighter, so what matters is the value in the artifact. Asserting against
        // the in-memory object would only restate the line in IndexGenerator that sets it.
        using var document = JsonDocument.Parse(RepositoryIndex.CommittedJson);

        var mistagged = new List<string>();

        foreach (var control in document.RootElement.GetProperty("controls").EnumerateArray())
        {
            foreach (var sample in control.GetProperty("samples").EnumerateArray())
            {
                var hasCode = sample.TryGetProperty("code", out var code) && code.GetString() is not null;
                var language = sample.TryGetProperty("language", out var tag) ? tag.GetString() : null;

                if (hasCode != (language is not null) || language is not (null or "csharp"))
                {
                    mistagged.Add($"{control.GetProperty("id").GetString()}: language '{language ?? "(none)"}'");
                }
            }
        }

        Assert.AreEqual(
            0,
            mistagged.Count,
            "Samples whose code and language fields disagree:\n  " + string.Join("\n  ", mistagged));
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

    [TestMethod]
    public void SomeControlsPublishTheNamespacesTheirCodeNeeds()
    {
        // Guards every import test below, and the field itself: consumers prepend this list to
        // each sample's code, so an extractor that quietly stopped filling it would publish C#
        // that names toolkit types and says nowhere which namespace they come from.
        var publishing = RepositoryIndex.Index.Controls.Count(c => c.Usings is { Count: > 0 });

        Assert.IsTrue(
            publishing > 10,
            $"Only {publishing} entries published any usings, which suggests imports are being dropped.");
    }

    [TestMethod]
    public void AControlPublishesTheNamespaceItsCodeCallsInto()
    {
        // NetworkHelperSample's code is NetworkHelper.Instance.ConnectionInformation and
        // nothing else, so the one namespace its file imports is the one the reader needs.
        var control = RepositoryIndex.Index.Controls.Single(c => c.Id == "networkhelper");

        CollectionAssert.AreEqual(new[] { "CommunityToolkit.WinUI.Helpers" }, control.Usings);
    }

    [TestMethod]
    public void AMultiTargetAliasIsPublishedAsTheNamespaceItNames()
    {
        // Samples alias a type to name one platform's version of it while multi-targeting. With
        // the WinAppSDK branch already resolved, what the reader needs is the namespace that
        // type lives in — nothing in the published code mentions the alias is an alias.
        var timers = RepositoryIndex.Index.Controls.Single(c => c.Id == "dispatcherqueuetimerextensions");
        var wrapPanel = RepositoryIndex.Index.Controls.Single(c => c.Id == "wrappanel");

        CollectionAssert.Contains(timers.Usings, "Microsoft.UI.Dispatching");
        CollectionAssert.Contains(wrapPanel.Usings, "CommunityToolkit.WinUI.Controls");

        // And the extension method the same sample calls, which enters scope through its
        // namespace without the code ever naming the class that declares it.
        CollectionAssert.Contains(timers.Usings, "CommunityToolkit.WinUI");
        CollectionAssert.Contains(wrapPanel.Usings, "CommunityToolkit.WinUI");
    }

    [TestMethod]
    public void ImportsOnlyTheScaffoldingNeededAreNotPublished()
    {
        // IsNullOrEmptyStateTriggerSample's file imports CommunityToolkit.WinUI for the trigger
        // its markup uses, but the code that survives extraction is two click handlers that
        // touch nothing in that namespace. Publishing the import anyway would tell the reader
        // to add a using for code they were never handed.
        var control = RepositoryIndex.Index.Controls.Single(c => c.Id == "triggers");

        Assert.IsNull(
            control.Usings,
            "Imports that only the discarded page scaffolding needed must not be published: "
            + string.Join(", ", control.Usings ?? []));
    }

    [TestMethod]
    public void NoControlPublishesANamespaceOnlyTheSampleAppHas()
    {
        // The Connected Animations sample navigates to pages declared in a namespace of this
        // repository's sample app. That using resolves here and nowhere else, so prepending it
        // to a snippet would break the paste in the reader's project.
        var sampleApp = SampleAppNamespaces();

        var leaked = RepositoryIndex.Index.Controls
            .SelectMany(c => (c.Usings ?? []).Select(u => (Control: c.Id, Using: u)))
            .Where(u => sampleApp.Contains(u.Using))
            .Select(u => $"{u.Control}: {u.Using}")
            .ToList();

        Assert.AreEqual(
            0,
            leaked.Count,
            "Published usings naming a sample-app namespace:\n  " + string.Join("\n  ", leaked));
    }

    [TestMethod]
    public void TheSampleAppNamespacesWereActuallyFound()
    {
        // Guards the test above: an empty set would let it pass while checking nothing.
        Assert.IsTrue(
            SampleAppNamespaces().Contains("AnimationsExperiment.Samples.ConnectedAnimations"),
            "The sample app's namespaces could not be read, so the gate above checked nothing.");
    }

    [TestMethod]
    public void NoPublishedCodeRepeatsTheImportsTheControlDeclares()
    {
        // The contract puts the imports on the control and has consumers prepend them, so a
        // snippet that carries its own would hand the reader the same using twice.
        var leaked = PublishedCode
            .Where(c => UsingDirectiveRegex().IsMatch(c.Code))
            .Select(c => c.Path)
            .ToList();

        Assert.AreEqual(
            0,
            leaked.Count,
            "Published code carrying its own using directives:\n  " + string.Join("\n  ", leaked));
    }

    [TestMethod]
    public void OnlyControlsThatPublishCodePublishImports()
    {
        // Imports exist to make the C# compile. An entry that publishes none but still carries
        // a usings list would have a consumer prepending using lines to pure markup.
        var orphaned = RepositoryIndex.Index.Controls
            .Where(c => c.Usings is { Count: > 0 } && c.Samples.All(s => s.Code is null))
            .Select(c => c.Id)
            .ToList();

        Assert.AreEqual(
            0,
            orphaned.Count,
            "Entries publishing usings with no code to import for:\n  " + string.Join("\n  ", orphaned));
    }

    [GeneratedRegex(@"^\s*using\s+[\w.]+\s*;", RegexOptions.Multiline)]
    private static partial Regex UsingDirectiveRegex();

    /// <summary>Every namespace this repository's sample app declares.</summary>
    /// <remarks>
    /// Read from the sources rather than matched against the <c>…Experiment.Samples</c> naming
    /// convention, so the gate keeps working if a component ever names its sample pages
    /// differently.
    /// </remarks>
    private static HashSet<string> SampleAppNamespaces()
    {
        var samples = $"{Path.DirectorySeparatorChar}samples{Path.DirectorySeparatorChar}";

        return Directory
            .EnumerateFiles(Path.Combine(RepositoryIndex.Root, "components"), "*.cs", SearchOption.AllDirectories)
            .Where(path => path.Contains(samples, StringComparison.Ordinal))
            .SelectMany(path => Microsoft.CodeAnalysis.CSharp.CSharpSyntaxTree
                .ParseText(File.ReadAllText(path))
                .GetRoot()
                .DescendantNodes()
                .OfType<Microsoft.CodeAnalysis.CSharp.Syntax.BaseNamespaceDeclarationSyntax>()
                .Select(declaration => declaration.Name.ToString()))
            .ToHashSet(StringComparer.Ordinal);
    }
}
