// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace SettingsControlsExperiment.Samples;

[ToolkitSampleBoolOption("IsCardExpanded", false, Title = "Is Expanded")]
[ToolkitSampleBoolOption("IsCardEnabled", true, Title = "Is Enabled")]
[ToolkitSample(id: nameof(SettingsExpanderSample), "SettingsExpander", description: "The SettingsExpander can be used to group settings. SettingsCards can be customized in terms of alignment and content.")]
public sealed partial class SettingsExpanderSample : Page
{
    public SettingsExpanderSample()
    {
        this.InitializeComponent();
    }
}
