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
        new PropertyMetadata(default(double), OnPropertyChanged));

    /// <summary>
    /// Backing <see cref="DependencyProperty"/> for the <see cref="Orientation"/> property.
    /// </summary>
    public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(
        nameof(Orientation),
        typeof(Orientation),
        typeof(EqualPanel),
        new PropertyMetadata(default(Orientation), OnPropertyChanged));

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

        // Define XY coords
        double xSize = 0, ySize = 0;

        // Define UV coords for orientation agnostic XY manipulation
        var uvSize = new UVCoord(ref xSize, ref ySize, Orientation);
        var maxItemSize = new UVCoord(ref _maxItemWidth, ref _maxItemHeight, Orientation);
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

        return new Size(xSize, ySize);
    }

    /// <inheritdoc/>
    protected override Size ArrangeOverride(Size finalSize)
    {
        // Define X and Y
        double x = 0;
        double y = 0;

        // Define UV axis
        var pos = new UVCoord(ref x, ref y, Orientation);
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
            child.Arrange(new Rect(x, y, _maxItemWidth, _maxItemHeight));
            pos.U += maxItemU + Spacing;
        }
        return finalSize;
    }

    private void OnAlignmentChanged(DependencyObject sender, DependencyProperty dp)
    {
        InvalidateMeasure();
    }

    private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var panel = (EqualPanel)d;
        panel.InvalidateMeasure();
    }

    /// <summary>
    /// A struct for mapping X/Y coordinates to an orientation adjusted U/V coordinate system.
    /// </summary>
    private readonly ref struct UVCoord
    {
        private readonly ref double _u;
        private readonly ref double _v;

        public UVCoord(ref double x, ref double y, Orientation orientation)
        {
            if (orientation is Orientation.Horizontal)
            {
                _u = ref x;
                _v = ref y;
            }
            else
            {
                _u = ref y;
                _v = ref x;
            }
        }

        public readonly ref double U => ref _u;

        public readonly ref double V => ref _v;
    }
}
