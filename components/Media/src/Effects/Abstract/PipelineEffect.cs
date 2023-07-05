// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using CommunityToolkit.WinUI.Media.Pipelines;

#if WINUI2
using Windows.UI.Composition;
#elif WINUI3
using Microsoft.UI.Composition;
#endif

#nullable enable

namespace CommunityToolkit.WinUI.Media;

/// <summary>
/// A base pipeline effect.
/// </summary>
public abstract class PipelineEffect : DependencyObject, IPipelineEffect
{
    /// <inheritdoc/>
    public CompositionBrush? Brush { get; private set; }

    /// <summary>
    /// Gets or sets a value indicating whether the effect can be animated.
    /// </summary>
    public bool IsAnimatable { get; set; }

    /// <inheritdoc/>
    public abstract PipelineBuilder AppendToBuilder(PipelineBuilder builder);

    /// <inheritdoc/>
    public virtual void NotifyCompositionBrushInUse(CompositionBrush brush)
    {
        Brush = brush;
    }
}
