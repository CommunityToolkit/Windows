// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.WinUI.Controls;

namespace RangeSelectorExperiment.Samples;

[ToolkitSampleNumericOption("Minimum", 0, 0, 100, 1, false, Title = "Minimum")]
[ToolkitSampleNumericOption("Maximum", 100, 0, 100, 1, false, Title = "Maximum")]
[ToolkitSampleNumericOption("StepFrequency", 1, 0, 10, 1, false, Title = "StepFrequency")]
[ToolkitSampleBoolOption("Enable", true, Title = "IsEnabled")]

[ToolkitSample(id: nameof(RangeSelectorSample), "RangeSelector", description: $"A sample for showing how to create and use a {nameof(RangeSelector)} control.")]
public sealed partial class RangeSelectorSample : Page
{
    public RangeSelectorSample()
    {
        this.InitializeComponent();
    }
}
