// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.WinUI.Controls;
using Windows.UI;

namespace PrimitivesExperiment.Samples;

[ToolkitSampleNumericOption("DesiredColumnWidth", initial: 250, min: 50, max: 400, step: 1, Title = "DesiredColumnWidth")]
[ToolkitSampleNumericOption("ColumnSpacing", initial: 5, min: 0, max: 50, step: 1, Title = "ColumnSpacing")]
[ToolkitSampleNumericOption("RowSpacing", initial: 5, min: 0, max: 50, step: 1, Title = "RowSpacing")]

[ToolkitSample(id: nameof(StaggeredLayoutSample), "StaggeredPanel", description: $"A sample for showing how to create and use a {nameof(StaggeredLayout)}.")]
public sealed partial class StaggeredLayoutSample : Page
{
    public ObservableCollection<ColorItem> ColorsCollection = new();
    public Random random;

    public StaggeredLayoutSample()
    {
        this.InitializeComponent();

        random = new Random(DateTime.Now.Millisecond);
        for (int i = 0; i < random.Next(100, 200); i++)
        {
            var item = new ColorItem { Index = i, Width = random.Next(50, 250), Height = random.Next(50, 250), Color = Color.FromArgb(255, (byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255)) };
            ColorsCollection.Add(item);
        }
    }
}

public class ColorItem
{
    public int Index { get; internal set; }

    public int Width { get; internal set; }

    public int Height { get; internal set; }

    public Color Color { get; internal set; }
}
