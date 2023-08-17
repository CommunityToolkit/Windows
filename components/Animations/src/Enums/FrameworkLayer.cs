// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#if WINUI2
using Composition = Windows.UI.Composition;
using Animation = Windows.UI.Xaml.Media.Animation;
#elif WINUI3
using Composition = Microsoft.UI.Composition;
using Animation = Microsoft.UI.Xaml.Media.Animation;
#endif

namespace CommunityToolkit.WinUI.Animations;

/// <summary>
/// An <see langword="enum"/> that indicates the framework layer to target in a specific animation.
/// </summary>
public enum FrameworkLayer
{
    /// <summary>
    /// Indicates the <see cref="Composition"/> APIs.
    /// </summary>
    Composition,
    
    /// <summary>
    /// Indicates the <see cref="Animation"/> APIs.
    /// </summary>
    Xaml
}
