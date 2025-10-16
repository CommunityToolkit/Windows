// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace SettingsControlsExperiment.Samples;

[ToolkitSample(id: nameof(SettingsExpanderDragHandleSample), "SettingsExpanderDragHandle", description: "The SettingsCard/SettingsExpander can be within a collection itself which can be re-ordered.")]
public sealed partial class SettingsExpanderDragHandleSample : Page
{

    public ObservableCollection<ExpandedCardInfo> MyDataSet = new() {
        new()
        {
            Name = "First Item",
            Info = "More about first item.",
            LinkDescription = "Click the link for more on first item.",
            Url = "https://microsoft.com/",
        },
        new()
        {
            Name = "Second Item",
            Info = "More about second item.",
            LinkDescription = "Click the link for more on second item.",
            Url = "https://xbox.com/",
        },
        new()
        {
            Name = "Third Item",
            Info = "More about third item.",
            LinkDescription = "Click the link for more on third item.",
            Url = "https://toolkitlabs.dev/",
        },
    };

    public SettingsExpanderDragHandleSample()
    {
        this.InitializeComponent();
    }
}

public class ExpandedCardInfo
{
    public string? Name { get; set; }

    public string? Info { get; set; }

    public string? LinkDescription { get; set; }

    public string? Url { get; set; }
}

