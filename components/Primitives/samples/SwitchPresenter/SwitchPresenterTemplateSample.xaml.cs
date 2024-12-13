// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace PrimitivesExperiment.Samples.SwitchPresenter;

[ToolkitSample(id: nameof(SwitchPresenterTemplateSample), "SwitchPresenter Template", description: $"A sample for showing how to use a {nameof(SwitchPresenter)} with a Content Template.")]
public sealed partial class SwitchPresenterTemplateSample : Page
{
    public SwitchPresenterTemplateSample()
    {
        this.InitializeComponent();
    }
}

public partial class TemplateInformation
{
    public string? Header { get; set; }

    public string? Regex { get; set; }

    public string? PlaceholderText { get; set; } 
}

