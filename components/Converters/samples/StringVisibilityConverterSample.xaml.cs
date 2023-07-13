// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace ConvertersExperiment.Samples;

[ToolkitSampleTextOption("MyTextStringValue", "", Title = "Enter text to hide the TextBlock")]

[ToolkitSample(id: nameof(StringVisibilityConverterSample), "StringVisibilityConverter", description: $"A sample for showing how to use the StringVisibilityConverter.")]
public sealed partial class StringVisibilityConverterSample : Page
{
    public StringVisibilityConverterSample()
    {
        this.InitializeComponent();
    }
}
