// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CommunityToolkit.SampleIndex.Tests;

/// <summary>
/// Unit tests for reading a <c>[ToolkitSample]</c> declaration.
/// </summary>
/// <remarks>
/// These are the forms the samples actually use. Each one was a way the index could have
/// disagreed with the sample app about what a sample is called or what its options default to,
/// which is the one thing a reader has no way to notice.
/// </remarks>
[TestClass]
public class SampleDeclarationTests
{
    [TestMethod]
    public void DisplayNameIsReadWhenArgumentsMixNamedAndPositional()
    {
        // The most common form in the repository. The display name is the second parameter but
        // the first positional argument, so reading arguments by position alone loses it.
        var declaration = ParseSingle(
            """
            [ToolkitSample(id: nameof(MySample), "My Sample", description: "Shows a thing.")]
            public sealed partial class MySample : Page;
            """);

        Assert.AreEqual("MySample", declaration.Id);
        Assert.AreEqual("My Sample", declaration.DisplayName);
        Assert.AreEqual("Shows a thing.", declaration.Description);
    }

    [TestMethod]
    public void DisplayNameIsReadWhenEveryArgumentIsPositional()
    {
        var declaration = ParseSingle(
            """
            [ToolkitSample(nameof(MySample), "My Sample", "Shows a thing.")]
            public sealed partial class MySample : Page;
            """);

        Assert.AreEqual("My Sample", declaration.DisplayName);
    }

    [TestMethod]
    public void DisplayNameIsReadWhenEveryArgumentIsNamed()
    {
        var declaration = ParseSingle(
            """
            [ToolkitSample(description: "Shows a thing.", displayName: "My Sample", id: nameof(MySample))]
            public sealed partial class MySample : Page;
            """);

        Assert.AreEqual("My Sample", declaration.DisplayName);
    }

    [TestMethod]
    public void DisplayNameIsReadFromNameOf()
    {
        var declaration = ParseSingle(
            """
            [ToolkitSample(id: nameof(SettingsCardSample), nameof(SettingsCard), description: "")]
            public sealed partial class SettingsCardSample : Page;
            """);

        Assert.AreEqual("SettingsCard", declaration.DisplayName);
    }

    [TestMethod]
    public void DisplayNameIsReadFromAnInterpolatedString()
    {
        var declaration = ParseSingle(
            """
            [ToolkitSample(id: nameof(FocusSample), $"{nameof(FocusBehavior)}: Lists", description: "")]
            public sealed partial class FocusSample : Page;
            """);

        Assert.AreEqual("FocusBehavior: Lists", declaration.DisplayName);
    }

    [TestMethod]
    public void AnUnreadableArgumentIsReportedRatherThanGuessed()
    {
        var declaration = ParseSingle(
            """
            [ToolkitSample(id: nameof(MySample), BuildName(), description: "")]
            public sealed partial class MySample : Page;
            """);

        Assert.IsNull(declaration.DisplayName);
        Assert.AreEqual(1, declaration.UnreadableArguments.Count);
        StringAssert.Contains(declaration.UnreadableArguments[0], "displayName");
    }

    [TestMethod]
    public void BoolOptionTakesItsDefaultState()
    {
        var declaration = ParseSingle(
            """
            [ToolkitSample(id: nameof(MySample), "My Sample", description: "")]
            [ToolkitSampleBoolOption("IsCardEnabled", true, Title = "Is card enabled")]
            public sealed partial class MySample : Page;
            """);

        var option = declaration.Options.Single();
        Assert.AreEqual("IsCardEnabled", option.Name);
        Assert.AreEqual("True", option.DefaultValue);
    }

    [TestMethod]
    public void NumericOptionTakesItsInitialValue()
    {
        var declaration = ParseSingle(
            """
            [ToolkitSample(id: nameof(MySample), "My Sample", description: "")]
            [ToolkitSampleNumericOption("Spacing", 8, 0, 48, 1, false, Title = "Spacing")]
            public sealed partial class MySample : Page;
            """);

        var option = declaration.Options.Single();
        Assert.AreEqual("Spacing", option.Name);
        Assert.AreEqual("8", option.DefaultValue);
    }

