// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.UI.Xaml.Controls;
using System.Collections.Specialized;
using Windows.Foundation;
using Microsoft.UI.Xaml;

namespace CommunityToolkit.WinUI.Controls;

/// <summary>
/// Arranges elements by wrapping them to fit the available space.
/// When <see cref="Orientation"/> is set to Orientation.Horizontal, element are arranged in rows until the available width is reached and then to a new row.
/// When <see cref="Orientation"/> is set to Orientation.Vertical, element are arranged in columns until the available height is reached.
/// </summary>
public class WrapLayout : VirtualizingLayout
{
    /// <summary>
    /// Gets or sets a uniform Horizontal distance (in pixels) between items when <see cref="Orientation"/> is set to Horizontal,
    /// or between columns of items when <see cref="Orientation"/> is set to Vertical.
    /// </summary>
    public double HorizontalSpacing
    {
        get => (double)GetValue(HorizontalSpacingProperty);
        set => SetValue(HorizontalSpacingProperty, value);
    }

    /// <summary>
    /// Identifies the <see cref="HorizontalSpacing"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty HorizontalSpacingProperty =
        DependencyProperty.Register(
            nameof(HorizontalSpacing),
            typeof(double),
            typeof(WrapLayout),
            new PropertyMetadata(0d, LayoutPropertyChanged));

    /// <summary>
    /// Gets or sets a uniform Vertical distance (in pixels) between items when <see cref="Orientation"/> is set to Vertical,
    /// or between rows of items when <see cref="Orientation"/> is set to Horizontal.
    /// </summary>
    public double VerticalSpacing
    {
        get => (double)GetValue(VerticalSpacingProperty);
        set => SetValue(VerticalSpacingProperty, value);
    }

    /// <summary>
    /// Identifies the <see cref="VerticalSpacing"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty VerticalSpacingProperty =
        DependencyProperty.Register(
            nameof(VerticalSpacing),
            typeof(double),
            typeof(WrapLayout),
            new PropertyMetadata(0d, LayoutPropertyChanged));

    /// <summary>
    /// Gets or sets the orientation of the WrapLayout.
    /// Horizontal means that child controls will be added horizontally until the width of the panel is reached, then a new row is added to add new child controls.
    /// Vertical means that children will be added vertically until the height of the panel is reached, then a new column is added.
    /// </summary>
    public Orientation Orientation
    {
        get => (Orientation)GetValue(OrientationProperty);
        set => SetValue(OrientationProperty, value);
    }

    /// <summary>
    /// Identifies the <see cref="Orientation"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty OrientationProperty =
        DependencyProperty.Register(
            nameof(Orientation),
            typeof(Orientation),
            typeof(WrapLayout),
            new PropertyMetadata(Orientation.Horizontal, LayoutPropertyChanged));

