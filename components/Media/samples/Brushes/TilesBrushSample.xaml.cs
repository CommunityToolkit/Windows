// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.WinUI.Media;

namespace MediaExperiment.Samples.Brushes;

[ToolkitSample(id: nameof(TilesBrushSample), "Tiles Brush", description: $"A sample that uses a {nameof(TilesBrush)} to display a tiled image. Here, an image of polka dots is tiled to create a seamless repeated background.")]
public sealed partial class TilesBrushSample : Page
{
    public TilesBrushSample()
    {
        this.InitializeComponent();
    }
}
