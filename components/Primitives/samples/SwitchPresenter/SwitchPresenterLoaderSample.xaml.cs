// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace PrimitivesExperiment.Samples.SwitchPresenter;

[ToolkitSampleBoolOption("LoadingState", true, Title = "IsLoading")]
[ToolkitSample(id: nameof(SwitchPresenterLoaderSample), "SwitchPresenter Loader", description: $"A sample for showing how to use a {nameof(SwitchPresenter)} for state changes from an enum.")]
public sealed partial class SwitchPresenterLoaderSample : Page
{
    public SwitchPresenterLoaderSample()
    {
        this.InitializeComponent();
    }
}
