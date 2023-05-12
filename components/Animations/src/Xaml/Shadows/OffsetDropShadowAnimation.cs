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
/// An offset animation working on the composition layer.
/// </summary>
public sealed class OffsetDropShadowAnimation : ShadowAnimation<string, Vector3>
{
    /// <inheritdoc/>
    protected override string ExplicitTarget => nameof(DropShadow.Offset);

    /// <inheritdoc/>
    protected override (Vector3?, Vector3?) GetParsedValues()
    {
        return (To?.ToVector3(), From?.ToVector3());
    }
}
