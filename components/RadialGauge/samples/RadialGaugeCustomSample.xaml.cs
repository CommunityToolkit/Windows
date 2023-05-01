// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.WinUI.Controls;

namespace RadialGaugeExperiment.Samples;

/// <summary>
/// An example sample page of a custom control inheriting from Panel.
/// </summary>
[ToolkitSampleTextOption("TitleText", "This is a title", Title = "Input the text")]
[ToolkitSampleMultiChoiceOption("LayoutOrientation", "Horizontal", "Vertical", Title = "Orientation")]

[ToolkitSample(id: nameof(RadialGaugeCustomSample), "Custom control", description: $"A sample for showing how to create and use a {nameof(RadialGauge)} custom control.")]
public sealed partial class RadialGaugeCustomSample : Page
{
    public RadialGaugeCustomSample()
    {
        this.InitializeComponent();
    }

    // TODO: See https://github.com/CommunityToolkit/Labs-Windows/issues/149
    public static Orientation ConvertStringToOrientation(string orientation) => orientation switch
    {
        "Vertical" => Orientation.Vertical,
        "Horizontal" => Orientation.Horizontal,
        _ => throw new System.NotImplementedException(),
    };
}
