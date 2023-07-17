// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.WinUI.Animations;

namespace MediaExperiment.Samples;

[ToolkitSampleOptionsPane(nameof(EffectAnimationsSample))]
[ToolkitSampleOptionsPane(nameof(ColorEffectAnimationSample))]
[ToolkitSampleOptionsPane(nameof(BlurEffectAnimationSample))]
[ToolkitSampleOptionsPane(nameof(CrossFadeEffectAnimationSample))]
public sealed partial class EffectAnimationsSampleOptions : Page
{
    private AnimationSet _animationSet;

    public EffectAnimationsSampleOptions(EffectAnimationsSample sampleInstance)
    {
        this.InitializeComponent();
        _animationSet = new EffectAnimationsSample.XamlNamedPropertyRelay(sampleInstance).ClipAnimation;
    }

    public EffectAnimationsSampleOptions(ColorEffectAnimationSample sampleInstance)
    {
        this.InitializeComponent();
        _animationSet = new ColorEffectAnimationSample.XamlNamedPropertyRelay(sampleInstance).ColorAnimation;
    }

    public EffectAnimationsSampleOptions(BlurEffectAnimationSample sampleInstance)
    {
        this.InitializeComponent();
        _animationSet = new BlurEffectAnimationSample.XamlNamedPropertyRelay(sampleInstance).BlurAnimation;
    }

    public EffectAnimationsSampleOptions(CrossFadeEffectAnimationSample sampleInstance)
    {
        this.InitializeComponent();
        _animationSet = new CrossFadeEffectAnimationSample.XamlNamedPropertyRelay(sampleInstance).CrossFadeAnimation;
    }

    public void OnStartAnimationButton_Clicked(object sender, RoutedEventArgs e)
    {
        _animationSet.Start();
    }

    public void OnStopAnimationButton_Clicked(object sender, RoutedEventArgs e)
    {
        _animationSet.Stop();
    }
}
