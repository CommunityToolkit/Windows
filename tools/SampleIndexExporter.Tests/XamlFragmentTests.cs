// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CommunityToolkit.SampleIndex.Tests;

/// <summary>
/// Unit tests for turning a sample page into a pasteable fragment.
/// </summary>
[TestClass]
public class XamlFragmentTests
{
    private const string PageHeader =
        """
        <Page x:Class="Sample.MySample"
              xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
              xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
              xmlns:controls="using:CommunityToolkit.WinUI.Controls"
              xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
              xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
              mc:Ignorable="d">
        """;

    [TestMethod]
    public void ThePageWrapperIsRemoved()
    {
        var result = Extract("<Button Content=\"Hi\" />");

        Assert.IsNull(result.Error);
        Assert.AreEqual("<Button Content=\"Hi\" />", result.Xaml);
    }

    [TestMethod]
    public void OnlyTheImportsTheBodyUsesAreKept()
    {
        // A snippet that hands over six xmlns declarations for one control makes the reader
        // work out which of them they need. Keeping only what the body uses removes that work.
        var result = Extract("<controls:SettingsCard Header=\"Hi\" />");

        CollectionAssert.AreEqual(
            new[] { "xmlns:controls=\"using:CommunityToolkit.WinUI.Controls\"" },
            result.XmlnsImports);
    }

    [TestMethod]
    public void DesignTimeImportsAreNeverKept()
    {
        var result = Extract("<Border d:Background=\"Red\" />");

        Assert.AreEqual(0, result.XmlnsImports.Count(i => i.Contains("blend") || i.Contains("markup-compatibility")));
    }

    [TestMethod]
    public void AnOptionBindingBecomesItsDefaultValue()
    {
        var result = Extract(
            "<Button IsEnabled=\"{x:Bind IsCardEnabled}\" />",
            new SampleOption("IsCardEnabled", "True"));

        StringAssert.Contains(result.Xaml!, "IsEnabled=\"True\"");
        CollectionAssert.Contains(result.OptionsResolved, "IsCardEnabled=True");
    }

    [TestMethod]
    public void AnAttributeNamedLikeAnOptionIsNotMistakenForABinding()
    {
        // IsAlwaysOn="{x:Bind IsAlwaysOn}" mentions the option name twice: once as the
        // attribute and once as the binding path. Matching on raw text flagged the attribute
        // name and reported a perfectly resolvable sample as unpasteable.
        var result = Extract(
            "<Button IsAlwaysOn=\"{x:Bind IsAlwaysOn}\" />",
            new SampleOption("IsAlwaysOn", "True"));

        Assert.IsNull(result.Error);
        StringAssert.Contains(result.Xaml!, "IsAlwaysOn=\"True\"");
    }

    [TestMethod]
    public void AnOptionBindingWithNoStaticValueDropsTheWholeAttribute()
    {
        // Leaving the attribute with an unresolved value would not compile, and inventing a
        // value would show markup the author never wrote. Removing the attribute gives the
        // control its own default, which is the honest answer.
        var result = Extract(
            "<Button Shape=\"{x:Bind Convert(Shape), Mode=OneWay}\" Content=\"Hi\" />",
            new SampleOption("Shape", null));

        Assert.IsNull(result.Error);
        Assert.IsFalse(result.Xaml!.Contains("Shape", StringComparison.Ordinal));
        StringAssert.Contains(result.Xaml!, "Content=\"Hi\"");
        CollectionAssert.Contains(result.OptionBindingsDropped, "Shape");
    }

    [TestMethod]
    public void ABindingToTheSamplePageIsSettled()
    {
        // ElementName points at the Page root, which is exactly the element being discarded.
        var result = Extract(
            """
            <StackPanel x:Name="ThisSamplePage">
                <Button IsEnabled="{Binding IsOn, ElementName=ThisSamplePage}" />
            </StackPanel>
            """,
            new SampleOption("IsOn", "True"));

        Assert.IsNull(result.Error);
        Assert.IsFalse(result.Xaml!.Contains("ElementName", StringComparison.Ordinal));
    }

    [TestMethod]
    public void ABindingBetweenTwoElementsInTheFragmentIsLeftAlone()
    {
        // This one works after a paste, because both ends of it come along.
        var result = Extract(
            """
            <StackPanel>
                <CheckBox x:Name="Toggle" />
                <Button IsEnabled="{Binding IsChecked, ElementName=Toggle}" />
            </StackPanel>
            """);

        Assert.IsNull(result.Error);
        StringAssert.Contains(result.Xaml!, "ElementName=Toggle");
    }

