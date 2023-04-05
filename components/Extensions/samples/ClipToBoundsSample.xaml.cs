// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace ExtensionsExperiment.Samples;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
[ToolkitSampleBoolOption("IsClipped", true, Title = "Clip to Bounds")]
[ToolkitSample(id: nameof(ClipToBoundsSample), "ClipToBounds", description: "An extension for turning on clipping to the bounds of a container.")]

public sealed partial class ClipToBoundsSample : Page
{
    public ClipToBoundsSample()
    {
        this.InitializeComponent();
    }
}
