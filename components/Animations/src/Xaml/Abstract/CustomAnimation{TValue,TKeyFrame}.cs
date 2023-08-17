// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using static CommunityToolkit.WinUI.Animations.AnimationExtensions;

namespace CommunityToolkit.WinUI.Animations;

/// <summary>
/// A custom animation types, that can target both the composition and the XAML layer, and that
/// can adapt based on the context it is being used from (eg. explicit or implicit animation).
/// </summary>
/// <inheritdoc cref="Animation{TValue, TKeyFrame}"/>
public abstract class CustomAnimation<TValue, TKeyFrame> : ImplicitAnimation<TValue, TKeyFrame>
    where TKeyFrame : unmanaged
{
    /// <summary>
    /// Gets or sets the target property for the animation.
    /// </summary>
    public string? Target { get; set; }


// Suppression required for net7-android. The #if compilation conditionals are preventing the compiler from interpreting it correctly.
#pragma warning disable CS1587 // XML comment is not placed on a valid language element
    /// <summary>
    /// Gets or sets the target framework layer for the animation. This is only supported
    /// for a set of animation types (see the docs for more on this). Furthermore, this is
    /// ignored when the animation is being used as an implicit composition animation.
#if !HAS_UNO
    /// The default value is <see cref="FrameworkLayer.Composition"/>.
    /// </summary>
    public FrameworkLayer Layer { get; set; }
#else
    /// The default value is <see cref="FrameworkLayer.Xaml"/>.
    /// </summary>
    public FrameworkLayer Layer { get; set; } = FrameworkLayer.Xaml;
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
#endif

    /// <inheritdoc/>
    protected override string? ExplicitTarget => Target;

    /// <inheritdoc/>
    public override AnimationBuilder AppendToBuilder(AnimationBuilder builder, TimeSpan? delayHint, TimeSpan? durationHint, EasingType? easingTypeHint, EasingMode? easingModeHint)
    {
        default(ArgumentNullException).ThrowIfNull(ExplicitTarget);

        return builder.NormalizedKeyFrames<TKeyFrame, (CustomAnimation<TValue, TKeyFrame> This, EasingType? EasingTypeHint, EasingMode? EasingModeHint)>(
            property: ExplicitTarget,
            state: (this, easingTypeHint, easingModeHint),
            delay: Delay ?? delayHint ?? DefaultDelay,
            duration: Duration ?? durationHint ?? DefaultDuration,
            delayBehavior: DelayBehavior,
            layer: Layer,
            build: static (b, s) => s.This.AppendToBuilder(b, s.EasingTypeHint, s.EasingModeHint));
    }
}
