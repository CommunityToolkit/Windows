// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace CommunityToolkit.WinUI.Animations;

#if WINUI2
using Windows.UI.Composition;
#else
using Microsoft.UI.Composition;
#endif

/// <summary>
/// Indicates a property of a <see cref="Visual"/> object.
/// </summary>
public enum VisualProperty
{
    /// <summary>
    /// The offset property of a <see cref="Visual"/> object.
    /// </summary>
    Offset,

    /// <summary>
    /// The translation property of a <see cref="Visual"/> object.
    /// </summary>
    Translation
}
