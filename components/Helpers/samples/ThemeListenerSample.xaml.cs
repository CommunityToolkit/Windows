// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.WinUI.Helpers;

namespace HelpersExperiment.Samples;

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
