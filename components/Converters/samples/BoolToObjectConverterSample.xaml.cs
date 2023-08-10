// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace ConvertersExperiment.Samples;

[ToolkitSampleBoolOption("MyBooleanValue", false, Title = "Toggle to change colors")]

[ToolkitSample(id: nameof(BoolToObjectConverterSample), "BoolToObjectConverter", description: $"A sample for showing how to use the BoolToObjectConverter.")]
public sealed partial class BoolToObjectConverterSample : Page
{
    public BoolToObjectConverterSample()
    {
        this.InitializeComponent();
    }
}
