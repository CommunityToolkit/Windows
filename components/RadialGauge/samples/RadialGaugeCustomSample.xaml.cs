// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.WinUI.Controls;

namespace RadialGaugeExperiment.Samples;

/// <summary>
/// An example sample page of a custom control inheriting from Panel.
/// </summary>
[ToolkitSampleBoolOption("Enabled", true, Title = "IsEnabled")]
[ToolkitSampleNumericOption("Value", 120, 0, 240, 1, false, Title = "Value")]
[ToolkitSampleNumericOption("StepSize", 30, 5, 30, 1, false, Title = "StepSize")]
[ToolkitSampleBoolOption("IsInteractive", true, Title = "IsInteractive")]
[ToolkitSampleNumericOption("TickSpacing", 30, 10, 30, 1, false, Title = "TickSpacing")]
[ToolkitSampleNumericOption("ScaleWidth", 12, 4, 50, 1, false, Title = "ScaleWidth")]
[ToolkitSampleNumericOption("MinAngle", 210, 0, 360, 1, false, Title = "MinAngle")]
[ToolkitSampleNumericOption("MaxAngle", 150, 0, 360, 1, false, Title = "MaxAngle")]
[ToolkitSampleNumericOption("NeedleWidth", 0, 1, 10, 1, false, Title = "NeedleWidth")]
[ToolkitSampleNumericOption("NeedleLength", 0, 20, 100, 1, false, Title = "NeedleLength")]
[ToolkitSampleNumericOption("TickLength", 8, 0, 30, 1, false, Title = "TickLength")]
[ToolkitSampleNumericOption("TickWidth", 2, 0, 30, 1, false, Title = "TickWidth")]
[ToolkitSampleNumericOption("ScalePadding", 0, 0, 100, 1, false, Title = "ScalePadding")]
[ToolkitSampleNumericOption("TickPadding", 16, 0, 100, 1, false, Title = "TickPadding")]
[ToolkitSampleNumericOption("ScaleTickWidth", 4, 2, 20, 1, false, Title = "ScaleTickWidth")]

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
