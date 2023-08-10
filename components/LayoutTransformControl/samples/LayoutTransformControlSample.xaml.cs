// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.WinUI.Controls;

namespace LayoutTransformControlExperiment.Samples;

/// <summary>
/// An example sample page of a custom control inheriting from Panel.
/// </summary>
[ToolkitSampleNumericOption("Angle", 0, -180.0, 180.0, 1, false, Title = "Angle")]
[ToolkitSampleNumericOption("CustomScaleX", 1, 0.0, 5.0, 1, false, Title = "ScaleX")]
[ToolkitSampleNumericOption("CustomScaleY", 1, 0.0, 5.0, 1, false, Title = "ScaleY")]
[ToolkitSampleNumericOption("SkewX", 0, -180.0, 180.0, 1, false, Title = "SkewX")]
[ToolkitSampleNumericOption("SkewY", 0, -180.0, 180.0, 1, false, Title = "SkewY")]

[ToolkitSample(id: nameof(LayoutTransformControlSample), "LayoutTransformControl", description: $"A sample for showing how to create and use a {nameof(LayoutTransformControl)}.")]
public sealed partial class LayoutTransformControlSample : Page
{
    public LayoutTransformControlSample()
    {
        this.InitializeComponent();
    }
}
