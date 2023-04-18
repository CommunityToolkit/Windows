// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.WinUI.Controls.SizerBaseLocal;

namespace CommunityToolkit.WinUI.Controls;

// Events for ContentSizer.
public partial class ContentSizer
{
    /// <inheritdoc/>
    protected override void OnLoaded(RoutedEventArgs e)
    {
        if (TargetControl == null)
        {
            TargetControl = this.FindAscendant<FrameworkElement>();
        }
    }

    private double _currentSize;

    /// <inheritdoc/>
    protected override void OnDragStarting()
    {
        if (TargetControl != null)
        {
            _currentSize =
                Orientation == Orientation.Vertical ?
                    TargetControl.ActualWidth :
                    TargetControl.ActualHeight;
        }
    }

    /// <inheritdoc/>
    protected override bool OnDragHorizontal(double horizontalChange)
    {
        if (TargetControl == null)
        {
            return true;
        }

        horizontalChange = IsDragInverted ? -horizontalChange : horizontalChange;

        if (!IsValidWidth(TargetControl, _currentSize + horizontalChange, ActualWidth))
        {
            return false;
        }

        TargetControl.Width = _currentSize + horizontalChange;

        return true;
    }

    /// <inheritdoc/>
    protected override bool OnDragVertical(double verticalChange)
    {
        if (TargetControl == null)
        {
            return false;
        }

        verticalChange = IsDragInverted ? -verticalChange : verticalChange;

        if (!IsValidHeight(TargetControl, _currentSize + verticalChange, ActualHeight))
        {
            return false;
        }

        TargetControl.Height = _currentSize + verticalChange;

        return true;
    }
}
