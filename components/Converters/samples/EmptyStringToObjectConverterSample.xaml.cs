// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace ConvertersExperiment.Samples;

[ToolkitSampleTextOption("MyTextStringValue", "", Title = "Enter text to enable the button")]

[ToolkitSample(id: nameof(EmptyStringToObjectConverterSample), "EmptyStringToObjectConverter", description: $"A sample for showing how to use the EmptyStringToObjectConverter.")]
public sealed partial class EmptyStringToObjectConverterSample : Page
{
    public EmptyStringToObjectConverterSample()
    {
        this.InitializeComponent();
    }
}
