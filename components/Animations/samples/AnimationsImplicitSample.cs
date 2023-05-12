// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#if WINAPPSDK
using Microsoft.UI.Xaml.Hosting;
#else
using Windows.UI.Xaml.Hosting;
#endif
using System.Numerics;

namespace AnimationsExperiment.Samples;

[ToolkitSample(id: nameof(AnimationsImplicitSample), "Implicit animations", description: $"A sample for showing how to create and use implicit animations.")]
public sealed partial class AnimationsImplicitSample : Page
{
    private Random _random = new Random();
    public AnimationsImplicitSample()
    {
        this.InitializeComponent();
    }

    private void Visibility_Click(object sender, RoutedEventArgs e)
    {
        Element.Visibility = Element.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
    }

    private void Move_Click(object sender, RoutedEventArgs e)
    {
        Canvas.SetTop(Element, _random.NextDouble() * this.ActualHeight);
        Canvas.SetLeft(Element, _random.NextDouble() * this.ActualWidth);
    }

    private void Scale_Click(object sender, RoutedEventArgs e)
    {
        var visual = ElementCompositionPreview.GetElementVisual(Element);
        visual.Scale = new Vector3(
            (float)_random.NextDouble() * 2,
            (float)_random.NextDouble() * 2,
                            1);
    }
}
