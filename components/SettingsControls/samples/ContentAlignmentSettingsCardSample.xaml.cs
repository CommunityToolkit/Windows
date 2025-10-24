// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace SettingsControlsExperiment.Samples;

[ToolkitSampleBoolOption("IsCardEnabled", true, Title = "Is Enabled")]

[ToolkitSample(id: nameof(ContentAlignmentSettingsCardSample), "ContentAlignmentSettingsCardSample", description: "A sample for showing how to use the SettingsCard.ContentAlignment attached property")]

public sealed partial class ContentAlignmentSettingsCardSample : Page
{
    public ContentAlignmentSettingsCardSample()
    {
        this.InitializeComponent();
    }
}
