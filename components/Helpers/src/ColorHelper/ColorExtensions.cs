// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Windows.UI;

namespace CommunityToolkit.WinUI.Helpers;

/// <summary>
/// A class containing extensions methods for <see cref="Color"/>
/// </summary>
public static class ColorExtensions
{
    /// <summary>
    /// Converts a <see cref="Color"/> to a premultiplied Int32 - 4 byte ARGB structure.
    /// </summary>
    /// <param name="color">The color to convert.</param>
    /// <returns>The int representation of the color.</returns>
    public static int ToInt(this Color color)
    {
        var a = color.A + 1;
        var col = (color.A << 24) | ((byte)((color.R * a) >> 8) << 16) | ((byte)((color.G * a) >> 8) << 8) | (byte)((color.B * a) >> 8);
        return col;
    }

    /// <summary>
    /// Converts a <see cref="Color"/> to an <see cref="HslColor"/>.
    /// </summary>
    /// <param name="color">The <see cref="Color"/> to convert.</param>
    /// <returns>The converted <see cref="HslColor"/>.</returns>
    public static HslColor ToHsl(this Color color) => (HslColor)color;

    /// <summary>
    /// Converts a <see cref="Color"/> to an <see cref="HsvColor"/>.
    /// </summary>
    /// <param name="color">The <see cref="Color"/> to convert.</param>
    /// <returns>The converted <see cref="HsvColor"/>.</returns>
    public static HsvColor ToHsv(this Color color) => (HsvColor)color;

#if NET10_0_OR_GREATER

    extension(Color color)
    {
        /// <summary>
        /// Gets a <see cref="HsvColor"/> from alpha, hue, saturation, and lightness channel info.
        /// </summary>
        /// <remarks>
        /// This returns a <see cref="HslColor"/> in order to avoid unneccesary conversions.
        /// However, the <see cref="HslColor"/> will be implicitly cast to a <see cref="Color"/> if needed.
        /// </remarks>
        /// <param name="h">The color's hue.</param>
        /// <param name="s">The color's saturation.</param>
        /// <param name="l">The color's lightness.</param>
        /// <param name="a">The color's alpha value.</param>
        /// <returns>The color as a <see cref="HslColor"/>.</returns>
        public static HslColor FromAhsl(double a, double h, double s, double l) => HslColor.Create(h, s, l, a);

        /// <summary>
        /// Gets a <see cref="HsvColor"/> from alpha, hue, saturation, and value channel info.
        /// </summary>
        /// <remarks>
        /// This returns a <see cref="HsvColor"/> in order to avoid unneccesary conversions.
        /// However, the <see cref="HsvColor"/> will be implicitly cast to a <see cref="Color"/> if needed.
        /// </remarks>
        /// <param name="h">The color's hue.</param>
        /// <param name="s">The color's saturation.</param>
        /// <param name="v">The color's value.</param>
        /// <param name="a">The color's alpha value.</param>
        /// <returns>The color as a <see cref="HsvColor"/>.</returns>
        public static HsvColor FromAhsv(double a, double h, double s, double v) => HsvColor.Create(h, s, v, a);
    }

#endif
}
