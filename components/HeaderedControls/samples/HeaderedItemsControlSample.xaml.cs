// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.WinUI.Controls;

namespace HeaderedControlsExperiment.Samples;

[ToolkitSample(id: nameof(HeaderedItemsControlSample), "HeaderedItemsControl", description: $"A sample for showing how to create and use a {nameof(HeaderedItemsControl)} control.")]
public sealed partial class HeaderedItemsControlSample : Page
{
    public HeaderedItemsControlSample()
    {
        this.InitializeComponent();
        Items = "The quick brown fox jumped over the lazy river".Split(' ');
    }
    public IEnumerable<string> Items { get; }
}
