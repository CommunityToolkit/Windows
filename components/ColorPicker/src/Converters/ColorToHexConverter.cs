// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Windows.UI;

using ColorHelper = CommunityToolkit.WinUI.Helpers.ColorHelper;

namespace CommunityToolkit.WinUI.Controls;

/// <summary>
/// Converts a color to a hex string and vice versa.
/// </summary>
public partial class ColorToHexConverter : IValueConverter
{
    /// <inheritdoc/>
    public object Convert(
        object value,
        Type targetType,
        object parameter,
        string language)
    {
        Color color;

        switch (value)
        {
            case Color valueColor:
                color = valueColor;
                break;
            case SolidColorBrush valueBrush:
                color = valueBrush.Color;
                break;
            default:
                // Invalid color value provided
                return DependencyProperty.UnsetValue;
        }

        return color.ToString().TrimStart('#');
    }

    /// <inheritdoc/>
    public object ConvertBack(
        object value,
        Type targetType,
        object? parameter,
        string? language)
    {
        string hexValue = value.ToString()!;

        if (!hexValue.StartsWith('#'))
            hexValue = $"#{hexValue}";

        if (ColorHelper.TryParseHexColor(hexValue, out var color))
            return color;

        // Invalid hex color value provided
        return DependencyProperty.UnsetValue;
    }
}
