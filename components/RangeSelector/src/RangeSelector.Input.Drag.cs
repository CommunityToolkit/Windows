// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace CommunityToolkit.WinUI.Controls;

/// <summary>
/// RangeSelector is a "double slider" control for range values.
/// </summary>
public partial class RangeSelector : Control
{
    private void MinThumb_DragDelta(object sender, DragDeltaEventArgs e)
    {
        _absolutePosition += Orientation == Orientation.Horizontal ? e.HorizontalChange : e.VerticalChange;

        var maxThumbPos = _maxThumb.GetCanvasU(Orientation);
        RangeStart = Orientation == Orientation.Horizontal
            ? DragThumb(_minThumb, 0, maxThumbPos, _absolutePosition)
            : DragThumb(_minThumb, maxThumbPos, DragWidth(), _absolutePosition);

        if (_toolTipText != null)
        {
            UpdateToolTipText(this, _toolTipText, RangeStart);
        }
    }

    private void MaxThumb_DragDelta(object sender, DragDeltaEventArgs e)
    {
        _absolutePosition += Orientation == Orientation.Horizontal ? e.HorizontalChange : e.VerticalChange;

        var minThumbPos = _minThumb.GetCanvasU(Orientation);
        RangeEnd = Orientation == Orientation.Horizontal
            ? DragThumb(_maxThumb, minThumbPos, DragWidth(), _absolutePosition)
            : DragThumb(_maxThumb, 0, minThumbPos, _absolutePosition);

        if (_toolTipText != null)
        {
            UpdateToolTipText(this, _toolTipText, RangeEnd);
        }
    }

    private void MinThumb_DragStarted(object sender, DragStartedEventArgs e)
    {
        OnThumbDragStarted(e);
        if (_minThumb is not null)
        {
            Thumb_DragStarted(_minThumb);
        }
    }

    private void MaxThumb_DragStarted(object sender, DragStartedEventArgs e)
    {
        OnThumbDragStarted(e);
        if (_maxThumb is not null)
        {
            Thumb_DragStarted(_maxThumb);
        }
    }

    private void Thumb_DragCompleted(object sender, DragCompletedEventArgs e)
    {
        OnThumbDragCompleted(e);
        OnValueChanged(sender.Equals(_minThumb) ? new RangeChangedEventArgs(_oldValue, RangeStart, RangeSelectorProperty.MinimumValue) : new RangeChangedEventArgs(_oldValue, RangeEnd, RangeSelectorProperty.MaximumValue));
        SyncThumbs();

        if (_toolTip != null)
        {
            _toolTip.Visibility = Visibility.Collapsed;
        }

        VisualStateManager.GoToState(this, NormalState, true);
    }

    private double DragWidth()
    {
        return _containerCanvas.GetActualSizeU(Orientation) - _maxThumb.GetSizeU(Orientation);
    }

    private double DragThumb(Thumb? thumb, double min, double max, double nextPos)
    {
        var isHorizontal = Orientation == Orientation.Horizontal;

        nextPos = Math.Max(min, nextPos);
        nextPos = Math.Min(max, nextPos);

        // Position the thumb
        var thumbPos = new UVPoint(Orientation, nextPos);
        thumb.SetCanvasU(thumbPos);

        // Position the tooltip
        if (_toolTip != null && thumb != null)
        {
            var thumbSize = thumb.GetSizeU(Orientation);
            var thumbCenter = nextPos + (thumbSize / 2);
            _toolTip.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            var ttHalfSize = isHorizontal
                ? _toolTip.ActualWidth / 2
                : _toolTip.DesiredSize.Height / 2;

            var toolTipPos = new UVPoint(Orientation, thumbCenter - ttHalfSize);
            _toolTip.SetCanvasU(toolTipPos);

            if (!isHorizontal)
            {
                UpdateToolTipPositionForVertical();
            }
        }

        // Calculate the range value
        // Horizontal: left (0) = Minimum, right (DragWidth) = Maximum
        // Vertical: top (0) = Maximum, bottom (DragWidth) = Minimum (inverted)
        var ratio = nextPos / DragWidth();
        var range = Maximum - Minimum;

        return isHorizontal
            ? Minimum + (ratio * range)
            : Maximum - (ratio * range);
    }

    private void Thumb_DragStarted(Thumb thumb)
    {
        var useMin = thumb == _minThumb;
        var otherThumb = useMin ? _maxThumb : _minThumb;
        var isHorizontal = Orientation == Orientation.Horizontal;

        _absolutePosition = thumb.GetCanvasU(Orientation);
        Canvas.SetZIndex(thumb, 10);
        Canvas.SetZIndex(otherThumb, 0);
        _oldValue = RangeStart;

        if (_toolTip != null)
        {
            if (!isHorizontal && VerticalToolTipPlacement == VerticalToolTipPlacement.None)
            {
                _toolTip.Visibility = Visibility.Collapsed;
            }
            else
            {
                _toolTip.Visibility = Visibility.Visible;

                // Update tooltip text first so Measure gets accurate size
                if (_toolTipText != null)
                {
                    UpdateToolTipText(this, _toolTipText, useMin ? RangeStart : RangeEnd);
                }

                _toolTip.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));

                var thumbSize = thumb.GetSizeU(Orientation);
                var thumbCenter = _absolutePosition + (thumbSize / 2);
                var ttHalfSize = _toolTip.GetDesiredSizeU(Orientation) / 2;

                var toolTipPos = new UVPoint(Orientation, thumbCenter - ttHalfSize);
                _toolTip.SetCanvasU(toolTipPos);

                if (!isHorizontal)
                {
                    UpdateToolTipPositionForVertical();
                }
            }
        }

        VisualStateManager.GoToState(this, useMin ? MinPressedState : MaxPressedState, true);
    }
}
