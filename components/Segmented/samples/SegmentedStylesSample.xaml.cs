// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace SegmentedExperiment.Samples;

/// <summary>
/// An sample that shows how the Segmented control has multiple built-in styles.
/// </summary>
[ToolkitSampleMultiChoiceOption("SelectionMode", "Single", "Multiple", Title = "Selection mode")]

[ToolkitSample(id: nameof(SegmentedStylesSample), "Additional styles", description: "A sample on how to use different built-in styles.")]
public sealed partial class SegmentedStylesSample : Page
{
    public SegmentedStylesSample()
    {
        this.InitializeComponent();
    }
    public static ListViewSelectionMode ConvertStringToSelectionMode(string selectionMode) => selectionMode switch
    {
        "Single" => ListViewSelectionMode.Single,
        "Multiple" => ListViewSelectionMode.Multiple,
        _ => throw new System.NotImplementedException(),
    };
}
