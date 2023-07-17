// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace MediaExperiment.Samples;

[ToolkitSampleOptionsPane(nameof(BlurEffectAnimationSample))]
public sealed partial class BlurEffectAnimationSampleOptions : Page
{
    private BlurEffectAnimationSample _sampleInstance;

    public BlurEffectAnimationSampleOptions(BlurEffectAnimationSample sampleInstance)
    {
        _sampleInstance = sampleInstance;
        this.InitializeComponent();
    }

    public void OnStartAnimationButton_Clicked(object sender, RoutedEventArgs e)
    {
        new BlurEffectAnimationSample.XamlNamedPropertyRelay(_sampleInstance).BlurAnimation.Start();
    }

    public void OnStopAnimationButton_Clicked(object sender, RoutedEventArgs e)
    {
        new BlurEffectAnimationSample.XamlNamedPropertyRelay(_sampleInstance).BlurAnimation.Stop();
    }
}
