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
/// Performs an animation on a ListView or GridView Header to make it quick return using composition.
/// </summary>
/// <seealso>
///     <cref>Microsoft.Xaml.Interactivity.Behavior{Windows.UI.Xaml.UIElement}</cref>
/// </seealso>
public class QuickReturnHeaderBehavior : HeaderBehaviorBase
{
    private double _headerPosition;
    
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

            _scrollViewer!.ViewChanged -= ScrollViewer_ViewChanged;
            _scrollViewer!.ViewChanged += ScrollViewer_ViewChanged;

            var propSetOffset = _animationProperties!.GetReference().GetScalarProperty("OffsetY");
            var scrollPropSet = _scrollProperties!.GetSpecializedReference<ManipulationPropertySetReferenceNode>();
            var expressionAnimation = ExpressionFunctions.Max(ExpressionFunctions.Min(propSetOffset, -scrollPropSet.Translation.Y), 0);

            _headerVisual?.StartAnimation("Offset.Y", expressionAnimation);

            return true;
        }

        return false;
    }

    /// <inheritdoc/>
    protected override void StopAnimation()
    {
        _headerVisual?.StopAnimation("Offset.Y");

        _animationProperties?.InsertScalar("OffsetY", 0.0f);

        if (_headerVisual != null)
        {
            var offset = _headerVisual.Offset;
            offset.Y = 0.0f;
            _headerVisual.Offset = offset;
        }
    }

    /// <inheritdoc/>
    protected override void RemoveAnimation()
    {
        if (_scrollViewer != null)
        {
            _scrollViewer.ViewChanged -= ScrollViewer_ViewChanged;
        }

        base.RemoveAnimation();
    }

    private void ScrollViewer_ViewChanged(object? sender, ScrollViewerViewChangedEventArgs e)
    {
        if (_animationProperties != null && _scrollViewer != null)
        {
            FrameworkElement header = (FrameworkElement)HeaderElement;
            var headerHeight = header.ActualHeight;
            if (_headerPosition + headerHeight < _scrollViewer.VerticalOffset)
            {
                // scrolling down: move header down, so it is just above screen
                _headerPosition = _scrollViewer.VerticalOffset - headerHeight;
                _animationProperties.InsertScalar("OffsetY", (float)_headerPosition);
            }
            else if (_headerPosition > _scrollViewer.VerticalOffset)
            {
                // scrolling up: move header up, align with top border.
                // the expression animation makes sure it never really is shown below border, so no lag effect!
                _headerPosition = _scrollViewer.VerticalOffset;
                _animationProperties.InsertScalar("OffsetY", (float)_headerPosition);
            }
        }
    }
}
