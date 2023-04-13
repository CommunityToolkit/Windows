// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace CommunityToolkit.WinUI.Controls;

/// <summary>
/// This is an example control based off of the BoxPanel sample here: https://docs.microsoft.com/windows/apps/design/layout/boxpanel-example-custom-panel. If you need this similar sort of layout component for an application, see UniformGrid in the Toolkit.
/// It is provided as an example of how to inherit from another control like <see cref="Panel"/>.
/// You can choose to start here or from the <see cref="Primitives_ClassicBinding"/> or <see cref="Primitives_xBind"/> example components. Remove unused components and rename as appropriate.
/// </summary>
public partial class Primitives : Panel
{
    /// <summary>
    /// Identifies the <see cref="Orientation"/> property.
    /// </summary>
    public static readonly DependencyProperty OrientationProperty =
        DependencyProperty.Register(nameof(Orientation), typeof(Orientation), typeof(Primitives), new PropertyMetadata(null, OnOrientationChanged));

    /// <summary>
    /// Gets the preference of the rows/columns when there are a non-square number of children. Defaults to Vertical.
    /// </summary>
    public Orientation Orientation
    {
        get { return (Orientation)GetValue(OrientationProperty); }
        set { SetValue(OrientationProperty, value); }
    }

    // Invalidate our layout when the property changes.
    private static void OnOrientationChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
    {
        if (dependencyObject is Primitives panel)
        {
            panel.InvalidateMeasure();
        }
    }

    // Store calculations we want to use between the Measure and Arrange methods.
    int _columnCount;
    double _cellWidth, _cellHeight;

    protected override Size MeasureOverride(Size availableSize)
    {
        // Determine the square that can contain this number of items.
        var maxrc = (int)Math.Ceiling(Math.Sqrt(Children.Count));
        // Get an aspect ratio from availableSize, decides whether to trim row or column.
        var aspectratio = availableSize.Width / availableSize.Height;
        if (Orientation == Orientation.Vertical) { aspectratio = 1 / aspectratio; }

        int rowcount;

        // Now trim this square down to a rect, many times an entire row or column can be omitted.
        if (aspectratio > 1)
        {
            rowcount = maxrc;
            _columnCount = (maxrc > 2 && Children.Count <= maxrc * (maxrc - 1)) ? maxrc - 1 : maxrc;
        }
        else
        {
            rowcount = (maxrc > 2 && Children.Count <= maxrc * (maxrc - 1)) ? maxrc - 1 : maxrc;
            _columnCount = maxrc;
        }

        // Now that we have a column count, divide available horizontal, that's our cell width.
        _cellWidth = (int)Math.Floor(availableSize.Width / _columnCount);
        // Next get a cell height, same logic of dividing available vertical by rowcount.
        _cellHeight = Double.IsInfinity(availableSize.Height) ? Double.PositiveInfinity : availableSize.Height / rowcount;

        double maxcellheight = 0;

        foreach (UIElement child in Children)
        {
            child.Measure(new Size(_cellWidth, _cellHeight));
            maxcellheight = (child.DesiredSize.Height > maxcellheight) ? child.DesiredSize.Height : maxcellheight;
        }

        return LimitUnboundedSize(availableSize, maxcellheight);
    }

    // This method limits the panel height when no limit is imposed by the panel's parent.
    // That can happen to height if the panel is close to the root of main app window.
    // In this case, base the height of a cell on the max height from desired size
    // and base the height of the panel on that number times the #rows.
    Size LimitUnboundedSize(Size input, double maxcellheight)
    { 
        if (Double.IsInfinity(input.Height))
        {
            input.Height = maxcellheight * _columnCount;
            _cellHeight = maxcellheight;
        }
        return input;
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        int count = 1;
        double x, y;
        foreach (UIElement child in Children)
        {
            x = (count - 1) % _columnCount * _cellWidth;
            y = ((int)(count - 1) / _columnCount) * _cellHeight;
            Point anchorPoint = new Point(x, y);
            child.Arrange(new Rect(anchorPoint, child.DesiredSize));
            count++;
        }
        return finalSize;
    }
}
