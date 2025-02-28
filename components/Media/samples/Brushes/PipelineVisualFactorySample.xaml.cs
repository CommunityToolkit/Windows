// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.WinUI.Media;

namespace MediaExperiment.Samples.Brushes;

[ToolkitSample(id: nameof(PipelineVisualFactorySample), "BackdropBlurBrush", description: $"A sample that uses a {nameof(PipelineVisualFactory)} to blur whatever is behind the application.")]
public sealed partial class PipelineVisualFactorySample : Page
{
    public PipelineVisualFactorySample()
    {
        this.InitializeComponent();
    }
}
