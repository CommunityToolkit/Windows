// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.WinUI;

namespace TriggersExperiment.Samples;

[ToolkitSample(id: nameof(FullScreenModeStateTriggerSample), "FullScreenModeStateTrigger", description: $"A sample for showing how to create and use a {nameof(FullScreenModeStateTrigger)}.")]
public sealed partial class FullScreenModeStateTriggerSample : Page
{
    public FullScreenModeStateTriggerSample()
    {
        this.InitializeComponent();
    }

    private void FullScreen_Click(object sender, RoutedEventArgs e)
    {
        var view = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView();
        var isFullScreenMode = view.IsFullScreenMode;

        if (isFullScreenMode)
        {
            view.ExitFullScreenMode();
        }
        else
        {
            view.TryEnterFullScreenMode();
        }
    }
}
