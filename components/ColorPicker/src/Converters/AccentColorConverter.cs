// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.WinUI.Helpers;
using System.Globalization;
using Windows.UI;

namespace CommunityToolkit.WinUI.Controls;

/// <summary>
/// Creates an accent color for a base color value.
/// </summary>
public partial class AccentColorConverter : IValueConverter
{
    /// <summary>
    /// The amount to change the Value channel for each accent color step.
    /// </summary>
    public const double ValueDelta = 0.1;

    /// <summary>
    /// This does not account for perceptual differences and also does not match with
    /// system accent color calculation.
    /// </summary>
    /// <remarks>
    /// Use the HSV representation as it's more perceptual.
    /// In most cases only the value is changed by a fixed percentage so the algorithm is reproducible.
    /// </remarks>
    /// <param name="hsvColor">The base color to calculate the accent from.</param>
    /// <param name="accentStep">The number of accent color steps to move.</param>
    /// <returns>The new accent color.</returns>
    public static HsvColor GetAccent(HsvColor hsvColor, int accentStep)
    {
        if (accentStep != 0)
        {
            double colorValue = hsvColor.Value;
            colorValue += accentStep * AccentColorConverter.ValueDelta;
            colorValue = Math.Round(colorValue, 2);
            hsvColor.Value = colorValue;
        }

        return hsvColor;
    }

    /// <inheritdoc/>
    public object Convert(
        object value,
        Type targetType,
        object parameter,
        string language)
    {
        int accentStep;
        Color? rgbColor = null;
        HsvColor? hsvColor = null;

        // Get the current color in HSV
        switch (value)
        {
            case Color valueColor:
                rgbColor = valueColor;
                break;
            case HsvColor valueHsvColor:
                hsvColor = valueHsvColor;
                break;
            case SolidColorBrush valueBrush:
                rgbColor = valueBrush.Color;
                break;
            default:
                // Invalid color value provided
                return DependencyProperty.UnsetValue;
        }

        // Get the value component delta
        try
        {
            accentStep = int.Parse(parameter?.ToString()!, CultureInfo.InvariantCulture);
        }
        catch
        {
            // Invalid parameter provided, unable to convert to integer
            return DependencyProperty.UnsetValue;
        }

        if (hsvColor == null &&
            rgbColor != null)
        {
            hsvColor = rgbColor.Value.ToHsv();
        }

        if (hsvColor != null)
        {
            return (Color)GetAccent(hsvColor.Value, accentStep);
        }
        else
        {
            return DependencyProperty.UnsetValue;
        }
    }

    /// <inheritdoc/>
    public object ConvertBack(
        object value,
        Type targetType,
        object parameter,
        string language)
    {
        return DependencyProperty.UnsetValue;
    }
}
