// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.WinUI.Media.Pipelines;

#if WINUI3
using Microsoft.UI.Composition;
using Microsoft.UI.Xaml.Hosting;
#elif WINUI2
using Windows.UI.Composition;
using Windows.UI.Xaml.Hosting;
#endif

namespace CommunityToolkit.WinUI.Media;

/// <summary>
/// A base class that extends <see cref="AttachedVisualFactoryBase"/> by leveraging the <see cref="PipelineBuilder"/> APIs.
/// </summary>
public abstract class PipelineVisualFactoryBase : AttachedVisualFactoryBase
{
    /// <inheritdoc/>
    public override async ValueTask<Visual> GetAttachedVisualAsync(UIElement element)
    {
        var visual = ElementCompositionPreview.GetElementVisual(element).Compositor.CreateSpriteVisual();

        visual.Brush = await OnPipelineRequested().BuildAsync();

        return visual;
    }

    /// <summary>
    /// A method that builds and returns the <see cref="PipelineBuilder"/> pipeline to use in the current instance.
    /// </summary>
    /// <returns>A <see cref="PipelineBuilder"/> instance to create the <see cref="Visual"/> to display.</returns>
    protected abstract PipelineBuilder OnPipelineRequested();
}
