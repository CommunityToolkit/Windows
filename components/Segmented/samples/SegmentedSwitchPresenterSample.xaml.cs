// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.WinUI.Controls;

namespace SegmentedExperiment.Samples;

/// <summary>
/// An example sample page of a Segmented control.
/// </summary>

[ToolkitSample(id: nameof(SegmentedSwitchPresenterSample), "Segmented + SwitchPresenter", description: $"A sample for showing how to create and use a {nameof(Segmented)} control in combination with a SwitchPresenter.")]
public sealed partial class SegmentedSwitchPresenterSample : Page
{
    public SegmentedSwitchPresenterSample()
    {
        this.InitializeComponent();
    }
}

