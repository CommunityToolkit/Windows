// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
#if WINAPPSDK
using Microsoft.UI.Composition;
#else
using Windows.UI.Composition;
#endif
using System.Numerics;

namespace CommunityToolkit.WinUI.Animations;

/// <summary>
/// An anchor point animation working on the composition layer.
/// </summary>
public sealed class AnchorPointAnimation : ImplicitAnimation<string, Vector2>
{
    /// <inheritdoc/>
    protected override string ExplicitTarget => nameof(Visual.AnchorPoint);

    /// <inheritdoc/>
    protected override (Vector2?, Vector2?) GetParsedValues()
    {
        return (To?.ToVector2(), From?.ToVector2());
    }
}
