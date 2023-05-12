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
/// A rotation animation working on the composition or layer.
/// </summary>
public sealed class RotationAnimation : ImplicitAnimation<double?, double>
{
    /// <inheritdoc/>
    protected override string ExplicitTarget => nameof(Visual.RotationAngle);

    /// <inheritdoc/>
    protected override (double?, double?) GetParsedValues()
    {
        return (To, From);
    }
}
