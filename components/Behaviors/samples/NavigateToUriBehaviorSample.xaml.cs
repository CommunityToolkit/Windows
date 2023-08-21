// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.WinUI.Behaviors;

namespace BehaviorsExperiment.Samples;

[ToolkitSample(id: nameof(NavigateToUriBehaviorSample), nameof(NavigateToUriBehavior), description: $"A sample demonstrating how to use {nameof(NavigateToUriBehavior)}.")]
public sealed partial class NavigateToUriBehaviorSample : Page
{
    public NavigateToUriBehaviorSample()
    {
        this.InitializeComponent();
    }
}
