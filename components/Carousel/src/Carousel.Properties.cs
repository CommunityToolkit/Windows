// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Numerics;

namespace CommunityToolkit.WinUI.Controls;

public partial class Carousel
{
    /// <summary>
    /// The backing <see cref="DependencyProperty"/> for the <see cref="TransitionDuration"/> property.
    /// </summary>
    public static readonly DependencyProperty OrientationProperty =
        DependencyProperty.Register(nameof(Orientation), typeof(Orientation), typeof(Carousel), new PropertyMetadata(Orientation.Horizontal));

    /// <summary>
    /// The backing <see cref="DependencyProperty"/> for the <see cref="SelectedIndex"/> property.
    /// </summary>
    public static readonly DependencyProperty SelectedIndexProperty =
        DependencyProperty.Register(nameof(SelectedIndex), typeof(int), typeof(Carousel), new PropertyMetadata(-1, OnCarouselSelectionPropertyChanged));

    /// <summary>
    /// The backing <see cref="DependencyProperty"/> for the <see cref="SelectedItem"/> property.
    /// </summary>
    public static readonly DependencyProperty SelectedItemProperty =
        DependencyProperty.Register(nameof(SelectedItem), typeof(object), typeof(Carousel), new PropertyMetadata(null, OnCarouselSelectionPropertyChanged));

    /// <summary>
    /// The backing <see cref="DependencyProperty"/> for the <see cref="TransitionDuration"/> property.
    /// </summary>
    public static readonly DependencyProperty ItemSpacingProperty =
        DependencyProperty.Register(nameof(ItemSpacing), typeof(double), typeof(Carousel), new PropertyMetadata(0));

    /// <summary>
    /// The backing <see cref="DependencyProperty"/> for the <see cref="TransitionDuration"/> property.
    /// </summary>
    public static readonly DependencyProperty TransitionDurationProperty =
        DependencyProperty.Register(nameof(TransitionDuration), typeof(TimeSpan), typeof(Carousel), new PropertyMetadata(TimeSpan.FromMilliseconds(200)));

    /// <summary>
    /// The backing <see cref="DependencyProperty"/> for the <see cref="TransitionDuration"/> property.
    /// </summary>
    public static readonly DependencyProperty EasingFunctionProperty =
        DependencyProperty.Register(nameof(TransitionDuration), typeof(EasingFunctionBase), typeof(Carousel), new PropertyMetadata(null));

    /// <summary>
    /// The backing <see cref="DependencyProperty"/> for the <see cref="TransitionDuration"/> property.
    /// </summary>
    public static readonly DependencyProperty ItemDepthProperty =
        DependencyProperty.Register(nameof(ItemDepth), typeof(double), typeof(Carousel), new PropertyMetadata(0));

    /// <summary>
    /// The backing <see cref="DependencyProperty"/> for the <see cref="TransitionDuration"/> property.
    /// </summary>
    public static readonly DependencyProperty ItemRotationProperty =
        DependencyProperty.Register(nameof(ItemRotation), typeof(double), typeof(Carousel), new PropertyMetadata(Vector3.Zero));

    /// <summary>
    /// The backing <see cref="DependencyProperty"/> for the <see cref="TransitionDuration"/> property.
    /// </summary>
    public static readonly DependencyProperty InvertPositiveProperty =
        DependencyProperty.Register(nameof(InvertPositive), typeof(bool), typeof(Carousel), new PropertyMetadata(true));

    /// <summary>
    /// Gets or sets the orientation to render items.
    /// </summary>
    public Orientation Orientation
    {
        get => (Orientation)GetValue(OrientationProperty);
        set => SetValue(OrientationProperty, value);
    }

    /// <summary>
    /// Gets or sets the index of the selected item.
    /// </summary>
    public int SelectedIndex
    {
        get => (int)GetValue(SelectedIndexProperty);
        set => SetValue(SelectedIndexProperty, value);
    }

    /// <summary>
    /// Gets or sets the selected item.
    /// </summary>
    public object? SelectedItem
    {
        get => (object?)GetValue(SelectedItemProperty);
        set => SetValue(SelectedItemProperty, value);
    }

    /// <summary>
    /// Gets or sets the selected item.
    /// </summary>
    public double? ItemSpacing
    {
        get => (double?)GetValue(ItemSpacingProperty);
        set => SetValue(ItemSpacingProperty, value);
    }

    /// <summary>
    /// Gets or sets the transition animation duration.
    /// </summary>
    public TimeSpan TransitionDuration
    {
        get => (TimeSpan)GetValue(TransitionDurationProperty);
        set => SetValue(TransitionDurationProperty, value);
    }

    /// <summary>
    /// Gets or sets the transition animation duration.
    /// </summary>
    public EasingFunctionBase? EasingFunction
    {
        get => (EasingFunctionBase?)GetValue(EasingFunctionProperty);
        set => SetValue(EasingFunctionProperty, value);
    }

    /// <summary>
    /// Gets or sets the depth for unselected items.
    /// </summary>
    public double ItemDepth
    {
        get => (double)GetValue(TransitionDurationProperty);
        set => SetValue(TransitionDurationProperty, value);
    }

    /// <summary>
    /// Gets or sets the rotation to apply to carousel items.
    /// </summary>
    public Vector3 ItemRotation
    {
        get => (Vector3)GetValue(ItemRotationProperty);
        set => SetValue(ItemRotationProperty, value);
    }

    /// <summary>
    /// Gets or sets the rotation to apply to carousel items.
    /// </summary>
    public bool InvertPositive
    {
        get => (bool)GetValue(InvertPositiveProperty);
        set => SetValue(InvertPositiveProperty, value);
    }

    private static void OnCarouselSelectionPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not Carousel carousel)
            return;
    }
}
