// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.WinUI.Controls;
using Windows.UI;

namespace PrimitivesExperiment.Samples;

[ToolkitSampleNumericOption("HorizontalSpacing", initial: 5, min: 0, max: 200, step: 1, Title = "Horizontal Spacing")]
[ToolkitSampleNumericOption("VerticalSpacing", initial: 5, min: 0, max: 200, step: 1, Title = "VerticalSpacing")]

[ToolkitSample(id: nameof(WrapLayoutSample), "WrapLayout", description: $"A sample for showing how to create and use a {nameof(WrapLayout)}.")]
public sealed partial class WrapLayoutSample : Page
{
    public ObservableCollection<ColorItem> ColorsCollection = new();
    public Random random;

    public WrapLayoutSample()
    {
        this.InitializeComponent();

        random = new Random(DateTime.Now.Millisecond);
        for (int i = 0; i < random.Next(1000, 5000); i++)
        {
            var item = new ColorItem { Index = i, Width = random.Next(50, 250), Height = random.Next(50, 250), Color = Color.FromArgb(255, (byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255)) };
            ColorsCollection.Add(item);
        }
    }
}
