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

    /// <inheritdoc cref="AlphaOver(Color, Color, double)"/>
    public static Color AlphaOver(this Color @base, Color overlay)
        => AlphaOver(@base, overlay, (double)overlay.A / 255);

    /// <summary>
    /// Performs an AlphaOverlay between two colors.
    /// </summary>
    /// <param name="base">The base color.</param>
    /// <param name="overlay">The overlay color.</param>
    /// <param name="alpha">The alpha factor.</param>
    /// <returns>The resulting color from alpha overlaying <paramref name="overlay"/> over <paramref name="base"/>.</returns>
    public static Color AlphaOver(this Color @base, Color overlay, double alpha)
    {
        // Find the color's mix, using the overlay's opacity as a factor
        var mix = MixColors(@base, overlay, alpha);

        // Restore the alpha to match the base
        mix.A = @base.A;
        return mix;
    }

    /// <summary>
    /// Gets a color with the same saturation and value, but with an adjusted hue.
    /// </summary>
    /// <param name="original">The original color.</param>
    /// <param name="hue">The new hue.</param>
    /// <returns>A <see cref="HsvColor"/> with a new hue and the same saturation and value.</returns>
    public static HsvColor WithHue(this Color original, double hue)
    {
        var hsv = (HsvColor)original;
        hsv.Hue = hue;
        return hsv;
    }

    /// <summary>
    /// Gets a color with the same hue and value, but with an adjusted saturation.
    /// </summary>
    /// <param name="original">The original color.</param>
    /// <param name="saturation">The new saturation.</param>
    /// <returns>A <see cref="HsvColor"/> with a new saturation and the same hue and value.</returns>
    public static HsvColor WithSaturation(this Color original, double saturation)
    {
        var hsv = (HsvColor)original;
        hsv.Saturation = saturation;
        return hsv;
    }

    /// <summary>
    /// Gets a color with the same hue and saturation, but with an adjusted saturation.
    /// </summary>
    /// <param name="original">The original color.</param>
    /// <param name="value">The new value.</param>
    /// <returns>A <see cref="HsvColor"/> with a new value and the same hue and saturation.</returns>
    public static HsvColor WithValue(this Color original, double value)
    {
        var hsv = (HsvColor)original;
        hsv.Value = value;
        return hsv;
    }

    /// <summary>
    /// Gets a color with the same hue and saturation, but with an adjusted lightness.
    /// </summary>
    /// <param name="original">The original color.</param>
    /// <param name="lightness">The new lightness.</param>
    /// <returns>A <see cref="HsvColor"/> with a new lightness and the same hue and saturation.</returns>
    public static HslColor WithLightness(this Color original, double lightness)
    {
        var hsl = (HslColor)original;
        hsl.Lightness = lightness;
        return hsl;
    }

    private static Color MixColors(Color color1, Color color2, double factor)
    {
        factor = Math.Clamp(factor, 0, 1);

        // Formula for linearly blending a channel
        var invFactor = 1 - factor;
        byte Blend(byte c1, byte c2) => (byte)Math.Round((c1 * factor) + (c2 * invFactor));

        if (factor is 0)
            return color1;

        if (factor is 1)
            return color2;

        // Blend each channel
        var a = Blend(color1.A, color2.A);
        var r = Blend(color1.R, color2.R);
        var g = Blend(color1.G, color2.G);
        var b = Blend(color1.B, color2.B);

        return Color.FromArgb(a, r, g, b);
    }

    extension(Color _)
    {
        /// <inheritdoc cref="ColorHelper.TryParseColor(string, out Color)"/>
        public static bool TryParseColor(string colorString, out Color color) => ColorHelper.TryParseColor(colorString, out color);

        /// <inheritdoc cref="ColorHelper.TryParseHexColor(string, out Color)"/>
        public static bool TryParseHexColor(string hexString, out Color color) => ColorHelper.TryParseHexColor(hexString, out color);

        /// <inheritdoc cref="ColorHelper.TryParseHslColor(string, out HslColor)"/>
        public static bool TryParseHslColor(string hslColor, out HslColor color) => ColorHelper.TryParseHslColor(hslColor, out color);

        /// <inheritdoc cref="ColorHelper.TryParseHsvColor(string, out HsvColor)"/>
        public static bool TryParseHsvColor(string hsvColor, out HsvColor color) => ColorHelper.TryParseHsvColor(hsvColor, out color);

        /// <inheritdoc cref="ColorHelper.TryParseScreenColor(string, out Color)"/>
        public static bool TryParseScreenColor(string screenColor, out Color color) => ColorHelper.TryParseScreenColor(screenColor, out color);

        /// <inheritdoc cref="ColorHelper.TryParseColorName(string, out Color)"/>
        public static bool TryParseColorName(string colorName, out Color color) => ColorHelper.TryParseColorName(colorName, out color);

        /// <inheritdoc cref="ColorHelper.ParseColor(string)"/>
        public static Color ParseColor(string colorString) => ColorHelper.ParseColor(colorString);

        /// <inheritdoc cref="ColorHelper.ParseHexColor(string)"/>
        public static Color ParseHexColor(string hexColor) => ColorHelper.ParseHexColor(hexColor);

        /// <inheritdoc cref="ColorHelper.ParseHslColor(string)"/>
        public static HslColor ParseHslColor(string hslColor) => ColorHelper.ParseHslColor(hslColor);

        /// <inheritdoc cref="ColorHelper.ParseHsvColor(string)"/>
        public static HsvColor ParseHsvColor(string hsvColor) => ColorHelper.ParseHsvColor(hsvColor);

        /// <inheritdoc cref="ColorHelper.ParseScreenColor(string)"/>
        public static Color ParseScreenColor(string screenColor) => ColorHelper.ParseScreenColor(screenColor);

        /// <inheritdoc cref="ColorHelper.ParseColorName(string)"/>
        public static Color ParseColorName(string colorName) => ColorHelper.ParseColorName(colorName);

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

        /// <summary>
        /// Mixes two colors using a factor for deciding how much influence each color has.
        /// </summary>
        /// <param name="color1">The first color.</param>
        /// <param name="color2">The second color.</param>
        /// <param name="factor">The influence of each color, where 0 is entirely <paramref name="color1"/> and 1 is entirely <paramref name="color2"/>.</param>
        /// <returns>The mix of the two colors.</returns>
        public static Color Mix(Color color1, Color color2, double factor)
            => MixColors(color1, color2, factor);

        /// <summary>
        /// Adds two colors.
        /// </summary>
        /// <remarks>
        /// Simple RGB summation, with each channel clamped seperately. Alpha is NOT included, and will always be opaque.
        /// </remarks>
        /// <param name="color1">The first color.</param>
        /// <param name="color2">The second color.</param>
        /// <returns>The sume of the two colors.</returns>
        public static Color Add(Color color1, Color color2)
        {
            static byte ClampedAdd(byte b1, byte b2) => (byte)int.Min(b1 + b2, 255);

            var r = ClampedAdd(color1.R, color2.R);
            var g = ClampedAdd(color1.G, color2.G);
            var b = ClampedAdd(color1.B, color2.B);

            return Color.FromArgb(255, r, g, b);
        }

        /// <summary>
        /// Subtracts a color from another.
        /// </summary>
        /// <remarks>
        /// Simple RGB subtraction, with each channel clamped seperately. Alpha is NOT included, and will always be opaque.
        /// </remarks>
        /// <param name="color1">The first color.</param>
        /// <param name="color2">The second color.</param>
        /// <returns>The sume of the two colors.</returns>
        public static Color Subtract(Color color1, Color color2)
        {
            static byte ClampedSubtract(byte b1, byte b2) => (byte)int.Max(b1 - b2, 0);

            var r = ClampedSubtract(color1.R, color2.R);
            var g = ClampedSubtract(color1.G, color2.G);
            var b = ClampedSubtract(color1.B, color2.B);

            return Color.FromArgb(255, r, g, b);
        }

        /// <inheritdoc cref="Add(Color, Color)"/>
        public static Color operator +(Color color1, Color color2) => Add(color1, color2);

        /// <inheritdoc cref="Subtract(Color, Color)"/>
        public static Color operator -(Color color1, Color color2) => Subtract(color1, color2);
    }
}
