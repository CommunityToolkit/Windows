// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#if WINAPPSDK
using Microsoft.UI.Composition;
#else
using Windows.UI.Composition;
#endif

namespace CommunityToolkit.WinUI.Animations.Expressions;

/// <summary>
/// Class AmbientLightReferenceNode. This class cannot be inherited.
/// </summary>
/// <seealso cref="ReferenceNode" />
public sealed partial class AmbientLightReferenceNode : ReferenceNode
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AmbientLightReferenceNode"/> class.
    /// </summary>
    /// <param name="paramName">Name of the parameter.</param>
    /// <param name="light">The light.</param>
    internal AmbientLightReferenceNode(string paramName, AmbientLight? light = null)
        : base(paramName, light)
    {
    }

    /// <summary>
    /// Creates the target reference.
    /// </summary>
    /// <returns>AmbientLightReferenceNode.</returns>
    internal static AmbientLightReferenceNode CreateTargetReference()
    {
        var node = new AmbientLightReferenceNode(null!);
        node.NodeType = ExpressionNodeType.TargetReference;

        return node;
    }

    /// <summary>
    /// Gets the color.
    /// </summary>
    /// <value>The color.</value>
    public ColorNode Color
    {
        get { return ReferenceProperty<ColorNode>("Color"); }
    }
}