    [TestMethod]
    public void CharacterReferencesSurviveExtraction()
    {
        // Glyphs are written as &#xE799; and mean nothing once decoded into a private-use
        // character, so the fragment is taken as source text rather than re-serialized.
        var result = Extract("<FontIcon Glyph=\"&#xE799;\" />");

        StringAssert.Contains(result.Xaml!, "&#xE799;");
    }

    [TestMethod]
    public void AnEmptyPageIsWithheldRatherThanPublishedBlank()
    {
        var result = Extract("  \n  ");

        Assert.IsNotNull(result.Error);
        Assert.IsNull(result.Xaml);
    }

    [TestMethod]
    public void MalformedMarkupIsWithheldRatherThanPublishedBroken()
    {
        var result = XamlFragment.Extract("<Page><Button></Page>", []);

        Assert.IsNotNull(result.Error);
        Assert.IsNull(result.Xaml);
    }

    [TestMethod]
    public void UnsettledBindingsFindsASampleAppReference()
    {
        var leftovers = XamlFragment.UnsettledBindings(
            "<Button IsEnabled=\"{x:Bind IsOn}\" />",
            [new SampleOption("IsOn", "True")]);

        Assert.AreEqual(1, leftovers.Count);
    }

    [TestMethod]
    public void UnsettledBindingsFindsABindingToAMissingElement()
    {
        var leftovers = XamlFragment.UnsettledBindings(
            "<Button IsEnabled=\"{Binding IsOn, ElementName=ThisSamplePage}\" />",
            []);

        Assert.AreEqual(1, leftovers.Count);
    }

    [TestMethod]
    public void AConditionalWindowsPrefixIsRemoved()
    {
        // xmlns:win="...presentation" is Uno's "Windows only" prefix, and on Windows it names
        // the same elements as no prefix at all.
        var result = XamlFragment.Extract(
            """
            <Page x:Class="Sample.MySample"
                  xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
                  xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
                  xmlns:win="http://schemas.microsoft.com/winfx/2006/xaml/presentation">
            <win:TextBox win:IsTextSelectionEnabled="True" Text="Hi" />
            </Page>
            """,
            []);

        Assert.IsNull(result.Error);
        Assert.AreEqual("<TextBox IsTextSelectionEnabled=\"True\" Text=\"Hi\" />", result.Xaml);
        Assert.AreEqual(0, result.XmlnsImports.Count);
    }

    [TestMethod]
    public void PageResourcesMoveOntoTheElementThatSurvives()
    {
        // <Page.Resources> belongs to the page the fragment discards. Left as it is, the
        // snippet fails to compile the moment it is pasted anywhere that is not a Page.
        var result = Extract(
            """
            <Page.Resources>
                <SolidColorBrush x:Key="Accent" Color="Red" />
            </Page.Resources>

            <Border Background="{StaticResource Accent}">
                <TextBlock Text="Hi" />
            </Border>
            """);

        Assert.IsNull(result.Error);
        Assert.IsFalse(result.Xaml!.Contains("Page.", StringComparison.Ordinal));
        StringAssert.Contains(result.Xaml!, "<Border.Resources>");
        StringAssert.StartsWith(result.Xaml!, "<Border");
    }

    [TestMethod]
    public void PageResourcesOpenUpASelfClosingHostElement()
    {
        var result = Extract(
            """
            <Page.Resources>
                <SolidColorBrush x:Key="Accent" Color="Red" />
            </Page.Resources>

            <Border Background="{StaticResource Accent}" />
            """);

        Assert.IsNull(result.Error);
        StringAssert.Contains(result.Xaml!, "<Border.Resources>");
        StringAssert.Contains(result.Xaml!, "</Border>");
    }

    [TestMethod]
    public void UnsettledBindingsAcceptsMarkupWithNoBindings()
    {
        var leftovers = XamlFragment.UnsettledBindings("<Button IsEnabled=\"True\" />", []);

        Assert.AreEqual(0, leftovers.Count);
    }

    [TestMethod]
    public void UnsettledBindingsAcceptsStaticResourceLookups()
    {
        // StaticResource resolves against the reader's own resource dictionary, and the ones
        // used here are system styles that every WinUI app has.
        var leftovers = XamlFragment.UnsettledBindings(
            "<Button Style=\"{StaticResource AccentButtonStyle}\" />",
            []);

        Assert.AreEqual(0, leftovers.Count);
    }

    private static XamlExtraction Extract(string body, params SampleOption[] options) =>
        XamlFragment.Extract($"{PageHeader}\n{body}\n</Page>", options);
}
