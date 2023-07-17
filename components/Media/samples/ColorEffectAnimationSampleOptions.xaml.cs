// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace MediaExperiment.Samples;

[ToolkitSampleOptionsPane(nameof(ColorEffectAnimationSample))]
public sealed partial class ColorEffectAnimationSampleOptions : Page
{
    private ColorEffectAnimationSample _sampleInstance;

    public ColorEffectAnimationSampleOptions(ColorEffectAnimationSample sampleInstance)
    {
        _sampleInstance = sampleInstance;
        this.InitializeComponent();
    }

    public void OnStartAnimationButton_Clicked(object sender, RoutedEventArgs e)
    {
        new ColorEffectAnimationSample.XamlNamedPropertyRelay(_sampleInstance).ColorAnimation.Start();
    }

    public void OnStopAnimationButton_Clicked(object sender, RoutedEventArgs e)
    {
        new ColorEffectAnimationSample.XamlNamedPropertyRelay(_sampleInstance).ColorAnimation.Stop();
    }
}
