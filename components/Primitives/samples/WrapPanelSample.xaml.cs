// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.WinUI;

// TODO: Discuss with Uno folks about their own internal WrapPanel implementation.
using WrapPanel = CommunityToolkit.WinUI.Controls.WrapPanel;

namespace PrimitivesExperiment.Samples;

[ToolkitSampleNumericOption("HorizontalSpacing", initial: 5, min: 0, max: 200, step: 1, Title = "Horizontal Spacing")]
[ToolkitSampleNumericOption("VerticalSpacing", initial: 5, min: 0, max: 200, step: 1, Title = "VerticalSpacing")]

[ToolkitSample(id: nameof(WrapPanelSample), "WrapPanel", description: $"A sample for showing how to create and use a {nameof(WrapPanel)}.")]
public sealed partial class WrapPanelSample : Page
{
    private static readonly Random Rand = new Random();
    private ObservableCollection<PhotoDataItemWithDimension> WrapPanelCollection = new();

    public WrapPanelSample()
    {
        this.InitializeComponent();
    }

    private void ItemControl_ItemClick(object sender, ItemClickEventArgs e)
    {
        var item = e.ClickedItem as PhotoDataItemWithDimension;
        if (item == null)
        {
            return;
        }

        WrapPanelCollection.Remove(item);
    }

    private void AddButton_Click(object sender, RoutedEventArgs e)
    {
        WrapPanelCollection.Add(new PhotoDataItemWithDimension
        {
            Category = "Remove",
            Thumbnail = "ms-appx:///Assets/BigFourSummerHeat.jpg",
            Width = Rand.Next(60, 180),
            Height = Rand.Next(40, 140)
        });
    }

    private void AddFixedBtn_Click(object sender, RoutedEventArgs e)
    {
        WrapPanelCollection.Add(new PhotoDataItemWithDimension
        {
            Category = "Remove",
            Thumbnail = "ms-appx:///Assets/BigFourSummerHeat.jpg",
            Width = 150,
            Height = 100
        });
    }

    private void SwitchBtn_Click(object sender, RoutedEventArgs e)
    {
        if (WrapPanelContainer.FindDescendant<WrapPanel>() is WrapPanel sampleWrapPanel)
        {
            if (sampleWrapPanel.Orientation == Orientation.Horizontal)
            {
                sampleWrapPanel.Orientation = Orientation.Vertical;
                ScrollViewer.SetVerticalScrollMode(WrapPanelContainer, ScrollMode.Disabled);
                ScrollViewer.SetVerticalScrollBarVisibility(WrapPanelContainer, ScrollBarVisibility.Disabled);
                ScrollViewer.SetHorizontalScrollMode(WrapPanelContainer, ScrollMode.Auto);
                ScrollViewer.SetHorizontalScrollBarVisibility(WrapPanelContainer, ScrollBarVisibility.Auto);
            }
            else
            {
                sampleWrapPanel.Orientation = Orientation.Horizontal;
                ScrollViewer.SetVerticalScrollMode(WrapPanelContainer, ScrollMode.Auto);
                ScrollViewer.SetVerticalScrollBarVisibility(WrapPanelContainer, ScrollBarVisibility.Auto);
                ScrollViewer.SetHorizontalScrollMode(WrapPanelContainer, ScrollMode.Disabled);
                ScrollViewer.SetHorizontalScrollBarVisibility(WrapPanelContainer, ScrollBarVisibility.Disabled);
            }
        }
    }

    public class PhotoDataItemWithDimension : PhotoDataItem
    {
        public double Width { get; set; }
        public double Height { get; set; }
    }

    public class PhotoDataItem
    {
        public string? Title { get; set; }

        public string? Category { get; set; }

        public string? Thumbnail { get; set; }

        public override string ToString()
        {
            return Title!;
        }
    }
}
