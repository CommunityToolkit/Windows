// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.WinUI.Controls;
using Windows.UI;

namespace PrimitivesExperiment.Samples;

[ToolkitSample(id: nameof(DockPanelSample), "DockPanel", description: $"A sample for showing how to create and use a {nameof(DockPanel)}.")]
public sealed partial class DockPanelSample : Page
{
    private static readonly Random Rand = new Random();

    public DockPanelSample()
    {
        this.InitializeComponent();
    }

    private void ClearAllDock(object sender, RoutedEventArgs e)
    {
        SampleDockPanel.Children.Clear();
        SampleDockPanel.LastChildFill = false;
    }

    private void AddStretchDock(object sender, RoutedEventArgs e)
    {
        AddChild(Dock.Bottom, false, false);
        SampleDockPanel.LastChildFill = true;
    }

    private void AddBottomDock(object sender, RoutedEventArgs e)
    {
        AddChild(Dock.Bottom, false, true);
    }

    private void AddTopDock(object sender, RoutedEventArgs e)
    {
        AddChild(Dock.Top, false, true);
    }

    private void AddLeftDock(object sender, RoutedEventArgs e)
    {
        AddChild(Dock.Left, true, false);
    }

    private void AddRightDock(object sender, RoutedEventArgs e)
    {
        AddChild(Dock.Right, true, false);
    }

    private void AddChild(Dock dock, bool setWidth = false, bool setHeight = false)
    {
        if (SampleDockPanel.LastChildFill)
        {
            return;
        }

        const int maxColor = 255;
        var childStackPanel = new StackPanel
        {
            Background = new SolidColorBrush(Color.FromArgb(
                                (byte)Rand.Next(0, maxColor),
                                (byte)Rand.Next(0, maxColor),
                                (byte)Rand.Next(0, maxColor),
                                1))
        };

        if (setHeight)
        {
            childStackPanel.Height = Rand.Next(50, 80);
        }

        if (setWidth)
        {
            childStackPanel.Width = Rand.Next(50, 80);
        }

        childStackPanel.SetValue(DockPanel.DockProperty, dock);
        childStackPanel.PointerPressed += this.ChildStackPanel_PointerPressed;
        SampleDockPanel.Children.Add(childStackPanel);
    }

    private void ChildStackPanel_PointerPressed(object sender, PointerRoutedEventArgs e)
    {
        SampleDockPanel.Children.Remove((StackPanel)sender);
    }
}
