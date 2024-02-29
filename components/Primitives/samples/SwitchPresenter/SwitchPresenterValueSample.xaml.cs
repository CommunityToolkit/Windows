// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace PrimitivesExperiment.Samples.SwitchPresenter;

[ToolkitSample(id: nameof(SwitchPresenterValueSample), "SwitchPresenter Value", description: $"A sample for showing how to use a {nameof(SwitchPresenter)} for state changes from an enum.")]
public sealed partial class SwitchPresenterValueSample : Page
{
    public SwitchPresenterValueSample()
    {
        this.InitializeComponent();
    }
}

public enum Animal
{
    Cat,
    Dog,
    Bunny,
    Llama,
    Parrot,
    Squirrel
}
