// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text.RegularExpressions;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CommunityToolkit.SampleIndex.Tests;

/// <summary>
/// The pasteability gate: every published snippet has to work when pasted into a page.
/// </summary>
/// <remarks>
/// This is the whole point of the index. A consumer offers the markup to someone who pastes it
/// into their own app, so a snippet that references the sample app's page, or a namespace prefix
/// the consumer was never told to declare, is worse than no snippet at all — it fails after the
/// paste, in their file, with an error that points at their code.
/// </remarks>
[TestClass]
public class PasteabilityTests
{
    [TestMethod]
    public void EverySampleHasMarkup()
    {
        var empty = RepositoryIndex.Samples
            .Where(s => string.IsNullOrWhiteSpace(s.Sample.Xaml))
            .Select(s => s.Sample.Toolkit!.SourcePath)
            .ToList();

        Assert.AreEqual(
            0,
            empty.Count,
            "Published samples with no markup:\n  " + string.Join("\n  ", empty));
    }

    [TestMethod]
    public void EverySampleIsWellFormedXml()
    {
        var malformed = new List<string>();

        foreach (var (_, sample) in RepositoryIndex.Samples)
        {
            var declarations = string.Join(
                ' ',
                (sample.XmlnsImports ?? []).Select(i => i));

            var document =
                "<Root xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" "
                + "xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\" "
                + declarations + ">" + sample.Xaml + "</Root>";

            try
            {
                XDocument.Parse(document);
            }
            catch (Exception ex)
            {
                malformed.Add($"{sample.Toolkit!.SourcePath}: {ex.Message}");
            }
        }

        Assert.AreEqual(
            0,
            malformed.Count,
            "Published samples that do not parse under their own declared imports:\n  "
            + string.Join("\n  ", malformed));
    }

    [TestMethod]
    public void NoSampleCarriesSampleAppBindings()
    {
        // The sample app binds its option controls into the markup. Those bindings resolve
        // against a generated view model that only exists in the sample app, so anything left
        // unsettled here would paste in and then fail to compile. The options have to come from
        // the declarations: checking against an empty list would only catch dangling element
        // names and would quietly pass everything else.
        var options = SampleOptionsBySampleId();
        var unsettled = new List<string>();

        foreach (var (_, sample) in RepositoryIndex.Samples)
        {
            var declared = options.GetValueOrDefault(sample.Toolkit!.SampleId, []);
            var leftovers = XamlFragment.UnsettledBindings(sample.Xaml!, declared);
            if (leftovers.Count > 0)
            {
                unsettled.Add($"{sample.Toolkit.SourcePath}: {string.Join(", ", leftovers)}");
            }
        }

        Assert.AreEqual(
            0,
            unsettled.Count,
            "Published samples still referencing the sample app:\n  " + string.Join("\n  ", unsettled));
    }

    [TestMethod]
    public void EverySampleWithOptionsActuallyHadThemChecked()
    {
        // Guards the test above. If the source paths and sample ids ever stopped lining up,
        // every lookup would miss, every sample would be checked against no options, and the
        // gate would pass while checking nothing.
        var options = SampleOptionsBySampleId();
        var matched = RepositoryIndex.Samples.Count(s => options.ContainsKey(s.Sample.Toolkit!.SampleId));

        Assert.AreEqual(
            RepositoryIndex.Samples.Count(),
            matched,
            "Some published samples could not be matched back to their declaration.");
    }

    private static Dictionary<string, IReadOnlyList<SampleOption>> SampleOptionsBySampleId()
    {
        return Directory
            .EnumerateFiles(Path.Combine(RepositoryIndex.Root, "components"), "*.xaml.cs", SearchOption.AllDirectories)
            .Where(path => path.Contains($"{Path.DirectorySeparatorChar}samples{Path.DirectorySeparatorChar}", StringComparison.Ordinal))
            .SelectMany(path => SampleDeclaration.Parse(path, Path.GetRelativePath(RepositoryIndex.Root, path)))
            .ToDictionary(d => d.Id, d => d.Options, StringComparer.Ordinal);
    }

