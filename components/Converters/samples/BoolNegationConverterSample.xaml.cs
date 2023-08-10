// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace ConvertersExperiment.Samples;

[ToolkitSampleBoolOption("MyBooleanValue", true, Title = "Original value")]

[ToolkitSample(id: nameof(BoolNegationConverterSample), "BoolNegationConverter", description: $"A sample for showing how to use the BoolNegationConverter.")]
public sealed partial class BoolNegationConverterSample : Page
{
    public BoolNegationConverterSample()
    {
        this.InitializeComponent();
    }
}
