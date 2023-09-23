// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace LoadingExperiment.Samples;

[ToolkitSampleTextOption("LoadingContent", "Loading content..", Title = "Content")]
[ToolkitSampleBoolOption("LoadingState", true, Title = "IsLoading")]

[ToolkitSample(id: nameof(LoadingSample), "Loading", description: $"A sample for showing how to create and use a {nameof(CommunityToolkit.WinUI.Controls.Loading)} control.")]
public sealed partial class LoadingSample : Page
{
    public LoadingSample()
    {
        this.InitializeComponent();
    }
}
