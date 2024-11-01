// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.WinUI.Converters;

namespace PrimitivesExperiment.Samples.SwitchPresenter;

[ToolkitSample(id: nameof(SwitchConverterBrushSample), "SwitchConverter Brush", description: $"A sample for showing how to use a {nameof(SwitchConverter)} for swapping a brush based on an enum.")]
public sealed partial class SwitchConverterBrushSample : Page
{
    public SwitchConverterBrushSample()
    {
        this.InitializeComponent();
    }
}

public enum CheckStatus
{
    Error,
    Warning,
    Success,
}
