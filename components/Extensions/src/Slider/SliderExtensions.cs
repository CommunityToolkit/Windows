// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace CommunityToolkit.WinUI;

/// <summary>
/// Provides attached dependency properties for the <see cref="Slider"/> control.
/// </summary>
public static class SliderExtensions
{
    /// <summary>
    /// Attached <see cref="DependencyProperty"/> that, when <c>true</c>, lets the
    /// target <see cref="Slider"/> respond to mouse-wheel input by adjusting its
    /// <see cref="RangeBase.Value"/>. Each notch changes the value by
    /// <see cref="MouseWheelChangeProperty"/>; the wheel event is marked handled so
    /// an enclosing <see cref="ScrollViewer"/> does not also scroll.
    /// </summary>
    public static readonly DependencyProperty IsMouseWheelEnabledProperty = DependencyProperty.RegisterAttached(
        nameof(GetIsMouseWheelEnabled)[3..],
        typeof(bool),
        typeof(SliderExtensions),
        new PropertyMetadata(false, OnIsMouseWheelEnabledChanged));

    /// <summary>
    /// Attached <see cref="DependencyProperty"/> for the value added to or subtracted
    /// from <see cref="RangeBase.Value"/> per mouse-wheel notch. Defaults to
    /// <see cref="double.NaN"/>, which means "use the slider's own
    /// <see cref="RangeBase.SmallChange"/>".
    /// </summary>
    public static readonly DependencyProperty MouseWheelChangeProperty = DependencyProperty.RegisterAttached(
        nameof(GetMouseWheelChange)[3..],
        typeof(double),
        typeof(SliderExtensions),
        new PropertyMetadata(double.NaN));

    /// <summary>
    /// Gets the value of <see cref="IsMouseWheelEnabledProperty"/>.
    /// </summary>
    /// <param name="obj">The <see cref="Slider"/> to read the property value from.</param>
    /// <returns><c>true</c> if mouse-wheel scrolling is enabled on the slider; otherwise <c>false</c>.</returns>
    public static bool GetIsMouseWheelEnabled(Slider obj) => (bool)obj.GetValue(IsMouseWheelEnabledProperty);

    /// <summary>
    /// Sets the value of <see cref="IsMouseWheelEnabledProperty"/>.
    /// </summary>
    /// <param name="obj">The <see cref="Slider"/> to set the property on.</param>
    /// <param name="value"><c>true</c> to enable mouse-wheel scrolling on the slider; otherwise <c>false</c>.</param>
    public static void SetIsMouseWheelEnabled(Slider obj, bool value) => obj.SetValue(IsMouseWheelEnabledProperty, value);

    /// <summary>
    /// Gets the value of <see cref="MouseWheelChangeProperty"/>.
    /// </summary>
    /// <param name="obj">The <see cref="Slider"/> to read the property value from.</param>
    /// <returns>The per-notch delta, or <see cref="double.NaN"/> to inherit from <see cref="RangeBase.SmallChange"/>.</returns>
    public static double GetMouseWheelChange(Slider obj) => (double)obj.GetValue(MouseWheelChangeProperty);

    /// <summary>
    /// Sets the value of <see cref="MouseWheelChangeProperty"/>.
    /// </summary>
    /// <param name="obj">The <see cref="Slider"/> to set the property on.</param>
    /// <param name="value">The per-notch delta to apply to <see cref="RangeBase.Value"/>.</param>
    public static void SetMouseWheelChange(Slider obj, double value) => obj.SetValue(MouseWheelChangeProperty, value);

    private static void OnIsMouseWheelEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not Slider slider)
        {
            return;
        }

        slider.PointerWheelChanged -= OnPointerWheelChanged;

        if (e.NewValue is true)
        {
            slider.PointerWheelChanged += OnPointerWheelChanged;
        }
    }

    private static void OnPointerWheelChanged(object sender, PointerRoutedEventArgs e)
    {
        if (sender is not Slider slider)
        {
            return;
        }

        int delta = e.GetCurrentPoint(slider).Properties.MouseWheelDelta;
        if (delta == 0)
        {
            return;
        }

        double step = GetMouseWheelChange(slider);
        if (double.IsNaN(step) || step <= 0)
        {
            step = slider.SmallChange;
        }

        double notches = delta / 120.0;
        double newValue = Math.Clamp(slider.Value + (notches * step), slider.Minimum, slider.Maximum);

        if (newValue != slider.Value)
        {
            slider.Value = newValue;
        }

        // Claim the gesture so an enclosing ScrollViewer does not also scroll.
        e.Handled = true;
    }
}
