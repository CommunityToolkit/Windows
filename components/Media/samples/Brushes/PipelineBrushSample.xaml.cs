// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.WinUI.Media;

namespace MediaExperiment.Samples.Brushes;

[ToolkitSample(id: nameof(PipelineBrushSample), "PipelineBrush", description: $"A sample for showing how to apply a {nameof(PipelineBrush)} effect to a background.")]
public sealed partial class PipelineBrushSample : Page
{
    public PipelineBrushSample()
    {
        this.InitializeComponent();
    }
}
