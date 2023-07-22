// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace ShimmerExperiment.Samples;

[ToolkitSampleBoolOption("HasLoaded", false, Title = "Content loaded")]

[ToolkitSample(id: nameof(ShimmerSample), "Basic Shimmer", description: "A sample that shows how to use a shimmer loading indicator.")]
public sealed partial class ShimmerSample : Page
{
    public ShimmerSample()
    {
        this.InitializeComponent();
    }

    private static Visibility ReverseVisibility(Visibility vis) => vis switch
    {
        Visibility.Collapsed => Visibility.Visible,
        Visibility.Visible => Visibility.Collapsed,
        _ => throw new System.NotImplementedException(),

    };
}
