// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace CommunityToolkit.WinUI.Controls;

/// <summary>
/// RangeSelector is a "double slider" control for range values.
/// </summary>
public partial class RangeSelector : Control
{
    private void ContainerCanvas_PointerEntered(object sender, PointerRoutedEventArgs e)
    {
        VisualStateManager.GoToState(this, PointerOverState, false);
    }

    private void ContainerCanvas_PointerExited(object sender, PointerRoutedEventArgs e)
    {
        var position = e.GetCurrentPoint(_containerCanvas).Position.X;
        var normalizedPosition = ((position / DragWidth()) * (Maximum - Minimum)) + Minimum;

        if (_pointerManipulatingMin)
        {
            _pointerManipulatingMin = false;
            OnValueChanged(new RangeChangedEventArgs(RangeStart, normalizedPosition, RangeSelectorProperty.MinimumValue));
        }
        else if (_pointerManipulatingMax)
        {
            _pointerManipulatingMax = false;
            OnValueChanged(new RangeChangedEventArgs(RangeEnd, normalizedPosition, RangeSelectorProperty.MaximumValue));
        }

        if (_containerCanvas is not null)
        {
            _containerCanvas.IsHitTestVisible = true;
        }

        if (_toolTip != null)
        {
            _toolTip.Visibility = Visibility.Collapsed;
        }

        VisualStateManager.GoToState(this, "Normal", false);
    }

    private void ContainerCanvas_PointerReleased(object sender, PointerRoutedEventArgs e)
    {
        var position = e.GetCurrentPoint(_containerCanvas).Position.X;
        var normalizedPosition = ((position / DragWidth()) * (Maximum - Minimum)) + Minimum;

        if (_pointerManipulatingMin)
        {
            _pointerManipulatingMin = false;
            OnValueChanged(new RangeChangedEventArgs(RangeStart, normalizedPosition, RangeSelectorProperty.MinimumValue));
        }
        else if (_pointerManipulatingMax)
        {
            _pointerManipulatingMax = false;
            OnValueChanged(new RangeChangedEventArgs(RangeEnd, normalizedPosition, RangeSelectorProperty.MaximumValue));
        }

        if (_containerCanvas is not null)
        {
            _containerCanvas.IsHitTestVisible = true;
        }

        SyncThumbs();

        if (_toolTip != null)
        {
            _toolTip.Visibility = Visibility.Collapsed;
        }
    }

    private void ContainerCanvas_PointerMoved(object sender, PointerRoutedEventArgs e)
    {
        var position = e.GetCurrentPoint(_containerCanvas).Position.X;
        var normalizedPosition = ((position / DragWidth()) * (Maximum - Minimum)) + Minimum;

        if (_pointerManipulatingMin)
        {
            RangeStart = DragThumb(_minThumb, 0, DragWidth(), position);
            if (_toolTipText is not null)
            {
                UpdateToolTipText(this, _toolTipText, RangeStart);
            }
        }
        else if (_pointerManipulatingMax)
        {
            if (_toolTipText is not null)
            {
                RangeEnd = DragThumb(_maxThumb, 0, DragWidth(), position);
                UpdateToolTipText(this, _toolTipText, RangeEnd);
            }
        }
    }

    private void ContainerCanvas_PointerPressed(object sender, PointerRoutedEventArgs e)
    {
        var position = e.GetCurrentPoint(_containerCanvas).Position.X;
        var normalizedPosition = position * Math.Abs(Maximum - Minimum) / DragWidth();
        double upperValueDiff = Math.Abs(RangeEnd - normalizedPosition);
        double lowerValueDiff = Math.Abs(RangeStart - normalizedPosition);

        if (upperValueDiff < lowerValueDiff)
        {
            RangeEnd = normalizedPosition;
            _pointerManipulatingMax = true;
            if (_maxThumb is not null)
            {
                Thumb_DragStarted(_maxThumb);
            }
        }
        else
        {
            RangeStart = normalizedPosition;
            _pointerManipulatingMin = true;
            if (_minThumb is not null)
            {
                Thumb_DragStarted(_minThumb);
            }
        }

        SyncThumbs();
    }
}
