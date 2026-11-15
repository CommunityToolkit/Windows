// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace CommunityToolkit.SampleIndex.Tests;

/// <summary>
/// Builds a miniature repository on disk for the tests that are about the exporter rather than
/// about this repository's samples.
/// </summary>
/// <remarks>
/// Small on purpose. It has to satisfy <see cref="IndexGenerator.FindRepoRoot"/> and carry one
/// component with one documented sample; anything more would make the tests that use it harder
/// to read without making them test more.
/// </remarks>
internal static class Fixture
{
    public const string SampleXaml =
        """
        <Page x:Class="Widgets.WidgetSample"
              xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
              xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml">
            <TextBlock Text="Hello" />
        </Page>
        """;

    /// <summary>Create a fixture repository and return its root.</summary>
    /// <param name="components">
    /// Component folder names, created in the order given. The generated index must not depend
    /// on that order.
    /// </param>
    public static string Create(params string[] components)
    {
        if (components.Length == 0)
        {
            components = ["Widgets"];
        }

        var root = Directory.CreateTempSubdirectory("sample-index-").FullName;

        // What FindRepoRoot looks for.
        File.WriteAllText(Path.Combine(root, "Directory.Build.props"), "<Project />");
        Directory.CreateDirectory(Path.Combine(root, "components"));

        foreach (var component in components)
        {
            var samples = Directory.CreateDirectory(
                Path.Combine(root, "components", component, "samples")).FullName;

            File.WriteAllText(Path.Combine(samples, $"{component}.md"), Document(component));
            File.WriteAllText(Path.Combine(samples, "WidgetSample.xaml.cs"), Declaration("WidgetSample", "Widget"));
            File.WriteAllText(Path.Combine(samples, "WidgetSample.xaml"), SampleXaml);
        }

        return root;
    }

    public static string Declaration(string id, string displayName) =>
        $$"""
        using CommunityToolkit.Tooling.SampleGen.Attributes;

        namespace Widgets;

        [ToolkitSample(id: nameof({{id}}), "{{displayName}}", description: "Shows a widget.")]
        public sealed partial class {{id}} : Page;
        """;

    private static string Document(string component) =>
        $"""
        ---
        title: {component}
        author: nobody
        description: A component used by the exporter's own tests.
        keywords: {component}
        category: Controls
        subcategory: Layout
        ---

        Prose introducing the sample.

        > [!SAMPLE WidgetSample]
        """;
}
