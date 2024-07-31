// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Windows.UI;

namespace CommunityToolkit.WinUI.Converters;

/// <summary>
/// Gets the approximated display name for the color.
/// </summary>
public partial class ColorToDisplayNameConverter : IValueConverter
{
    /// <inheritdoc/>
    public object Convert(
        object value,
        Type targetType,
        object parameter,
        string language)
    {
        Color color;

        if (value is Color valueColor)
        {
            color = valueColor;
        }
        else if (value is SolidColorBrush valueBrush)
        {
            color = valueBrush.Color;
        }
        else
        {
            // Invalid color value provided
            return DependencyProperty.UnsetValue;
        }

#if !WINAPPSDK && !HAS_UNO
        return Windows.UI.ColorHelper.ToDisplayName(color);
#else
        // ToDisplayName not yet supported on WASDK. See https://github.com/microsoft/microsoft-ui-xaml/issues/8287
        return "Not supported";
#endif
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
