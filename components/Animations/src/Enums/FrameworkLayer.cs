// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace CommunityToolkit.WinUI.Animations;

/// <summary>
/// An <see langword="enum"/> that indicates the framework layer to target in a specific animation.
/// </summary>
public enum FrameworkLayer
{
    /// <summary>
#if WINUI2
    /// Indicates the <see cref="Windows.UI.Composition"/> APIs.
#elif WINUI3
    /// Indicates the <see cref="Microsoft.UI.Composition"/> APIs.
#endif
    /// </summary>
    Composition,
    
    /// <summary>
#if WINUI2
    /// Indicates the <see cref="Windows.UI.Xaml.Media.Animation"/> APIs.
#elif WINUI3
    /// Indicates the <see cref="Microsoft.UI.Xaml.Media.Animation"/> APIs.
#endif
    /// </summary>
    Xaml
}
