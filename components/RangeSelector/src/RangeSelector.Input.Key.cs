// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#if !WINAPPSDK
using Windows.System;
#else
using Microsoft.UI.Dispatching;
#endif
using VirtualKey = Windows.System.VirtualKey;

namespace CommunityToolkit.WinUI.Controls;

/// <summary>
/// RangeSelector is a "double slider" control for range values.
/// </summary>
public partial class RangeSelector : Control
{
    private readonly DispatcherQueueTimer keyDebounceTimer = DispatcherQueue.GetForCurrentThread().CreateTimer();

    private void MinThumb_KeyDown(object sender, KeyRoutedEventArgs e)
    {
        var change = GetKeyboardChange(e.Key);

        if (change != 0)
        {
            RangeStart += change;

            SyncThumbs(fromMinKeyDown: true);
            ShowToolTip();
            e.Handled = true;
        }
    }

    private double GetKeyboardChange(VirtualKey key)
    {
        var isHorizontal = Orientation == Orientation.Horizontal;
        var isRtl = FlowDirection == FlowDirection.RightToLeft;

        if (isHorizontal)
        {
            switch (key)
            {
                case VirtualKey.Left:
                    return isRtl ? StepFrequency : -StepFrequency;
                case VirtualKey.Right:
                    return isRtl ? -StepFrequency : StepFrequency;
            }
        }
        else // Vertical
        {
            switch (key)
            {
                case VirtualKey.Down:
                    return -StepFrequency;
                case VirtualKey.Up:
                    return StepFrequency;
            }
        }

        return 0;
    }

    private void MaxThumb_KeyDown(object sender, KeyRoutedEventArgs e)
    {
        var change = GetKeyboardChange(e.Key);

        if (change != 0)
        {
            RangeEnd += change;

            SyncThumbs(fromMaxKeyDown: true);
            ShowToolTip();
            e.Handled = true;
        }
    }

    private void ShowToolTip()
    {
        var isHorizontal = Orientation == Orientation.Horizontal;
        if (!isHorizontal && VerticalToolTipPlacement == VerticalToolTipPlacement.None) return;
        if (_toolTip != null)
        {
            _toolTip.Visibility = Visibility.Visible;
        }
    }

    private void Thumb_KeyUp(object sender, KeyRoutedEventArgs e)
    {
        switch (e.Key)
        {
            case VirtualKey.Left:
            case VirtualKey.Right:
            case VirtualKey.Up:
            case VirtualKey.Down:
                if (_toolTip != null)
                {
                    keyDebounceTimer.Debounce(
                        () => _toolTip.Visibility = Visibility.Collapsed,
                        TimeToHideToolTipOnKeyUp);
                }

                e.Handled = true;
                break;
        }
    }
}
