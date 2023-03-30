// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.WinUI;

namespace TriggersExperiment.Samples;

[ToolkitSample(id: nameof(IsNullOrEmptyStateTriggerSample), "IsNullOrEmptyStateTrigger", description: $"A sample for showing how to create and use a {nameof(IsNullOrEmptyStateTrigger)}.")]
public sealed partial class IsNullOrEmptyStateTriggerSample : Page
{
    public IsNullOrEmptyStateTriggerSample()
    {
        this.InitializeComponent();
    }

    private void AddButton_Click(object sender, RoutedEventArgs e)
    {
        if (OurList != null)
        {
            OurList.Items.Add("Item");
        }
    }

    private void RemoveButton_Click(object sender, RoutedEventArgs e)
    {
        if (OurList != null)
        {
            OurList.Items.RemoveAt(0);
        }
    }
}
