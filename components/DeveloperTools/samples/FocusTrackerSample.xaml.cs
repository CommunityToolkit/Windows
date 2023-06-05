// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.WinUI.DeveloperTools;

namespace DeveloperToolsExperiment.Samples;

[ToolkitSampleBoolOption("IsActive", true, Title = "IsActive")]

[ToolkitSample(id: nameof(FocusTrackerSample), "FocusTracker", description: $"A sample for showing how to create and use a {nameof(FocusTracker)}.")]
public sealed partial class FocusTrackerSample : Page
{
    public FocusTrackerSample()
    {
        this.InitializeComponent();
    }
}
