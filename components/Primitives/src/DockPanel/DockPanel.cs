// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace CommunityToolkit.WinUI.Controls;

/// <summary>
/// Defines an area where you can arrange child elements either horizontally or vertically, relative to each other.
/// </summary>
public partial class DockPanel : Panel
{
    private static void DockChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
        var senderElement = sender as FrameworkElement;
        var dockPanel = senderElement?.FindParent<DockPanel>();

        dockPanel?.InvalidateArrange();
    }

    private static void OnPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
        var dockPanel = (DockPanel)sender;
        dockPanel.InvalidateMeasure();
    }

    /// <inheritdoc />
    protected override Size ArrangeOverride(Size finalSize)
    {
        if (Children.Count is 0)
            return finalSize;

        var currentBounds = new Rect(
            Padding.Left,
            Padding.Top,
            Math.Max(0, finalSize.Width - Padding.Left - Padding.Right),
            Math.Max(0, finalSize.Height - Padding.Top - Padding.Bottom));
        var childrenCount = LastChildFill ? Children.Count - 1 : Children.Count;

        for (var index = 0; index < childrenCount; ++index)
        {
            var child = Children[index];
            if (child.Visibility is Visibility.Collapsed)
                continue;
            var dock = (Dock)child.GetValue(DockProperty);
            double width, height;
            switch (dock)
            {
                case Dock.Left:

                    width = Math.Min(child.DesiredSize.Width, currentBounds.Width);
                    child.Arrange(new Rect(currentBounds.X, currentBounds.Y, width, currentBounds.Height));
                    width += HorizontalSpacing;
                    currentBounds.X += width;
                    currentBounds.Width = Math.Max(0, currentBounds.Width - width);

                    break;
                case Dock.Top:

                    height = Math.Min(child.DesiredSize.Height, currentBounds.Height);
                    child.Arrange(new Rect(currentBounds.X, currentBounds.Y, currentBounds.Width, height));
                    height += VerticalSpacing;
                    currentBounds.Y += height;
                    currentBounds.Height = Math.Max(0, currentBounds.Height - height);

                    break;
                case Dock.Right:

                    width = Math.Min(child.DesiredSize.Width, currentBounds.Width);
                    child.Arrange(new Rect(currentBounds.X + currentBounds.Width - width, currentBounds.Y, width, currentBounds.Height));
                    width += HorizontalSpacing;
                    currentBounds.Width = Math.Max(0, currentBounds.Width - width);

                    break;
                case Dock.Bottom:

                    height = Math.Min(child.DesiredSize.Height, currentBounds.Height);
                    child.Arrange(new Rect(currentBounds.X, currentBounds.Y + currentBounds.Height - height, currentBounds.Width, height));
                    height += VerticalSpacing;
                    currentBounds.Height = Math.Max(0, currentBounds.Height - height);

                    break;
            }
        }

        if (LastChildFill && Children.Count > 0)
        {
            var child = Children[Children.Count - 1];
            child.Arrange(new Rect(currentBounds.X, currentBounds.Y, currentBounds.Width, currentBounds.Height));
        }

        return finalSize;
    }

    /// <inheritdoc />
    protected override Size MeasureOverride(Size availableSize)
    {
        var parentWidth = 0d;
        var parentHeight = 0d;
        var accumulatedWidth = 0d;
        var accumulatedHeight = 0d;

        var horizontalSpacing = false;
        var verticalSpacing = false;
        var childrenCount = LastChildFill ? Children.Count - 1 : Children.Count;

        for (var index = 0; index < childrenCount; ++index)
        {
            var child = Children[index];
            var childConstraint = new Size(
                Math.Max(0, availableSize.Width - accumulatedWidth),
                Math.Max(0, availableSize.Height - accumulatedHeight));

            child.Measure(childConstraint);
            var childDesiredSize = child.DesiredSize;

            switch (child.GetValue(DockProperty))
            {
                case Dock.Left:
                case Dock.Right:
                    parentHeight = Math.Max(parentHeight, accumulatedHeight + childDesiredSize.Height);
                    if (child.Visibility is Visibility.Visible)
                    {
                        accumulatedWidth += HorizontalSpacing;
                        horizontalSpacing = true;
                    }

                    accumulatedWidth += childDesiredSize.Width;
                    break;

                case Dock.Top:
                case Dock.Bottom:
                    parentWidth = Math.Max(parentWidth, accumulatedWidth + childDesiredSize.Width);
                    if (child.Visibility is Visibility.Visible)
                    {
                        accumulatedHeight += VerticalSpacing;
                        verticalSpacing = true;
                    }

                    accumulatedHeight += childDesiredSize.Height;
                    break;
            }
        }

        if (LastChildFill && Children.Count > 0)
        {
            var child = Children[Children.Count - 1];
            var childConstraint = new Size(
                Math.Max(0, availableSize.Width - accumulatedWidth),
                Math.Max(0, availableSize.Height - accumulatedHeight));

            child.Measure(childConstraint);
            var childDesiredSize = child.DesiredSize;
            parentHeight = Math.Max(parentHeight, accumulatedHeight + childDesiredSize.Height);
            parentWidth = Math.Max(parentWidth, accumulatedWidth + childDesiredSize.Width);
            accumulatedHeight += childDesiredSize.Height;
            accumulatedWidth += childDesiredSize.Width;
        }
        else
        {
            if (horizontalSpacing)
                accumulatedWidth -= HorizontalSpacing;
            if (verticalSpacing)
                accumulatedHeight -= VerticalSpacing;
        }

        // Make sure the final accumulated size is reflected in parentSize.
        parentWidth = Math.Max(parentWidth, accumulatedWidth);
        parentHeight = Math.Max(parentHeight, accumulatedHeight);
        return new Size(parentWidth, parentHeight);
    }
}
