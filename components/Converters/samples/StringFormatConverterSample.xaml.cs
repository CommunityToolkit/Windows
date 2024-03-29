// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.WinUI.Converters;

namespace ConvertersExperiment.Samples;

/// <summary>
/// An example sample page of a custom control inheriting from Panel.
/// </summary>
[ToolkitSampleBoolOption("MyBooleanValue", true, Title = "Is loading")]
[ToolkitSampleNumericOption("MyDoubleValue", 1, 0, 10, 0.25, false, Title = "Number of pixels")]
[ToolkitSample(id: nameof(StringFormatConverterSample), "StringFormatConverter", description: $"A sample for showing how to use the StringFormatConverter.")]
public sealed partial class StringFormatConverterSample : Page
{
    public StringFormatConverterSample()
    {
        this.InitializeComponent();
    }
}
