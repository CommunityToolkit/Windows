// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#if WINAPPSDK
using Microsoft.UI.Composition;
#else
using Windows.UI.Composition;
#endif

namespace CommunityToolkit.WinUI.Animations;

/// <summary>
/// An opacity animation working on the composition layer.
/// </summary>
public sealed class OpacityDropShadowAnimation : ShadowAnimation<double?, double>
{
    /// <inheritdoc/>
    protected override string ExplicitTarget => nameof(DropShadow.Opacity);

    /// <inheritdoc/>
    protected override (double?, double?) GetParsedValues()
    {
        return (To, From);
    }
}
