// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace SettingsControlsExperiment.Samples;

[ToolkitSampleBoolOption("IsCardEnabled", true, Title = "Is Enabled")]

[ToolkitSample(id: nameof(SettingsCardSample), "SettingsCard", description: "A sample for showing how SettingsCard can be static or clickable.")]
public sealed partial class SettingsCardSample : Page
{
    public SettingsCardSample()
    {
        this.InitializeComponent();
    }
}
