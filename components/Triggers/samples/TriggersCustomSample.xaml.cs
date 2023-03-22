// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.WinUI.Controls;

namespace TriggersExperiment.Samples;

/// <summary>
/// An example sample page of a custom control inheriting from Panel.
/// </summary>

[ToolkitSample(id: nameof(TriggersCustomSample), "Custom control", description: $"A sample for showing how to create and use a {nameof(Triggers)} custom control.")]
public sealed partial class TriggersCustomSample : Page
{
    public TriggersCustomSample()
    {
        this.InitializeComponent();
    }
}
