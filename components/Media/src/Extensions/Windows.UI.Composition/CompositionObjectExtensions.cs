// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Numerics;
using Windows.UI;

#if WINUI3
using Microsoft.UI.Composition;
using Microsoft.UI.Xaml.Hosting;
#elif WINUI2
using Windows.UI.Composition;
using Windows.UI.Xaml.Hosting;
#endif

namespace CommunityToolkit.WinUI.Media;

/// <summary>
/// An extension <see langword="class"/> for the <see cref="CompositionObject"/> type
/// </summary>
internal static class CompositionObjectExtensions
{
    /// <summary>
    /// Starts an <see cref="ExpressionAnimation"/> to keep the size of the source <see cref="Visual"/> in sync with the target <see cref="UIElement"/>
    /// </summary>
    /// <param name="source">The <see cref="Visual"/> to start the animation on</param>
    /// <param name="target">The target <see cref="UIElement"/> to read the size updates from</param>
    public static void BindSize(this Visual source, UIElement target)
    {
        var visual = ElementCompositionPreview.GetElementVisual(target);
        var bindSizeAnimation = source.Compositor.CreateExpressionAnimation($"{nameof(visual)}.Size");

        bindSizeAnimation.SetReferenceParameter(nameof(visual), visual);

        // Start the animation
        source.StartAnimation("Size", bindSizeAnimation);
    }

    /// <summary>
    /// Starts an animation on the given property of a <see cref="CompositionObject"/>
    /// </summary>
    /// <typeparam name="T">The type of the property to animate</typeparam>
    /// <param name="target">The target <see cref="CompositionObject"/></param>
    /// <param name="property">The name of the property to animate</param>
    /// <param name="value">The final value of the property</param>
    /// <param name="duration">The animation duration</param>
    /// <returns>A <see cref="Task"/> that completes when the created animation completes</returns>
    public static Task StartAnimationAsync<T>(this CompositionObject target, string property, T value, TimeSpan duration)
        where T : unmanaged
    {
        // Stop previous animations
        target.StopAnimation(property);

        // Setup the animation to run
        KeyFrameAnimation animation;
        switch (value)
        {
            case float f:
                var scalarAnimation = target.Compositor.CreateScalarKeyFrameAnimation();
                scalarAnimation.InsertKeyFrame(1f, f);
                animation = scalarAnimation;
                break;
            case Color c:
                var colorAnimation = target.Compositor.CreateColorKeyFrameAnimation();
                colorAnimation.InsertKeyFrame(1f, c);
                animation = colorAnimation;
                break;
            case Vector4 v4:
                var vector4Animation = target.Compositor.CreateVector4KeyFrameAnimation();
                vector4Animation.InsertKeyFrame(1f, v4);
                animation = vector4Animation;
                break;
            default: throw new ArgumentException($"Invalid animation type: {typeof(T)}", nameof(value));
        }

        animation.Duration = duration;

        // Get the batch and start the animations
        var batch = target.Compositor.CreateScopedBatch(CompositionBatchTypes.Animation);

        var tcs = new TaskCompletionSource<object?>();

        batch.Completed += (s, e) => tcs.SetResult(null);

        target.StartAnimation(property, animation);

        batch.End();

        return tcs.Task;
    }
}
