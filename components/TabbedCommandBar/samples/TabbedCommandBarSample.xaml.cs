// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.WinUI.Controls;

namespace TabbedCommandBarExperiment.Samples;

[ToolkitSampleBoolOption("ContextualItem", true, Title = "Show contextual item")]

[ToolkitSample(id: nameof(TabbedCommandBarSample), "TabbedCommandBar", description: $"A sample for showing how to create and use a {nameof(TabbedCommandBar)} control.")]
public sealed partial class TabbedCommandBarSample : Page
{
    public TabbedCommandBarSample()
    {
        this.InitializeComponent();
    }
}
