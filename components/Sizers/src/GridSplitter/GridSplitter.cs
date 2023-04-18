// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace CommunityToolkit.WinUI.Controls;

/// <summary>
/// Represents the control that redistributes space between columns or rows of a Grid control.
/// </summary>
public partial class GridSplitter : SizerBase
{
    private GridResizeDirection _resizeDirection;
    private GridResizeBehavior _resizeBehavior;

    /// <summary>
    /// Gets the target parent grid from level
    /// </summary>
    private FrameworkElement? TargetControl
    {
        get
        {
            if (ParentLevel == 0)
            {
                return this;
            }

            // TODO: Can we just use our Visual/Logical Tree extensions for this?
            var parent = Parent;
            for (int i = 2; i < ParentLevel; i++) // TODO: Why is this 2? We need better documentation on ParentLevel
            {
                if (parent is FrameworkElement frameworkElement)
                {
                    parent = frameworkElement.Parent;
                }
                else
                {
                    break;
                }
            }

            return parent as FrameworkElement;
        }
    }

    /// <summary>
    /// Gets GridSplitter Container Grid
    /// </summary>
    private Grid? Resizable => TargetControl?.Parent as Grid;

    /// <summary>
    /// Gets the current Column definition of the parent Grid
    /// </summary>
    private ColumnDefinition? CurrentColumn
    {
        get
        {
            if (Resizable == null)
            {
                return null;
            }

            var gridSplitterTargetedColumnIndex = GetTargetedColumn();

            if ((gridSplitterTargetedColumnIndex >= 0)
                && (gridSplitterTargetedColumnIndex < Resizable.ColumnDefinitions.Count))
            {
                return Resizable.ColumnDefinitions[gridSplitterTargetedColumnIndex];
            }

            return null;
        }
    }

    /// <summary>
    /// Gets the Sibling Column definition of the parent Grid
    /// </summary>
    private ColumnDefinition? SiblingColumn
    {
        get
        {
            if (Resizable == null)
            {
                return null;
            }

            var gridSplitterSiblingColumnIndex = GetSiblingColumn();

            if ((gridSplitterSiblingColumnIndex >= 0)
                && (gridSplitterSiblingColumnIndex < Resizable.ColumnDefinitions.Count))
            {
                return Resizable.ColumnDefinitions[gridSplitterSiblingColumnIndex];
            }

            return null;
        }
    }

    /// <summary>
    /// Gets the current Row definition of the parent Grid
    /// </summary>
    private RowDefinition? CurrentRow
    {
        get
        {
            if (Resizable == null)
            {
                return null;
            }

            var gridSplitterTargetedRowIndex = GetTargetedRow();

            if ((gridSplitterTargetedRowIndex >= 0)
                && (gridSplitterTargetedRowIndex < Resizable.RowDefinitions.Count))
            {
                return Resizable.RowDefinitions[gridSplitterTargetedRowIndex];
            }

            return null;
        }
    }

    /// <summary>
    /// Gets the Sibling Row definition of the parent Grid
    /// </summary>
    private RowDefinition? SiblingRow
    {
        get
        {
            if (Resizable == null)
            {
                return null;
            }

            var gridSplitterSiblingRowIndex = GetSiblingRow();

            if ((gridSplitterSiblingRowIndex >= 0)
                && (gridSplitterSiblingRowIndex < Resizable.RowDefinitions.Count))
            {
                return Resizable.RowDefinitions[gridSplitterSiblingRowIndex];
            }

            return null;
        }
    }
}
