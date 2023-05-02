// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.WinUI;
using CommunityToolkit.WinUI.Helpers;
using Microsoft.UI;
using Windows.UI;

namespace HelpersExperiment.Samples;

/// <summary>
/// An example sample page of a custom control inheriting from Panel.
/// </summary>
//[ToolkitSampleTextOption("TitleText", "This is a title", Title = "Input the text")]
//[ToolkitSampleMultiChoiceOption("LayoutOrientation", "Horizontal", "Vertical", Title = "Orientation")]

[ToolkitSample(id: nameof(ThemeListenerSample), "Theme Listener", description: $"A sample for showing how to use {nameof(ThemeListener)}.")]
public sealed partial class ThemeListenerSample : Page
{
    public ThemeListenerSample()
    {
        this.InitializeComponent();
        Listener = new ThemeListener();
        this.Loaded += ThemeListenerPage_Loaded;
        Listener.ThemeChanged += Listener_ThemeChanged;
        this.ActualThemeChanged += this.ThemeListenerSample_ActualThemeChanged;
        float result = ScreenUnitHelper.Convert(ScreenUnit.Inch, ScreenUnit.Pixel, 1);
    }

    private void ThemeListenerSample_ActualThemeChanged(FrameworkElement sender, object args)
    {
        UpdateThemeState();
    }

    private void ThemeListenerPage_Loaded(object sender, RoutedEventArgs e)
    {
        UpdateThemeState();
    }

    private void Listener_ThemeChanged(ThemeListener sender)
    {
        UpdateThemeState();
    }

    private void UpdateThemeState()
    {
        SystemTheme.Text = Listener.CurrentThemeName;
        CurrentTheme.Text = this.ActualTheme.ToString();
    }

    public ThemeListener Listener { get; }
}
