// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace CommunityToolkit.WinUI.Controls;

/// <summary>
/// A converter that returns a margin based on the position of the item in a segmented control.
/// </summary>
public partial class SegmentedMarginConverter : DependencyObject, IValueConverter
{
    /// <summary>
    /// Identifies the <see cref="LeftItemMargin"/> property.
    /// </summary>
    public static readonly DependencyProperty LeftItemMarginProperty =
        DependencyProperty.Register(nameof(LeftItemMargin), typeof(Thickness), typeof(SegmentedMarginConverter), new PropertyMetadata(null));

    /// <summary>
    /// Gets or sets the margin of the first item
    /// </summary>
    public Thickness LeftItemMargin
    {
        get { return (Thickness)GetValue(LeftItemMarginProperty); }
        set { SetValue(LeftItemMarginProperty, value); }
    }

    /// <summary>
    /// Identifies the <see cref="MiddleItemMargin"/> property.
    /// </summary>
    public static readonly DependencyProperty MiddleItemMarginProperty =
        DependencyProperty.Register(nameof(MiddleItemMargin), typeof(Thickness), typeof(SegmentedMarginConverter), new PropertyMetadata(null));

    /// <summary>
    /// Gets or sets the margin of any middle item
    /// </summary>
    public Thickness MiddleItemMargin
    {
        get { return (Thickness)GetValue(MiddleItemMarginProperty); }
        set { SetValue(MiddleItemMarginProperty, value); }
    }

    /// <summary>
    /// Identifies the <see cref="RightItemMargin"/> property.
    /// </summary>
    public static readonly DependencyProperty RightItemMarginProperty =
        DependencyProperty.Register(nameof(RightItemMargin), typeof(Thickness), typeof(SegmentedMarginConverter), new PropertyMetadata(null));

    /// <summary>
    /// Gets or sets the margin of the last item
    /// </summary>
    public Thickness RightItemMargin
    {
        get { return (Thickness)GetValue(RightItemMarginProperty); }
        set { SetValue(RightItemMarginProperty, value); }
    }

    /// <inheritdoc/>
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        var segmentedItem = value as SegmentedItem;
        var listView = ItemsControl.ItemsControlFromItemContainer(segmentedItem);

        int index = listView.IndexFromContainer(segmentedItem);

        if (index == 0)
        {
            return LeftItemMargin;
        }
        else if (index == listView.Items.Count - 1)
        {
            return RightItemMargin;
        }
        else
        {
            return MiddleItemMargin;
        }
    }

    /// <inheritdoc/>
    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        return value;
    }
}
