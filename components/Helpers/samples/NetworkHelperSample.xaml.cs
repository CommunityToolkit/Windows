// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.WinUI.Helpers;

namespace HelpersExperiment.Samples;

[ToolkitSample(id: nameof(NetworkHelperSample), "Network Helper", description: $"A sample for showing how to use {nameof(NetworkHelper)}.")]
public sealed partial class NetworkHelperSample : Page
{
    public NetworkHelperSample()
    {
        this.InitializeComponent();
        Load();
    }

    private void Load()
    {
        IsInternetAvailableText.Text = NetworkHelper.Instance.ConnectionInformation.IsInternetAvailable ? "Yes" : "No";
        IsInternetOnMeteredConnectionText.Text = NetworkHelper.Instance.ConnectionInformation.IsInternetOnMeteredConnection ? "Yes" : "No";
        ConnectionTypeText.Text = NetworkHelper.Instance.ConnectionInformation.ConnectionType.ToString();
        SignalBarsText.Text = NetworkHelper.Instance.ConnectionInformation.SignalStrength.GetValueOrDefault(0).ToString();
        NetworkNamesText.Text = string.Join(", ", NetworkHelper.Instance.ConnectionInformation.NetworkNames);
    }
}
