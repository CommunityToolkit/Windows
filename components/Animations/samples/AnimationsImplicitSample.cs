// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.WinUI.Animations;

namespace AnimationsExperiment.Samples;

[ToolkitSample(id: nameof(AnimationsImplicitSample), "Implicit animations", description: $"A sample for showing how to create and use implicit animations.")]
public sealed partial class AnimationsImplicitSample : Page
{
    public AnimationsImplicitSample()
    {
        this.InitializeComponent();
    }

    private void Visibility_Click(object sender, RoutedEventArgs e)
    {
        Element.Visibility = Element.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
    }
}