    private static void LayoutPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is WrapLayout wp)
        {
            wp.InvalidateMeasure();
            wp.InvalidateArrange();
        }
    }

    /// <inheritdoc />
    protected override void InitializeForContextCore(VirtualizingLayoutContext context)
    {
        context.LayoutState = new WrapLayoutState(context);
        base.InitializeForContextCore(context);
    }

    /// <inheritdoc />
    protected override void UninitializeForContextCore(VirtualizingLayoutContext context)
    {
        context.LayoutState = null;
        base.UninitializeForContextCore(context);
    }

    /// <inheritdoc />
    protected override void OnItemsChangedCore(VirtualizingLayoutContext context, object source, NotifyCollectionChangedEventArgs args)
    {
        var state = (WrapLayoutState)context.LayoutState;

        switch (args.Action)
        {
            case NotifyCollectionChangedAction.Add:
                state.RemoveFromIndex(args.NewStartingIndex);
                break;
            case NotifyCollectionChangedAction.Move:
                var minIndex = Math.Min(args.NewStartingIndex, args.OldStartingIndex);
                state.RemoveFromIndex(minIndex);

                state.RecycleElementAt(args.OldStartingIndex);
                state.RecycleElementAt(args.NewStartingIndex);
                break;
            case NotifyCollectionChangedAction.Remove:
                state.RemoveFromIndex(args.OldStartingIndex);
                break;
            case NotifyCollectionChangedAction.Replace:
                state.RemoveFromIndex(args.NewStartingIndex);
                state.RecycleElementAt(args.NewStartingIndex);
                break;
            case NotifyCollectionChangedAction.Reset:
                state.Clear();
                break;
        }

        base.OnItemsChangedCore(context, source, args);
    }

    /// <inheritdoc />
    protected override Size MeasureOverride(VirtualizingLayoutContext context, Size availableSize)
    {
        var parentMeasure = new UvMeasure(Orientation, availableSize);
        var spacingMeasure = new UvMeasure(Orientation, new Size(HorizontalSpacing, VerticalSpacing));

        var state = (WrapLayoutState)context.LayoutState;
        if (state.Orientation != Orientation)
        {
            state.SetOrientation(Orientation);
        }

        if (spacingMeasure != state.Spacing || state.AvailableU != parentMeasure.U)
        {
            state.ClearPositions();
            state.Spacing = spacingMeasure;
            state.AvailableU = parentMeasure.U;
        }

        double currentV = 0;
        var realizationBounds = new UvBounds(Orientation, context.RealizationRect);
        var position = new UvMeasure();
        for (var i = 0; i < context.ItemCount; ++i)
        {
            var measured = false;
            var item = state.GetItemAt(i);
            if (item.Measure is null)
            {
                item.Element = context.GetOrCreateElementAt(i);
                item.Element.Measure(availableSize);
                item.Measure = new UvMeasure(Orientation, item.Element.DesiredSize);
                measured = true;
            }

            var currentMeasure = item.Measure.Value;

            if (item.Position is null)
            {
                if (parentMeasure.U < position.U + currentMeasure.U)
                {
                    // New Row
                    position.U = 0;
                    position.V += currentV + spacingMeasure.V;
                    currentV = 0;
                }

                item.Position = position;
            }

            position = item.Position.Value;

            var vEnd = position.V + currentMeasure.V;
            if (vEnd < realizationBounds.VMin)
            {
                // Item is "above" the bounds
                if (item.Element is not null)
                {
                    context.RecycleElement(item.Element);
                    item.Element = null;
                }

                continue;
            }
            else if (position.V > realizationBounds.VMax)
            {
                // Item is "below" the bounds.
                if (item.Element is not null)
                {
                    context.RecycleElement(item.Element);
                    item.Element = null;
                }

                // We don't need to measure anything below the bounds
                break;
            }
            else if (!measured)
            {
                // Always measure elements that are within the bounds
                item.Element = context.GetOrCreateElementAt(i);
                item.Element.Measure(availableSize);

                currentMeasure = new UvMeasure(Orientation, item.Element.DesiredSize);
                if (currentMeasure != item.Measure)
                {
                    // this item changed size; we need to recalculate layout for everything after this
                    state.RemoveFromIndex(i + 1);
                    item.Measure = currentMeasure;

                    // did the change make it go into the new row?
                    if (parentMeasure.U < position.U + currentMeasure.U)
                    {
                        // New Row
                        position.U = 0;
                        position.V += currentV + spacingMeasure.V;
                        currentV = 0;
                    }

                    item.Position = position;
                }
            }

            position.U += currentMeasure.U + spacingMeasure.U;
            currentV = Math.Max(currentMeasure.V, currentV);
        }

        // update value with the last line
        // if the the last loop is(parentMeasure.U > currentMeasure.U + lineMeasure.U) the total isn't calculated then calculate it
        // if the last loop is (parentMeasure.U > currentMeasure.U) the currentMeasure isn't added to the total so add it here
        // for the last condition it is zeros so adding it will make no difference
        // this way is faster than an if condition in every loop for checking the last item
        // Propagating an infinite size causes a crash. This can happen if the parent is scrollable and infinite in the opposite
        // axis to the panel. Clearing to zero prevents the crash.
        // This is likely an incorrect use of the control by the developer, however we need stability here so setting a default that won't crash.

        var totalMeasure = new UvMeasure
        {
            U = double.IsInfinity(parentMeasure.U) ? 0 : Math.Ceiling(parentMeasure.U),
            V = state.GetHeight()
        };

        return totalMeasure.GetSize(Orientation);
    }

    /// <inheritdoc />
    protected override Size ArrangeOverride(VirtualizingLayoutContext context, Size finalSize)
    {
        if (context.ItemCount > 0)
        {
            var realizationBounds = new UvBounds(Orientation, context.RealizationRect);

            var state = (WrapLayoutState)context.LayoutState;
            bool ArrangeItem(WrapItem item)
            {
                if (item is { Measure: null } or { Position: null })
                {
                    return false;
                }

                var desiredMeasure = item.Measure.Value;

                var position = item.Position.Value;

                if (realizationBounds.VMin <= position.V + desiredMeasure.V && position.V <= realizationBounds.VMax)
                {
                    // place the item
                    var child = context.GetOrCreateElementAt(item.Index);
                    child.Arrange(new Rect(position.GetPoint(Orientation), desiredMeasure.GetSize(Orientation)));
                }
                else if (position.V > realizationBounds.VMax)
                {
                    return false;
                }

                return true;
            }

            for (var i = 0; i < context.ItemCount; ++i)
            {
                if (!ArrangeItem(state.GetItemAt(i)))
                {
                    break;
                }
            }
        }

        return finalSize;
    }
}
