// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace CommunityToolkit.WinUI.Controls;

public partial class GridSplitter
{
    /// <inheritdoc/>
    protected override void OnLoaded(RoutedEventArgs e)
    {
        _resizeDirection = GetResizeDirection();
        Orientation = _resizeDirection == GridResizeDirection.Rows ?
            Orientation.Horizontal : Orientation.Vertical;
        _resizeBehavior = GetResizeBehavior();
    }

    private double _currentSize;
    private double _siblingSize;

    /// <inheritdoc />
    protected override void OnDragStarting()
    {
        _resizeDirection = GetResizeDirection();
        Orientation = _resizeDirection == GridResizeDirection.Rows ?
            Orientation.Horizontal : Orientation.Vertical;
        _resizeBehavior = GetResizeBehavior();

        // Record starting points
        if (Orientation == Orientation.Horizontal)
        {
            _currentSize = CurrentRow?.ActualHeight ?? -1;
            _siblingSize = SiblingRow?.ActualHeight ?? -1;
        }
        else
        {
            _currentSize = CurrentColumn?.ActualWidth ?? -1;
            _siblingSize = SiblingColumn?.ActualWidth ?? -1;
        }
    }

    /// <inheritdoc/>
    protected override bool OnDragVertical(double verticalChange)
    {
        if (CurrentRow == null || SiblingRow == null || Resizable == null)
        {
            return false;
        }

        var currentChange = _currentSize + verticalChange;
        var siblingChange = _siblingSize + (verticalChange * -1); // sibling moves opposite

        // Would changing the columnn sizes violate the constraints?
        if (!IsValidRowHeight(CurrentRow, currentChange) || !IsValidRowHeight(SiblingRow, siblingChange))
        {
            return false;
        }

        // NOTE: If the column contains another row with Star sizing, it's not enough to just change current.
        // The change will flow to the Star sized item and not to the sibling if the sibling is fixed-size.
        // So, we need to explicitly apply the change to the sibling.

        // if current row has fixed height then resize it
        if (!IsStarRow(CurrentRow))
        {
            // No need to check for the row Min height because it is automatically respected
            var changed = SetRowHeight(CurrentRow, currentChange, GridUnitType.Pixel);

            if (!IsStarRow(SiblingRow))
            {
                changed = SetRowHeight(SiblingRow, siblingChange, GridUnitType.Pixel);
            }

            return changed;
        }

        // if sibling row has fixed width then resize it
        else if (!IsStarRow(SiblingRow))
        {
            return SetRowHeight(SiblingRow, siblingChange, GridUnitType.Pixel);
        }

        // if both row haven't fixed height (auto *)
        else
        {
            // change current row height to the new height with respecting the auto
            // change sibling row height to the new height relative to current row
            // respect the other star row height by setting it's height to it's actual height with stars

            // We need to validate current and sibling height to not cause any unexpected behavior
            if (!IsValidRowHeight(CurrentRow, currentChange) ||
                !IsValidRowHeight(SiblingRow, siblingChange))
            {
                return false;
            }

            foreach (var rowDefinition in Resizable.RowDefinitions)
            {
                if (rowDefinition == CurrentRow)
                {
                    SetRowHeight(CurrentRow, currentChange, GridUnitType.Star);
                }
                else if (rowDefinition == SiblingRow)
                {
                    SetRowHeight(SiblingRow, siblingChange, GridUnitType.Star);
                }
                else if (IsStarRow(rowDefinition))
                {
                    rowDefinition.Height = new GridLength(rowDefinition.ActualHeight, GridUnitType.Star);
                }
            }

            return true;
        }
    }

    /// <inheritdoc/>
    protected override bool OnDragHorizontal(double horizontalChange)
    {
        if (CurrentColumn == null || SiblingColumn == null || Resizable == null)
        {
            return false;
        }

        var currentChange = _currentSize + horizontalChange;
        var siblingChange = _siblingSize + (horizontalChange * -1); // sibling moves opposite

        // Would changing the columnn sizes violate the constraints?
        if (!IsValidColumnWidth(CurrentColumn, currentChange) || !IsValidColumnWidth(SiblingColumn, siblingChange))
        {
            return false;
        }

        // NOTE: If the row contains another column with Star sizing, it's not enough to just change current.
        // The change will flow to the Star sized item and not to the sibling if the sibling is fixed-size.
        // So, we need to explicitly apply the change to the sibling.

        // if current column has fixed width then resize it
        if (!IsStarColumn(CurrentColumn))
        {
            // No need to check for the Column Min width because it is automatically respected
            var changed = SetColumnWidth(CurrentColumn, currentChange, GridUnitType.Pixel);

            if (!IsStarColumn(SiblingColumn))
            {
                changed = SetColumnWidth(SiblingColumn, siblingChange, GridUnitType.Pixel);
            }

            return changed;
        }

        // if sibling column has fixed width then resize it
        else if (!IsStarColumn(SiblingColumn))
        {
            return SetColumnWidth(SiblingColumn, siblingChange, GridUnitType.Pixel);
        }

        // if both column haven't fixed width (auto *)
        else
        {
            // change current column width to the new width with respecting the auto
            // change sibling column width to the new width relative to current column
            // respect the other star column width by setting it's width to it's actual width with stars

            // We need to validate current and sibling width to not cause any unexpected behavior
            if (!IsValidColumnWidth(CurrentColumn, currentChange) ||
                !IsValidColumnWidth(SiblingColumn, siblingChange))
            {
                return false;
            }

            foreach (var columnDefinition in Resizable.ColumnDefinitions)
            {
                if (columnDefinition == CurrentColumn)
                {
                    SetColumnWidth(CurrentColumn, currentChange, GridUnitType.Star);
                }
                else if (columnDefinition == SiblingColumn)
                {
                    SetColumnWidth(SiblingColumn, siblingChange, GridUnitType.Star);
                }
                else if (IsStarColumn(columnDefinition))
                {
                    columnDefinition.Width = new GridLength(columnDefinition.ActualWidth, GridUnitType.Star);
                }
            }

            return true;
        }
    }
}
