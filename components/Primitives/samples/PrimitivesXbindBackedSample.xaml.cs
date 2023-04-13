// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace PrimitivesExperiment.Samples;

[ToolkitSampleBoolOption("IsTextVisible", true, Title = "IsVisible")]
// Single values without a colon are used for both label and value.
// To provide a different label for the value, separate with a colon surrounded by a single space on both sides ("label : value").
[ToolkitSampleNumericOption("TextSize", 12, 8, 48, 2, false, Title = "FontSize")]
[ToolkitSampleMultiChoiceOption("TextFontFamily", "Segoe UI", "Arial", "Consolas", Title = "Font family")]
[ToolkitSampleMultiChoiceOption("TextForeground", "Teal : #0ddc8c", "Sand : #e7a676", "Dull green : #5d7577", Title = "Text foreground")]

[ToolkitSample(id: nameof(PrimitivesXbindBackedSample), "Backed templated control", description: "A sample for showing how to create and use a templated control with a backed resource dictionary.")]
public sealed partial class PrimitivesXbindBackedSample : Page
{
    public PrimitivesXbindBackedSample()
    {
        this.InitializeComponent();
    }
}
