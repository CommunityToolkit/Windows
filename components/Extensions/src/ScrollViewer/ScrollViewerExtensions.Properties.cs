// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace CommunityToolkit.WinUI;

/// <summary>
/// Provides attached dependency properties and methods for the <see cref="ScrollViewer"/> control.
/// </summary>
public partial class ScrollViewerExtensions
{
    /// <summary>
    /// Attached <see cref="DependencyProperty"/> for binding a <see cref="Thickness"/> for the horizontal <see cref="ScrollBar"/> of a <see cref="ScrollViewer"/>
    /// </summary>
    public static readonly DependencyProperty HorizontalScrollBarMarginProperty = DependencyProperty.RegisterAttached("HorizontalScrollBarMargin", typeof(Thickness), typeof(ScrollViewerExtensions), new PropertyMetadata(null, OnHorizontalScrollBarMarginPropertyChanged));

    /// <summary>
    /// Attached <see cref="DependencyProperty"/> for binding a <see cref="Thickness"/> for the vertical <see cref="ScrollBar"/> of a <see cref="ScrollViewer"/>
    /// </summary>
    public static readonly DependencyProperty VerticalScrollBarMarginProperty = DependencyProperty.RegisterAttached("VerticalScrollBarMargin", typeof(Thickness), typeof(ScrollViewerExtensions), new PropertyMetadata(null, OnVerticalScrollBarMarginPropertyChanged));

    /// <summary>
    /// Gets the <see cref="Thickness"/> associated with the specified vertical <see cref="ScrollBar"/> of a <see cref="ScrollViewer"/>
    /// </summary>
    /// <param name="obj">The <see cref="FrameworkElement"/> to get the associated <see cref="Thickness"/> from</param>
    /// <returns>The <see cref="Thickness"/> associated with the <see cref="FrameworkElement"/></returns>
    public static Thickness GetVerticalScrollBarMargin(FrameworkElement obj)
    {
        return (Thickness)obj.GetValue(VerticalScrollBarMarginProperty);
    }

    /// <summary>
    /// Sets the <see cref="Thickness"/> associated with the specified vertical <see cref="ScrollBar"/> of a <see cref="ScrollViewer"/>
    /// </summary>
    /// <param name="obj">The <see cref="FrameworkElement"/> to associate the <see cref="Thickness"/> with</param>
    /// <param name="value">The <see cref="Thickness"/> for binding to the <see cref="FrameworkElement"/></param>
    public static void SetVerticalScrollBarMargin(FrameworkElement obj, Thickness value)
    {
        obj.SetValue(VerticalScrollBarMarginProperty, value);
    }

    /// <summary>
    /// Gets the <see cref="Thickness"/> associated with the specified horizontal <see cref="ScrollBar"/> of a <see cref="ScrollViewer"/>
    /// </summary>
    /// <param name="obj">The <see cref="FrameworkElement"/> to get the associated <see cref="Thickness"/> from</param>
    /// <returns>The <see cref="Thickness"/> associated with the <see cref="FrameworkElement"/></returns>
    public static Thickness GetHorizontalScrollBarMargin(FrameworkElement obj)
    {
        return (Thickness)obj.GetValue(HorizontalScrollBarMarginProperty);
    }

    /// <summary>
    /// Sets the <see cref="Thickness"/> associated with the specified horizontal <see cref="ScrollBar"/> of a <see cref="ScrollViewer"/>
    /// </summary>
    /// <param name="obj">The <see cref="FrameworkElement"/> to associate the <see cref="Thickness"/> with</param>
    /// <param name="value">The <see cref="Thickness"/> for binding to the <see cref="FrameworkElement"/></param>
    public static void SetHorizontalScrollBarMargin(FrameworkElement obj, Thickness value)
    {
        obj.SetValue(HorizontalScrollBarMarginProperty, value);
    }
}
