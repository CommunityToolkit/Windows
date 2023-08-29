// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using AnimationsExperiment.Samples.ConnectedAnimations;

namespace AnimationsExperiment.Samples;

[ToolkitSample(id: nameof(ConnectedAnimationsSample), "Connected Animations", description: $"A sample for showing how to create and use connected animations.")]
public sealed partial class ConnectedAnimationsSample : Page
{

    public ConnectedAnimationsSample()
    {
        this.InitializeComponent();

        RootFrame.Navigate(typeof(FirstPage));
    }

    private void Frame_Navigated(object sender, NavigationEventArgs e)
    {
        BackButton.Visibility = RootFrame.CanGoBack ? Visibility.Visible : Visibility.Collapsed;
    }

    private void RootFrame_Navigating(object sender, NavigatingCancelEventArgs e)
    {
        if (e.SourcePageType == RootFrame.SourcePageType)
        {
            e.Cancel = true;
        }
    }

    private void BackButton_Click(object sender, RoutedEventArgs e)
    {
        RootFrame.GoBack(new SuppressNavigationTransitionInfo());
    }
}
