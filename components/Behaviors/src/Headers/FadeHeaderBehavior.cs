// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.WinUI.Animations.Expressions;
using CommunityToolkit.WinUI.Behaviors.Internal;

#if WINUI3
using Microsoft.UI.Composition;
using Microsoft.UI.Xaml.Hosting;
using ListViewBase = Microsoft.UI.Xaml.Controls.ListViewBase;
#else
using Windows.UI.Composition;
using Windows.UI.Xaml.Hosting;
using ListViewBase = Windows.UI.Xaml.Controls.ListViewBase;
#endif

namespace CommunityToolkit.WinUI.Behaviors;

/// <summary>
/// Performs an fade animation on a ListView or GridView Header using composition.
/// </summary>
/// <seealso>
///     <cref>Microsoft.Xaml.Interactivity.Behavior{Windows.UI.Xaml.UIElement}</cref>
/// </seealso>
public class FadeHeaderBehavior : HeaderBehaviorBase
{
    /// <inheritdoc/>
    protected override bool AssignAnimation()
    {
        if (base.AssignAnimation())
        {
            // Get the ScrollViewer's ManipulationPropertySet
            var scrollPropSet = _scrollProperties!.GetSpecializedReference<ManipulationPropertySetReferenceNode>();

            // Use the ScrollViewer's Y offset and the header's height to calculate the opacity percentage. Clamp it between 0% and 100%
            var headerHeight = (float)AssociatedObject.RenderSize.Height;
            var opacityExpression = ExpressionFunctions.Clamp(1 - (-scrollPropSet.Translation.Y / headerHeight), 0, 1);

            // Begin animating
            _headerVisual?.StartAnimation("Opacity", opacityExpression);

            return true;
        }

        return false;
    }

    /// <summary>
    /// Stops the animation.
    /// </summary>
    protected override void StopAnimation()
    {
        if (_headerVisual != null)
        {
            _headerVisual.StopAnimation("Opacity");
            _headerVisual.Opacity = 1.0f;
        }
    }
}
