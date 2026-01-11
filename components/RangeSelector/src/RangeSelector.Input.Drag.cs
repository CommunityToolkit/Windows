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
        var uvChange = new UVCoord(e.HorizontalChange, e.VerticalChange, Orientation);
        _absolutePosition += uvChange.U;

        var maxThumbPos = GetCanvasPos(_maxThumb).U;
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
        var uvChange = new UVCoord(e.HorizontalChange, e.VerticalChange, Orientation);
        _absolutePosition += uvChange.U;

        var minThumbPos = GetCanvasPos(_minThumb).U;
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
        return new UVCoord(_containerCanvas!.ActualWidth, _containerCanvas.ActualHeight, Orientation).U
             - new UVCoord(_maxThumb!.Width, _maxThumb.Height, Orientation).U;
    }

    private double DragThumb(Thumb? thumb, double min, double max, double nextPos)
    {
        nextPos = Math.Max(min, nextPos);
        nextPos = Math.Min(max, nextPos);

        // Position the thumb
        var thumbPos = new UVCoord(Orientation) { U = nextPos };
        Canvas.SetLeft(thumb, thumbPos.X);
        Canvas.SetTop(thumb, thumbPos.Y);

        // Position the tooltip
        if (_toolTip != null && thumb != null)
        {
            var thumbSize = new UVCoord(thumb.Width, thumb.Height, Orientation).U;
            var thumbCenter = nextPos + (thumbSize / 2);
            _toolTip.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            var ttHalfSize = new UVCoord(_toolTip.DesiredSize, Orientation).U / 2;

            var toolTipPos = new UVCoord(Orientation) { U = thumbCenter - ttHalfSize };
            Canvas.SetLeft(_toolTip, toolTipPos.X);
            Canvas.SetTop(_toolTip, toolTipPos.Y);

            if (Orientation == Orientation.Vertical)
            {
                UpdateToolTipPositionForVertical();
            }
        }

        // Calculate the range value
        // Horizontal: left (0) = Minimum, right (DragWidth) = Maximum
        // Vertical: top (0) = Maximum, bottom (DragWidth) = Minimum (inverted)
        var ratio = nextPos / DragWidth();
        var range = Maximum - Minimum;

        return Orientation == Orientation.Horizontal
            ? Minimum + (ratio * range)
            : Maximum - (ratio * range);
    }

    private void Thumb_DragStarted(Thumb thumb)
    {
        var useMin = thumb == _minThumb;
        var otherThumb = useMin ? _maxThumb : _minThumb;

        _absolutePosition = GetCanvasPos(thumb).U;
        Canvas.SetZIndex(thumb, 10);
        Canvas.SetZIndex(otherThumb, 0);
        _oldValue = RangeStart;

        if (_toolTip != null)
        {
            if (Orientation == Orientation.Vertical && VerticalToolTipPlacement == VerticalToolTipPlacement.None)
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

                var thumbSize = new UVCoord(thumb.Width, thumb.Height, Orientation).U;
                var thumbCenter = _absolutePosition + (thumbSize / 2);
                var ttHalfSize = new UVCoord(_toolTip.DesiredSize, Orientation).U / 2;

                var toolTipPos = new UVCoord(Orientation) { U = thumbCenter - ttHalfSize };
                Canvas.SetLeft(_toolTip, toolTipPos.X);
                Canvas.SetTop(_toolTip, toolTipPos.Y);

                if (Orientation == Orientation.Vertical)
                {
                    UpdateToolTipPositionForVertical();
                }
            }
        }

        VisualStateManager.GoToState(this, useMin ? MinPressedState : MaxPressedState, true);
    }
}
