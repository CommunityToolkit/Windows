// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.WinUI.Behaviors;

namespace BehaviorsExperiment.Samples;

[ToolkitSampleBoolOption("IsButtonEnabled", true, Title = "Enable button")]
[ToolkitSampleBoolOption("ControlLoaded", true, Title = "Toggle x:Bind")]
[ToolkitSample(id: nameof(FocusBehaviorButtonSample), $"{nameof(FocusBehavior)}: Disabled / Unloaded items", description: $"A sample demonstrating how {nameof(FocusBehavior)} affects disabled or unloaded controls.")]
public sealed partial class FocusBehaviorButtonSample : Page
{
    public FocusBehaviorButtonSample()
    {
        this.InitializeComponent();
    }
}
