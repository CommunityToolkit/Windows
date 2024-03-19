// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using ColorSpectrumShape = Microsoft.UI.Xaml.Controls.ColorSpectrumShape;

namespace ColorPickerExperiment.Samples;

[ToolkitSample(id: nameof(ColorPickerButtonSample), "ColorPickerButton", description: $"A sample for showing how to create and use a {nameof(CommunityToolkit.WinUI.Controls.ColorPickerButton)} control.")]
public sealed partial class ColorPickerButtonSample : Page
{
    public ColorPickerButtonSample()
    {
        this.InitializeComponent();
    }
}
