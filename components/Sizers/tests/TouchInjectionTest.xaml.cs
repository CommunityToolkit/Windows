// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace SizersTests;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class TouchInjectionTest : Page
{
    public bool WasButtonClicked { get; set; }

    public TouchInjectionTest()
    {
        this.InitializeComponent();
    }

    private void TestButton_Click(object sender, RoutedEventArgs args)
    {
        WasButtonClicked = true;

        TestButton.Content = "Clicked!";
    }
}
