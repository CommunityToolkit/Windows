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
    /// Identifies the Spacing dependency property.
    /// </summary>
    /// <returns>The identifier for the <see cref="Spacing"/> dependency property.</returns>
    public static readonly DependencyProperty SpacingProperty = DependencyProperty.Register(
        nameof(Spacing),
        typeof(double),
        typeof(EqualPanel),
        new PropertyMetadata(default(double), OnEqualPanelPropertyChanged));

    /// <summary>
    /// Backing <see cref="DependencyProperty"/> for the <see cref="Orientation"/> property.
    /// </summary>
    public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(
        nameof(Orientation),
        typeof(Orientation),
        typeof(EqualPanel),
        new PropertyMetadata(default(Orientation), OnEqualPanelPropertyChanged));

    /// <summary>
    /// Gets or sets the spacing between items.
    /// </summary>
    public double Spacing
    {
        get => (double)GetValue(SpacingProperty);
        set => SetValue(SpacingProperty, value);
    }

    /// <summary>
    /// Gets or sets the panel orientation.
    /// </summary>
    public Orientation Orientation
    {
        get => (Orientation)GetValue(OrientationProperty);
        set => SetValue(OrientationProperty, value);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="EqualPanel"/> class.
    /// </summary>
    public EqualPanel()
    {
        RegisterPropertyChangedCallback(HorizontalAlignmentProperty, OnAlignmentChanged);
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

        // No children, no space taken
        if (_visibleItemsCount <= 0)
            return new Size(0, 0);

        // Determine if the desired alignment is stretched.
        // Don't stretch if infinite space is available though. Attempting to divide infinite space will result in a crash.
        bool stretch = Orientation switch
        {
            Orientation.Horizontal => HorizontalAlignment is HorizontalAlignment.Stretch && !double.IsInfinity(availableSize.Width),
            Orientation.Vertical or _ => VerticalAlignment is VerticalAlignment.Stretch && !double.IsInfinity(availableSize.Height),
        };

        // Define UV coords for orientation agnostic XY manipulation
        var uvSize = new UVCoord(0, 0, Orientation);
        var maxItemSize = new UVCoord(_maxItemWidth, _maxItemHeight, Orientation);
        double availableU = Orientation is Orientation.Horizontal ? availableSize.Width : availableSize.Height;

        if (stretch)
        {
            // Adjust maxItemU to form equal rows/columns by available U space (adjust for spacing)
            double totalU = availableU - (Spacing * (_visibleItemsCount - 1));
            maxItemSize.U = totalU / _visibleItemsCount;

            // Set uSize/vSize for XY result construction
            uvSize.U = availableU;
            uvSize.V = maxItemSize.V;
        }
        else
        {
            uvSize.U = (maxItemSize.U * _visibleItemsCount) + (Spacing * (_visibleItemsCount - 1));
            uvSize.V = maxItemSize.V;
        }

        return new Size(uvSize.X, uvSize.Y);
    }

    /// <inheritdoc/>
    protected override Size ArrangeOverride(Size finalSize)
    {
        // Define UV axis
        var pos = new UVCoord(0, 0, Orientation);
        ref double maxItemU = ref _maxItemWidth;
        double finalSizeU = finalSize.Width;
        if (Orientation is Orientation.Vertical)
        {
            maxItemU = ref _maxItemHeight;
            finalSizeU = finalSize.Height;
        }
        
        // Check if there's more (little) width available - if so, set max item width to the maximum possible as we have an almost perfect height.
        if (finalSizeU > _visibleItemsCount * maxItemU + (Spacing * (_visibleItemsCount - 1)))
        {
            maxItemU = (finalSizeU - (Spacing * (_visibleItemsCount - 1))) / _visibleItemsCount;
        }

        var elements = Children.Where(static e => e.Visibility == Visibility.Visible);
        foreach (var child in elements)
        {
            // NOTE: The arrange method is still in X/Y coordinate system
            child.Arrange(new Rect(pos.X, pos.Y, _maxItemWidth, _maxItemHeight));
            pos.U += maxItemU + Spacing;
        }
        return finalSize;
    }

    private void OnAlignmentChanged(DependencyObject sender, DependencyProperty dp)
    {
        InvalidateMeasure();
    }

    private static void OnEqualPanelPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var panel = (EqualPanel)d;
        panel.InvalidateMeasure();
    }

    /// <summary>
    /// A struct for mapping X/Y coordinates to an orientation adjusted U/V coordinate system.
    /// </summary>
    private struct UVCoord(double x, double y, Orientation orientation)
    {
        private readonly bool _horizontal = orientation is Orientation.Horizontal;

        public UVCoord(Size size, Orientation orientation) : this(size.Width, size.Height, orientation)
        {
        }

        public double X { get; set; } = x;

        public double Y { get; set; } = y;

        public double U
        {
            readonly get => _horizontal ? X : Y;
            set
            {
                if (_horizontal)
                {
                    X = value;
                }
                else
                {
                    Y = value;
                }
            }
        }

        public double V
        {
            readonly get => _horizontal ? Y : X;
            set
            {
                if (_horizontal)
                {
                    Y = value;
                }
                else
                {
                    X = value;
                }
            }
        }

        public readonly Size Size => new(X, Y);
    }
}
