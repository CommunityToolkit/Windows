// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace ConvertersExperiment.Samples;

[ToolkitSampleNumericOption("NumericValue", 0, 0, 10, 1, false, Title = "Number")]

[ToolkitSample(id: nameof(DoubleToVisibilityConverterSample), "DoubleToVisibilityConverter", description: $"A sample for showing how to use the DoubleToObjectConverter.")]
public sealed partial class DoubleToVisibilityConverterSample : Page
{
    public DoubleToVisibilityConverterSample()
    {
        this.InitializeComponent();
    }
}
