// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
#if WINAPPSDK
using Microsoft.UI.Composition;
#else
using Windows.UI.Composition;
#endif
using Windows.UI;

namespace CommunityToolkit.WinUI.Animations;

/// <summary>
/// A custom <see cref="Color"/> animation on a <see cref="DropShadow"/>.
/// </summary>
public sealed class ColorDropShadowAnimation : ShadowAnimation<Color?, Color>
{
    /// <inheritdoc/>
    protected override string ExplicitTarget => nameof(DropShadow.Color);

    /// <inheritdoc/>
    protected override (Color?, Color?) GetParsedValues()
    {
        return (To, From);
    }
}