    [TestMethod]
    public void NumericOptionWithOnlyNamedRangeArgumentsStillHasADefault()
    {
        var declaration = ParseSingle(
            """
            [ToolkitSample(id: nameof(MySample), "My Sample", description: "")]
            [ToolkitSampleNumericOption("Spacing", min: 0, max: 48, initial: 8)]
            public sealed partial class MySample : Page;
            """);

        Assert.AreEqual("8", declaration.Options.Single().DefaultValue);
    }

    [TestMethod]
    public void MultiChoiceOptionTakesItsFirstChoice()
    {
        // The sample app selects the first choice, so anything else here would publish markup
        // showing a state the gallery never opens in.
        var declaration = ParseSingle(
            """
            [ToolkitSample(id: nameof(MySample), "My Sample", description: "")]
            [ToolkitSampleMultiChoiceOption("Orientation", "Horizontal", "Vertical", Title = "Orientation")]
            public sealed partial class MySample : Page;
            """);

        Assert.AreEqual("Horizontal", declaration.Options.Single().DefaultValue);
    }

    [TestMethod]
    public void MultiChoiceLabelsAreStrippedFromTheValue()
    {
        // "Label : Value" is display text on the left and the real value on the right. Pasting
        // the label would produce markup that does not compile.
        var declaration = ParseSingle(
            """
            [ToolkitSample(id: nameof(MySample), "My Sample", description: "")]
            [ToolkitSampleMultiChoiceOption("Stretch", "Fill the box : UniformToFill", "Fit : Uniform")]
            public sealed partial class MySample : Page;
            """);

        Assert.AreEqual("UniformToFill", declaration.Options.Single().DefaultValue);
    }

    [TestMethod]
    public void TextOptionDefaultsToItsPlaceholder()
    {
        // The sample app seeds the text box with the placeholder and binds Text to it, so the
        // placeholder is what a reader sees the sample doing, not just prompt text.
        var declaration = ParseSingle(
            """
            [ToolkitSample(id: nameof(MySample), "My Sample", description: "")]
            [ToolkitSampleTextOption("Caption", placeholderText: "Enter a caption")]
            public sealed partial class MySample : Page;
            """);

        Assert.AreEqual("Enter a caption", declaration.Options.Single().DefaultValue);
    }

    [TestMethod]
    public void AttributesOnOtherClassesAreNotMixedTogether()
    {
        var declarations = ParseAll(
            """
            [ToolkitSample(id: nameof(First), "First", description: "")]
            [ToolkitSampleBoolOption("IsOn", true)]
            public sealed partial class First : Page;

            [ToolkitSample(id: nameof(Second), "Second", description: "")]
            public sealed partial class Second : Page;
            """);

        Assert.AreEqual(2, declarations.Count);
        Assert.AreEqual(1, declarations[0].Options.Count);
        Assert.AreEqual(0, declarations[1].Options.Count);
    }

    [TestMethod]
    public void AClassWithNoSampleAttributeIsIgnored()
    {
        var declarations = ParseAll("public sealed partial class NotASample : Page;");

        Assert.AreEqual(0, declarations.Count);
    }

    private SampleDeclaration ParseSingle(string source) => ParseAll(source).Single();

    private static List<SampleDeclaration> ParseAll(string source)
    {
        // Deliberately outside the repository: a .xaml.cs written anywhere under the test
        // project would be picked up by the SDK's implicit compile glob on the next build, and
        // these fragments are not compilable C# files.
        var directory = Path.Combine(Path.GetTempPath(), "toolkit-sample-index-tests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(directory);
        var path = Path.Combine(directory, "Test.xaml.cs");

        try
        {
            File.WriteAllText(path, source);
            return SampleDeclaration.Parse(path, "components/Test/samples/Test.xaml.cs");
        }
        finally
        {
            Directory.Delete(directory, recursive: true);
        }
    }
}
