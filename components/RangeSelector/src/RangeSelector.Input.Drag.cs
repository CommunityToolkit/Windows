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
        var isHorizontal = Orientation == Orientation.Horizontal;
        _absolutePosition += isHorizontal ? e.HorizontalChange : e.VerticalChange;

        RangeStart = isHorizontal
            ? DragThumb(_minThumb, 0, Canvas.GetLeft(_maxThumb), _absolutePosition)
            : DragThumbVertical(_minThumb, Canvas.GetTop(_maxThumb), DragWidth(), _absolutePosition);

        if (_toolTipText != null)
        {
            UpdateToolTipText(this, _toolTipText, RangeStart);
        }
    }

    private void MaxThumb_DragDelta(object sender, DragDeltaEventArgs e)
    {
        var isHorizontal = Orientation == Orientation.Horizontal;
        _absolutePosition += isHorizontal ? e.HorizontalChange : e.VerticalChange;

        RangeEnd = isHorizontal
            ? DragThumb(_maxThumb, Canvas.GetLeft(_minThumb), DragWidth(), _absolutePosition)
            : DragThumbVertical(_maxThumb, 0, Canvas.GetTop(_minThumb), _absolutePosition);

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
        return Orientation == Orientation.Horizontal
            ? _containerCanvas!.ActualWidth - _maxThumb!.Width
            : _containerCanvas!.ActualHeight - _maxThumb!.Height;
    }

    private double DragThumb(Thumb? thumb, double min, double max, double nextPos)
    {
        nextPos = Math.Max(min, nextPos);
        nextPos = Math.Min(max, nextPos);

        Canvas.SetLeft(thumb, nextPos);

        if (_toolTip != null && thumb != null)
        {
            var thumbCenter = nextPos + (thumb.Width / 2);
            _toolTip.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            var ttWidth = _toolTip.ActualWidth / 2;

            Canvas.SetLeft(_toolTip, thumbCenter - ttWidth);
        }

        return Minimum + ((nextPos / DragWidth()) * (Maximum - Minimum));
    }

    private double DragThumbVertical(Thumb? thumb, double min, double max, double nextPos)
    {
        nextPos = Math.Max(min, nextPos);
        nextPos = Math.Min(max, nextPos);

        Canvas.SetTop(thumb, nextPos);

        if (_toolTip != null && thumb != null)
        {
            var thumbCenter = nextPos + (thumb.Height / 2);
            _toolTip.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            var ttHeight = _toolTip.ActualHeight / 2;

            Canvas.SetTop(_toolTip, thumbCenter - ttHeight);
        }

        // Invert: top position (0) = Maximum, bottom position (DragWidth) = Minimum
        return Maximum - ((nextPos / DragWidth()) * (Maximum - Minimum));
    }

    private void Thumb_DragStarted(Thumb thumb)
    {
        var useMin = thumb == _minThumb;
        var otherThumb = useMin ? _maxThumb : _minThumb;
        var isHorizontal = Orientation == Orientation.Horizontal;

        _absolutePosition = isHorizontal ? Canvas.GetLeft(thumb) : Canvas.GetTop(thumb);
        Canvas.SetZIndex(thumb, 10);
        Canvas.SetZIndex(otherThumb, 0);
        _oldValue = RangeStart;

        if (_toolTip != null)
        {
            _toolTip.Visibility = Visibility.Visible;
            _toolTip.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));

            if (isHorizontal)
            {
                var thumbCenter = _absolutePosition + (thumb.Width / 2);
                Canvas.SetLeft(_toolTip, thumbCenter - (_toolTip.ActualWidth / 2));
            }
            else
            {
                var thumbCenter = _absolutePosition + (thumb.Height / 2);
                Canvas.SetTop(_toolTip, thumbCenter - (_toolTip.ActualHeight / 2));
            }

            if (_toolTipText != null)
            {
                UpdateToolTipText(this, _toolTipText, useMin ? RangeStart : RangeEnd);
            }
        }

        VisualStateManager.GoToState(this, useMin ? MinPressedState : MaxPressedState, true);
    }
}
