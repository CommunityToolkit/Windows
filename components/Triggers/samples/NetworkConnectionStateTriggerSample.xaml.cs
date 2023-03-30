// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.WinUI;

namespace TriggersExperiment.Samples;

[ToolkitSample(id: nameof(NetworkConnectionStateTriggerSample), "NetworkConnectionStateTrigger", description: $"A sample for showing how to create and use a {nameof(NetworkConnectionStateTrigger)}.")]
public sealed partial class NetworkConnectionStateTriggerSample : Page
{
    public NetworkConnectionStateTriggerSample()
    {
        this.InitializeComponent();
    }
}
