// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.WinUI.Media;

namespace MediaExperiment.Samples.Brushes;

[ToolkitSample(id: nameof(ImageBlendBrushSample), "BackdropGammaTransferBrush", description: $"A sample for showing how to apply a {nameof(ImageBlendBrush)} effect to a background.")]
public sealed partial class ImageBlendBrushSample : Page
{
    public ImageBlendBrushSample()
    {
        this.InitializeComponent();
    }
}
