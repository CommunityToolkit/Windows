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
[ToolkitSampleNumericOption("TickSpacing", 15, 10, 30, 1, false, Title = "TickSpacing")]
[ToolkitSampleNumericOption("ScaleWidth", 12, 4, 50, 1, false, Title = "ScaleWidth")]
[ToolkitSampleNumericOption("MinAngle", -150, -150, 360, 1, false, Title = "MinAngle")]
[ToolkitSampleNumericOption("MaxAngle", 150, 0, 360, 1, false, Title = "MaxAngle")]
[ToolkitSampleNumericOption("NeedleWidth", 4, 0, 10, 1, false, Title = "NeedleWidth")]
[ToolkitSampleNumericOption("NeedleLength", 60, 0, 100, 1, false, Title = "NeedleLength")]
[ToolkitSampleNumericOption("TickLength", 6, 0, 30, 1, false, Title = "TickLength")]
[ToolkitSampleNumericOption("TickWidth", 2, 0, 30, 1, false, Title = "TickWidth")]
[ToolkitSampleNumericOption("ScalePadding", 0, 0, 100, 1, false, Title = "ScalePadding")]
[ToolkitSampleNumericOption("TickPadding", 24, 0, 100, 1, false, Title = "TickPadding")]
[ToolkitSampleNumericOption("ScaleTickWidth", 0, 0, 20, 1, false, Title = "ScaleTickWidth")]

[ToolkitSample(id: nameof(RadialGaugeSample), "RadialGauge", description: $"A sample for showing how to create and use a {nameof(RadialGauge)} control.")]
public sealed partial class RadialGaugeSample : Page
{
    public RadialGaugeSample()
    {
        this.InitializeComponent();
    }
}
