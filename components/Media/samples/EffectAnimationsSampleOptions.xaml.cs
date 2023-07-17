// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace MediaExperiment.Samples;

[ToolkitSampleOptionsPane(nameof(EffectAnimationsSample))]
public sealed partial class EffectAnimationsSampleOptions : Page
{
    private EffectAnimationsSample _sampleInstance;

    public EffectAnimationsSampleOptions(EffectAnimationsSample sampleInstance)
    {
        _sampleInstance = sampleInstance;
        this.InitializeComponent();
    }

    public void OnStartAnimationButton_Clicked(object sender, RoutedEventArgs e)
    {
        new EffectAnimationsSample.XamlNamedPropertyRelay(_sampleInstance).ClipAnimation.Start();
    }

    public void OnStopAnimationButton_Clicked(object sender, RoutedEventArgs e)
    {
        new EffectAnimationsSample.XamlNamedPropertyRelay(_sampleInstance).ClipAnimation.Stop();
    }
}
