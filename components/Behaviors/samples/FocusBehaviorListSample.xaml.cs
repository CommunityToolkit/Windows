// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.WinUI.Behaviors;

namespace BehaviorsExperiment.Samples;

[ToolkitSample(id: nameof(FocusBehaviorListSample), $"{nameof(FocusBehavior)}: Lists", description: $"A sample demonstrating how {nameof(FocusBehavior)} affects lists.")]
public sealed partial class FocusBehaviorListSample : Page
{
    public FocusBehaviorListSample()
    {
        this.InitializeComponent();
    }
}
