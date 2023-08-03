// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace ConvertersExperiment.Samples;

[ToolkitSampleBoolOption("MyBooleanValue", true, Title = "Toggle to show or hide the image")]

[ToolkitSample(id: nameof(VisibilityToBoolConverterSample), "VisibilityToBoolConverter", description: $"A sample for showing how to use the VisibilityToBoolConverter.")]
public sealed partial class VisibilityToBoolConverterSample : Page
{
    public VisibilityToBoolConverterSample()
    {
        this.InitializeComponent();
    }
}
