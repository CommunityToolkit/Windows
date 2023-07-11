// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.WinUI.Animations;

namespace BehaviorsExperiment.Samples;

[ToolkitSample(id: nameof(StartAnimationActivitySample), "StartAnimationActivity", description: $"A sample for showing how to create and use a {nameof(InvokeActionsActivity)} behavior.")]
public sealed partial class StartAnimationActivitySample : Page
{
    public StartAnimationActivitySample()
    {
        this.InitializeComponent();
    }
}
