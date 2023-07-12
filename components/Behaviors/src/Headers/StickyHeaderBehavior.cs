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
/// Performs an animation on a ListView or GridView Header to make it 'sticky' and remain in view using composition.
/// </summary>
/// <seealso>
///     <cref>Microsoft.Xaml.Interactivity.Behavior{Microsoft.UI.Xaml.UIElement}</cref>
/// </seealso>
public class StickyHeaderBehavior : HeaderBehaviorBase
{
    /// <summary>
    /// Show the header
    /// </summary>
    public void Show()
    {
        if (_headerVisual != null && _scrollViewer != null)
        {
            _animationProperties?.InsertScalar("OffsetY", 0.0f);
        }
    }

    /// <inheritdoc/>
    protected override bool AssignAnimation()
    {
        if (base.AssignAnimation())
        {
            _animationProperties?.InsertScalar("OffsetY", 0.0f);

            var propSetOffset = _animationProperties!.GetReference().GetScalarProperty("OffsetY");
            var scrollPropSet = _scrollProperties!.GetSpecializedReference<ManipulationPropertySetReferenceNode>();
            var expressionAnimation = ExpressionFunctions.Max(propSetOffset - scrollPropSet.Translation.Y, 0);

            _headerVisual?.StartAnimation("Offset.Y", expressionAnimation);

            return true;
        }

        return false;
    }

    /// <inheritdoc/>
    protected override void StopAnimation()
    {
        _animationProperties?.InsertScalar("OffsetY", 0.0f);

        if (_headerVisual != null)
        {
            _headerVisual.StopAnimation("Offset.Y");

            var offset = _headerVisual.Offset;
            offset.Y = 0.0f;
            _headerVisual.Offset = offset;
        }
    }
}
