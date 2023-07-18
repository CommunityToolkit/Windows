// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace ExtensionsExperiment.Samples;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
[ToolkitSample(id: nameof(FrameworkElementAncestorSample), nameof(FrameworkElementAncestorSample), description: $"A sample for showing how to use the FrameworkElementExtensions.Ancestor attached property.")]
public sealed partial class FrameworkElementAncestorSample : Page
{
    public FrameworkElementAncestorSample()
    {
        this.InitializeComponent();
    }
}
