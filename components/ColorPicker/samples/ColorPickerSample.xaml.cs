// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using ColorSpectrumShape = Microsoft.UI.Xaml.Controls.ColorSpectrumShape;

namespace ColorPickerExperiment.Samples;

[ToolkitSampleBoolOption("AccentColors", true, Title = "ShowAccentColors")]
[ToolkitSampleBoolOption("AlphaEnabled", true, Title = "IsAlphaEnabled")]
[ToolkitSampleBoolOption("AlphaSlider", true, Title = "IsAlphaSliderVisible")]
[ToolkitSampleBoolOption("ColorSlider", true, Title = "IsColorSliderVisible")]
[ToolkitSampleBoolOption("ColorChannel", true, Title = "IsColorChannelTextInputVisible")]
[ToolkitSampleBoolOption("SpectrumVisible", true, Title = "IsColorSpectrumVisible")]
[ToolkitSampleBoolOption("ColorPalette", true, Title = "IsColorPaletteVisible")]

[ToolkitSampleMultiChoiceOption("SpectrumShape", "Box", "Ring", Title = "ColorSpectrumShape")]

[ToolkitSample(id: nameof(ColorPickerSample), "ColorPicker", description: $"A sample for showing how to create and use a {nameof(CommunityToolkit.WinUI.Controls.ColorPicker)} control.")]
public sealed partial class ColorPickerSample : Page
{
    public ColorPickerSample()
    {
        this.InitializeComponent();
    }

    // TODO: See https://github.com/CommunityToolkit/Labs-Windows/issues/149
    public static ColorSpectrumShape ConvertStringToColorSpectrumShape(string shape) => shape switch
    {
        "Ring" => ColorSpectrumShape.Ring,
        "Box" => ColorSpectrumShape.Box,
        _ => throw new System.NotImplementedException(),
    };
}
