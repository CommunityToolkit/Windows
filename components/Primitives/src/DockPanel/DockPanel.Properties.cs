// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace CommunityToolkit.WinUI.Controls;

/// <summary>
/// Defines an area where you can arrange child elements either horizontally or vertically, relative to each other.
/// </summary>
public partial class DockPanel
{
    /// <summary>
    /// Gets or sets a value that indicates the position of a child element within a parent <see cref="DockPanel"/>.
    /// </summary>
    public static readonly DependencyProperty DockProperty = DependencyProperty.RegisterAttached(
        nameof(Dock),
        typeof(Dock),
        typeof(FrameworkElement),
        new PropertyMetadata(Dock.Left, DockChanged));

    /// <summary>
    /// Gets DockProperty attached property
    /// </summary>
    /// <param name="obj">Target FrameworkElement</param>
    /// <returns>Dock value</returns>
    public static Dock GetDock(FrameworkElement obj)
    {
        return (Dock)obj.GetValue(DockProperty);
    }

    /// <summary>
    /// Sets DockProperty attached property
    /// </summary>
    /// <param name="obj">Target FrameworkElement</param>
    /// <param name="value">Dock Value</param>
    public static void SetDock(FrameworkElement obj, Dock value)
    {
        obj.SetValue(DockProperty, value);
    }

    /// <summary>
    /// Identifies the <see cref="LastChildFill"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty LastChildFillProperty
        = DependencyProperty.Register(
            nameof(LastChildFill),
            typeof(bool),
            typeof(DockPanel),
            new PropertyMetadata(true, LastChildFillChanged));

    /// <summary>
    /// Gets or sets a value indicating whether the last child element within a DockPanel stretches to fill the remaining available space.
    /// </summary>
    public bool LastChildFill
    {
        get => (bool)GetValue(LastChildFillProperty);
        set => SetValue(LastChildFillProperty, value);
    }

    /// <summary>
    /// Identifies the Padding dependency property.
    /// </summary>
    /// <returns>The identifier for the <see cref="Padding"/> dependency property.</returns>
    public static readonly DependencyProperty PaddingProperty =
        DependencyProperty.Register(
            nameof(Padding),
            typeof(Thickness),
            typeof(DockPanel),
            new PropertyMetadata(default(Thickness), OnPaddingChanged));

    /// <summary>
    /// Gets or sets the distance between the border and its child object.
    /// </summary>
    /// <returns>
    /// The dimensions of the space between the border and its child as a Thickness value.
    /// Thickness is a structure that stores dimension values using pixel measures.
    /// </returns>
    public Thickness Padding
    {
        get => (Thickness)GetValue(PaddingProperty); 
        set => SetValue(PaddingProperty, value); 
    }

    /// <summary>
    /// Identifies the HorizontalSpacing dependency property.
    /// </summary>
    /// <returns>The identifier for the <see cref="HorizontalSpacing"/> dependency property.</returns>
    public static readonly DependencyProperty HorizontalSpacingProperty
        = DependencyProperty.Register(
            nameof(HorizontalSpacing), 
            typeof(double), 
            typeof(DockPanel), 
            new PropertyMetadata(0d, OnPropertyChanged));

    /// <summary>
    /// Gets or sets the horizontal distance between the child objects.
    /// </summary>
    public double HorizontalSpacing
    {
        get => (double)GetValue(HorizontalSpacingProperty); 
        set => SetValue(HorizontalSpacingProperty, value);
    }

    /// <summary>
    /// Identifies the VerticalSpacing dependency property.
    /// </summary>
    /// <returns>The identifier for the <see cref="VerticalSpacing"/> dependency property.</returns>
    public static readonly DependencyProperty VerticalSpacingProperty
        = DependencyProperty.Register(
            nameof(VerticalSpacing), 
            typeof(double), 
            typeof(DockPanel), 
            new PropertyMetadata(default(double), OnPropertyChanged));

    /// <summary>
    /// Gets or sets the vertical distance between the child objects.
    /// </summary>
    public double VerticalSpacing
    {
        get => (double)GetValue(VerticalSpacingProperty); 
        set => SetValue(VerticalSpacingProperty, value);
    }
}