    [TestMethod]
    public void NoSampleCarriesThePageItWasExtractedFrom()
    {
        var wrappers = RepositoryIndex.Samples
            .Where(s => s.Sample.Xaml!.Contains("x:Class", StringComparison.Ordinal)
                        || s.Sample.Xaml!.StartsWith("<Page", StringComparison.Ordinal)
                        || s.Sample.Xaml!.Contains("<UserControl", StringComparison.Ordinal))
            .Select(s => s.Sample.Toolkit!.SourcePath)
            .ToList();

        Assert.AreEqual(
            0,
            wrappers.Count,
            "Published samples that still include the sample page wrapper:\n  "
            + string.Join("\n  ", wrappers));
    }

    [TestMethod]
    public void EveryPrefixUsedIsDeclared()
    {
        // A snippet that uses controls: without saying what controls: means sends the reader
        // hunting through the repository for an xmlns they were supposed to be handed.
        var undeclared = new List<string>();

        foreach (var (_, sample) in RepositoryIndex.Samples)
        {
            var declared = (sample.XmlnsImports ?? [])
                .Select(import => import["xmlns:".Length..import.IndexOf('=')])
                .ToHashSet(StringComparer.Ordinal);

            foreach (var prefix in PrefixesUsed(sample.Xaml!))
            {
                if (prefix is "x" or "xmlns" || declared.Contains(prefix))
                {
                    continue;
                }

                undeclared.Add($"{sample.Toolkit!.SourcePath}: {prefix}");
            }
        }

        Assert.AreEqual(
            0,
            undeclared.Count,
            "Published samples using a prefix they do not declare:\n  "
            + string.Join("\n  ", undeclared.Distinct()));
    }

    [TestMethod]
    public void NoSampleUsesDesignTimeMarkup()
    {
        // Design-time attributes (d:, mc:) are Visual Studio scaffolding. They carry two more
        // xmlns declarations and do nothing at run time, so shipping them makes the snippet
        // longer and the paste more fragile for no benefit.
        var designTime = RepositoryIndex.Samples
            .Where(s => s.Sample.Xaml!.Contains(" d:", StringComparison.Ordinal)
                        || s.Sample.Xaml!.Contains(" mc:", StringComparison.Ordinal))
            .Select(s => s.Sample.Toolkit!.SourcePath)
            .ToList();

        Assert.AreEqual(
            0,
            designTime.Count,
            "Published samples carrying design-time markup:\n  " + string.Join("\n  ", designTime));
    }

    /// <summary>
    /// Every prefix the markup relies on: element names, attached property names and markup
    /// extensions all fail the same way when the prefix was never declared.
    /// </summary>
    /// <remarks>
    /// Comments are removed first. Samples sometimes keep a commented-out alternative — the
    /// Behaviors sample keeps a <c>PlaySoundAction</c> that way — and a prefix inside a comment
    /// is never resolved, so counting it would report a problem that cannot happen.
    /// </remarks>
    private static IEnumerable<string> PrefixesUsed(string xaml)
    {
        var text = Regex.Replace(xaml, "<!--.*?-->", string.Empty, RegexOptions.Singleline);

        foreach (Match match in Regex.Matches(text, @"</?(?<prefix>[A-Za-z_][\w.-]*):"))
        {
            yield return match.Groups["prefix"].Value;
        }

        foreach (Match match in Regex.Matches(text, @"\s(?<prefix>[A-Za-z_][\w.-]*):[A-Za-z_][\w.]*\s*="))
        {
            yield return match.Groups["prefix"].Value;
        }

        foreach (Match match in Regex.Matches(text, @"\{\s*(?<prefix>[A-Za-z_][\w.-]*):"))
        {
            yield return match.Groups["prefix"].Value;
        }
    }
}
