// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.WinUI.Controls;

namespace PrimitivesExperiment.Samples;

[ToolkitSampleNumericOption("FirstColumn", initial: 1, min: 0, max: 3, step: 1, Title = "FirstColumn")]
[ToolkitSampleMultiChoiceOption("OrientationProperty", "Horizontal", "Vertical", Title = "Orientation")]
[ToolkitSampleNumericOption("Rows", initial: 0, min: 0, max: 5, step: 1, Title = "Rows")]
[ToolkitSampleNumericOption("Columns", initial: 0, min: 0, max: 5, step: 1, Title = "Columns")]
[ToolkitSampleNumericOption("Item1RowSpan", initial: 2, min: 1, max: 3, step: 1, Title = "Item 1 RowSpan")]
[ToolkitSampleNumericOption("Item1ColumnSpan", initial: 2, min: 1, max: 3, step: 1, Title = "Item 1 ColumnSpan")]

[ToolkitSample(id: nameof(UniformGridSample), "UniformGrid", description: $"A sample for showing how to create and use a {nameof(UniformGrid)}.")]
public sealed partial class UniformGridSample : Page
{
    public UniformGridSample()
    {
        this.InitializeComponent();
    }

    public static Orientation ConvertStringToOrientation(string orientation) => orientation switch
    {
        "Horizontal" => Orientation.Horizontal,
        "Vertical" => Orientation.Vertical,
        _ => throw new System.NotImplementedException(),
    };
}
