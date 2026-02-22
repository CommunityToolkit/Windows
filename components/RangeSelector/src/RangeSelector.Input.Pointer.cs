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
        var point = new UVCoord(e.GetCurrentPoint(_containerCanvas).Position, Orientation);
        var normalizedPosition = CalculateNormalizedPosition(point.U);

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

        VisualStateManager.GoToState(this, NormalState, false);
    }

    private void ContainerCanvas_PointerReleased(object sender, PointerRoutedEventArgs e)
    {
        var point = new UVCoord(e.GetCurrentPoint(_containerCanvas).Position, Orientation);
        var normalizedPosition = CalculateNormalizedPosition(point.U);

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
        var point = new UVCoord(e.GetCurrentPoint(_containerCanvas).Position, Orientation);

        if (_pointerManipulatingMin)
        {
            var maxThumbPos = GetCanvasPos(_maxThumb).U;
            RangeStart = Orientation == Orientation.Horizontal
                ? DragThumb(_minThumb, 0, maxThumbPos, point.U)
                : DragThumb(_minThumb, maxThumbPos, DragWidth(), point.U);

            if (_toolTipText is not null)
            {
                UpdateToolTipText(this, _toolTipText, RangeStart);
            }
        }
        else if (_pointerManipulatingMax)
        {
            var minThumbPos = GetCanvasPos(_minThumb).U;
            RangeEnd = Orientation == Orientation.Horizontal
                ? DragThumb(_maxThumb, minThumbPos, DragWidth(), point.U)
                : DragThumb(_maxThumb, 0, minThumbPos, point.U);

            if (_toolTipText is not null)
            {
                UpdateToolTipText(this, _toolTipText, RangeEnd);
            }
        }
    }

    private void ContainerCanvas_PointerPressed(object sender, PointerRoutedEventArgs e)
    {
        var point = new UVCoord(e.GetCurrentPoint(_containerCanvas).Position, Orientation);
        var normalizedPosition = CalculateNormalizedPosition(point.U);

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

    /// <summary>
    /// Converts a position along the primary axis to a normalized value in the range [Minimum, Maximum].
    /// Handles vertical inversion automatically.
    /// </summary>
    private double CalculateNormalizedPosition(double position)
    {
        var ratio = position / DragWidth();
        var range = Maximum - Minimum;

        return Orientation == Orientation.Horizontal
            ? (ratio * range) + Minimum
            : Maximum - (ratio * range);
    }
}
