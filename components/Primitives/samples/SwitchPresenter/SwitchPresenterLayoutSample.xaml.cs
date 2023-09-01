// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using PrimitivesExperiment.Samples.ConstrainedBox;

namespace PrimitivesExperiment.Samples.SwitchPresenter;

[ToolkitSample(id: nameof(SwitchPresenterLayoutSample), "SwitchPresenter Layout", description: $"A sample for showing how to use a {nameof(SwitchPresenter)} for complex layouts.")]
public sealed partial class SwitchPresenterLayoutSample : Page
{
    public SwitchPresenterLayoutSample()
    {
        this.InitializeComponent();
    }
}
