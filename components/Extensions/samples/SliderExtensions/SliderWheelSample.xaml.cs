// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace ExtensionsExperiment.Samples.SliderExtensions;

/// <summary>
/// A sample showing how the <c>SliderExtensions</c> attached properties make a
/// <see cref="Microsoft.UI.Xaml.Controls.Slider"/> respond to mouse-wheel input.
/// </summary>
[ToolkitSampleBoolOption("IsMouseWheelEnabled", true, Title = "Enable mouse-wheel scrolling")]
[ToolkitSampleNumericOption("MouseWheelChange", 5, 1, 25, Title = "Value change per wheel notch")]
[ToolkitSample(id: nameof(SliderWheelSample), "Slider Mouse Wheel", description: "An extension that lets a Slider be adjusted by mouse-wheel scrolling.")]
public sealed partial class SliderWheelSample : Page
{
    public SliderWheelSample()
    {
        this.InitializeComponent();
    }
}
