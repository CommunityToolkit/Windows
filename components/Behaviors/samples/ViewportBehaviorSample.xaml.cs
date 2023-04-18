// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.WinUI.Behaviors;

namespace BehaviorsExperiment.Samples;

[ToolkitSampleBoolOption("IsAlwaysOn", true, Title = "IsAlwaysOn")]
[ToolkitSample(id: nameof(ViewportBehaviorSample), nameof(ViewportBehavior), description: $"A sample for showing how to use the {nameof(ViewportBehavior)}.")]
public sealed partial class ViewportBehaviorSample : Page
{
    public ViewportBehaviorSample()
    {
        this.InitializeComponent();
    }
}
