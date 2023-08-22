// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace AnimationsExperiment.Samples.ConnectedAnimations;

  public sealed partial class FirstPage : Page
{
    public FirstPage()
    {
        this.InitializeComponent();
    }

    private void Border_Tapped(object sender, TappedRoutedEventArgs e)
    {
        Frame.Navigate(typeof(SecondPage), null, new SuppressNavigationTransitionInfo());
    }
}
