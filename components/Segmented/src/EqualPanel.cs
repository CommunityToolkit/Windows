// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Data;

namespace CommunityToolkit.WinUI.Controls;

/// <summary>
/// A panel that arranges its children in equal columns.
/// </summary>
public partial class EqualPanel : Panel
{
    private double _maxItemWidth = 0;
    private double _maxItemHeight = 0;
    private int _visibleItemsCount = 0;
    
    /// <summary>
    /// Gets or sets the spacing between items.
    /// </summary>
    public double Spacing
    {
        get { return (double)GetValue(SpacingProperty); }
        set { SetValue(SpacingProperty, value); }
    }

    /// <summary>
    /// Identifies the Spacing dependency property.
    /// </summary>
    /// <returns>The identifier for the <see cref="Spacing"/> dependency property.</returns>
    public static readonly DependencyProperty SpacingProperty = DependencyProperty.Register(
        nameof(Spacing),
        typeof(double),
        typeof(EqualPanel),
        new PropertyMetadata(default(double), OnSpacingChanged));

    /// <summary>
    /// Creates a new instance of the <see cref="EqualPanel"/> class.
    /// </summary>
    public EqualPanel()
    {
        RegisterPropertyChangedCallback(HorizontalAlignmentProperty, OnHorizontalAlignmentChanged);
    }

    /// <inheritdoc/>
    protected override Size MeasureOverride(Size availableSize)
    {
        _maxItemWidth = 0;
        _maxItemHeight = 0;

        var elements = Children.Where(static e => e.Visibility == Visibility.Visible);
        _visibleItemsCount = elements.Count();

        foreach (var child in elements)
        {
            child.Measure(availableSize);
            _maxItemWidth = Math.Max(_maxItemWidth, child.DesiredSize.Width);
            _maxItemHeight = Math.Max(_maxItemHeight, child.DesiredSize.Height);
        }

        if (_visibleItemsCount > 0)
        {
            // Return equal widths based on the widest item
            // In very specific edge cases the AvailableWidth might be infinite resulting in a crash.
            if (HorizontalAlignment != HorizontalAlignment.Stretch || double.IsInfinity(availableSize.Width))
            {
                return new Size((_maxItemWidth * _visibleItemsCount) + (Spacing * (_visibleItemsCount - 1)), _maxItemHeight);
            }
            else
            {
                // Equal columns based on the available width, adjust for spacing
                double totalWidth = availableSize.Width - (Spacing * (_visibleItemsCount - 1));
                _maxItemWidth = totalWidth / _visibleItemsCount;
                return new Size(availableSize.Width, _maxItemHeight);
            }
        }
        else
        {
            return new Size(0, 0);
        }
    }

    /// <inheritdoc/>
    protected override Size ArrangeOverride(Size finalSize)
    {
        double x = 0;

        // Check if there's more (little) width available - if so, set max item width to the maximum possible as we have an almost perfect height.
        if (finalSize.Width > _visibleItemsCount * _maxItemWidth + (Spacing * (_visibleItemsCount - 1)))
        {
            _maxItemWidth = (finalSize.Width - (Spacing * (_visibleItemsCount - 1))) / _visibleItemsCount;
        }

        var elements = Children.Where(static e => e.Visibility == Visibility.Visible);
        foreach (var child in elements)
        {
            child.Arrange(new Rect(x, 0, _maxItemWidth, _maxItemHeight));
            x += _maxItemWidth + Spacing;
        }
        return finalSize;
    }

    private void OnHorizontalAlignmentChanged(DependencyObject sender, DependencyProperty dp)
    {
        InvalidateMeasure();
    }

    private static void OnSpacingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var panel = (EqualPanel)d;
        panel.InvalidateMeasure();
    }
}
