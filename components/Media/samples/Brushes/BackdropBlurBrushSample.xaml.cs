// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.WinUI.Media;

namespace MediaExperiment.Samples.Brushes;

[ToolkitSample(id: nameof(BackdropBlurBrushSample), "BackdropBlurBrush", description: $"A sample that uses a {nameof(BackdropBlurBrush)} to blur whatever is behind the application.")]
public sealed partial class BackdropBlurBrushSample : Page
{
    public BackdropBlurBrushSample()
    {
        this.InitializeComponent();
    }
}
