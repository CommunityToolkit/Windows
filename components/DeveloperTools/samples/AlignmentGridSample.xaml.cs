// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.WinUI.DeveloperTools;

namespace DeveloperToolsExperiment.Samples;

[ToolkitSampleNumericOption("OpacitySetting", 0.2, 0.1, 1.0, 0.1, false, Title = "Opacity")]
[ToolkitSampleNumericOption("HorizontalStep", 20, 5, 100, 1, false, Title = "HorizontalStep")]
[ToolkitSampleNumericOption("VerticalStep", 20, 5, 100, 1, false, Title = "VerticalStep")]

[ToolkitSample(id: nameof(AlignmentGridSample), "AlignmentGrid", description: $"A sample for showing how to create and use a {nameof(AlignmentGrid)}.")]
public sealed partial class AlignmentGridSample : Page
{
    public AlignmentGridSample()
    {
        this.InitializeComponent();
    }
}
