// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;

namespace SettingsControlsExperiment.Samples;

[ToolkitSampleBoolOption("IsCardEnabled", true, Title = "Is Enabled")]

[ToolkitSample(id: nameof(ClickableSettingsCardSample), "ClickableSettingsCardSample", description: "A sample for showing how SettingsCard can be static or clickable.")]
public sealed partial class ClickableSettingsCardSample : Page
{
    public ClickableSettingsCardSample()
    {
        this.InitializeComponent();
    }

    private async void OnCardClicked(object sender, RoutedEventArgs e)
    {
        await Windows.System.Launcher.LaunchUriAsync(new Uri("https://www.microsoft.com"));
    }
}
