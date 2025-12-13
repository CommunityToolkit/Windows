// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Globalization;

#if WINAPPSDK
using Microsoft.UI;
#else
using Windows.UI;
#endif
using Color = Windows.UI.Color;

namespace CommunityToolkit.WinUI.Helpers;

/// <summary>
/// This class provides static helper methods for colors.
/// </summary>
public static partial class ColorHelper
{
    /// <summary>
    /// Attempts to parse a string as a color.
    /// </summary>
    /// <remarks>
    /// Any format used in XAML should work.
    /// </remarks>
    /// <param name="colorString">The string to parse.</param>
    /// <param name="color">The resulting color.</param>
    /// <returns>Whether or not the parse succeeded.</returns>
    public static bool TryParseColor(string colorString, out Color color)
    {
        color = default;

        if (string.IsNullOrEmpty(colorString))
            return false;

        // Try as hex
        if (TryParseHexColor(colorString, out color))
            return true;

        // Try as screen color
        if (TryParseScreenColor(colorString, out color))
            return true;

        // TODO: Should hsl and hsv be added?

        if (TryParseColorName(colorString, out color))
            return true;

        return false;
    }

    /// <summary>
    /// Attempts to parse a string as a hex color.
    /// </summary>
    /// <param name="hexString">The string to parse.</param>
    /// <param name="color">The resulting color.</param>
    /// <returns>Whether or not the parse succeeded.</returns>
    public static bool TryParseHexColor(string hexString, out Color color)
    {
        color = default;

        // Ensure string begins with '#'
        // and is long enough to not break later in the parser.
        if (string.IsNullOrEmpty(hexString) ||
            hexString.Length < 2 ||
            hexString[0] != '#')
            return false;

        // Convert base 16 string to uint
        if (!uint.TryParse(hexString[1..], NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var cuint))
            return false;

        // Extract 4 bytes
        byte b3 = (byte)(cuint >> 24);
        byte b2 = (byte)((cuint >> 16) & 0xff);
        byte b1 = (byte)((cuint >> 8) & 0xff);
        byte b0 = (byte)(cuint & 0xff);

        // Extract 4 half-bytes
        byte h3 = (byte)(cuint >> 12);
        byte h2 = (byte)((cuint >> 8) & 0xf);
        byte h1 = (byte)((cuint >> 4) & 0xf);
        byte h0 = (byte)(cuint & 0xf);
        h3 = (byte)(h3 << 4 | h3);
        h2 = (byte)(h2 << 4 | h2);
        h1 = (byte)(h1 << 4 | h1);
        h0 = (byte)(h0 << 4 | h0);

        // Select bytes based on string length
        switch (hexString.Length)
        {
            // #AaRrGgBb
            case 9:
                color = Color.FromArgb(b3, b2, b1, b0);
                return true;
            // #RrGgBb
            case 7:
                color = Color.FromArgb(255, b2, b1, b0);
                return true;
            // #ARGB
            case 5:
                color = Color.FromArgb(h3, h2, h1, h0);
                return true;
            // #RGB
            case 4:
                color = Color.FromArgb(255, h2, h1, h0);
                return true;
            // Invalid format
            default:
                return false;
        }
    }

    /// <summary>
    /// Attempts to parse a string as a hsl color.
    /// </summary>
    /// <param name="hslColor">The string to parse.</param>
    /// <param name="color">The resulting color.</param>
    /// <returns>Whether or not the parse succeeded.</returns>
    public static bool TryParseHslColor(string hslColor, out HslColor color)
    {
        color = default;

        if (!MatchArgPattern<double>(hslColor, "hsl", out var args))
            return false;

        if (args.Length != 3)
            return false;

        color = HslColor.Create(args[0], args[1], args[2]);
        return true;
    }
    
    /// <summary>
    /// Attempts to parse a string as a hsv color.
    /// </summary>
    /// <param name="hsvColor">The string to parse.</param>
    /// <param name="color">The resulting color.</param>
    /// <returns>Whether or not the parse succeeded.</returns>
    public static bool TryParseHsvColor(string hsvColor, out HsvColor color)
    {
        color = default;

        if (!MatchArgPattern<double>(hsvColor, "hsv", out var args))
            return false;

        if (args.Length != 3)
            return false;

        color = HsvColor.Create(args[0], args[1], args[2]);
        return true;
    }

    /// <summary>
    /// Attempts to parse a string as a screen color.
    /// </summary>
    /// <param name="screenColor">The string to parse.</param>
    /// <param name="color">The resulting color.</param>
    /// <returns>Whether or not the parse succeeded.</returns>
    public static bool TryParseScreenColor(string screenColor, out Color color)
    {
        color = default;

        // Ensure the string begins with "sc#"
        if (screenColor.Length < 3 || screenColor[0..3] != "sc#")
            return false;

        // Get arguments
        screenColor = screenColor[3..];
        var values = screenColor.Split(',');

        // Parse the arguments from string doubles into bytes
        var args = new byte[values.Length];
        for (int i = 0; i < values.Length; i++)
        {
            if (!double.TryParse(values[i], out var arg))
                return false;

            args[i] = (byte)(arg * 255);
        }

        // Assign channel data based on arguments
        switch (args.Length)
        {
            // sc#Alpha,Red,Green,Blue
            case 4:
                color = Color.FromArgb(args[0], args[1], args[2], args[3]);
                return true;
            // sc#Red,Green,Blue
            case 3:
                color = Color.FromArgb(255, args[0], args[1], args[2]);
                return true;
            // Invalid format
            default:
                return false;
        }
    }

    /// <summary>
    /// Attempts to parse a string color name.
    /// </summary>
    /// <param name="colorName">The color name.</param>
    /// <param name="color">The resulting color.</param>
    /// <returns>Whether or not the parse succeeded.</returns>
    public static bool TryParseColorName(string colorName, out Color color)
    {
        color = default;

#pragma warning disable CS8605 // Unboxing a possibly null value.
        var prop = typeof(Colors).GetTypeInfo().GetDeclaredProperty(colorName);
        if (prop != null)
        {
            color = (Color)prop.GetValue(null);
            return true;
        }
#pragma warning restore CS8605 // Unboxing a possibly null value.

        return false;
    }

    /// <summary>
    /// Creates a <see cref="Color"/> from a XAML color string.
    /// Any format used in XAML should work.
    /// </summary>
    /// <param name="colorString">The XAML color string.</param>
    /// <returns>The created <see cref="Color"/>.</returns>
    public static Color ParseColor(string colorString)
    {
        if (TryParseColor(colorString, out var color))
            return color;

        throw new FormatException($"The string '{colorString}' could not be parsed as a string.");
    }

    /// <summary>
    /// Parses a hexadecimal color string into a <see cref="Color"/>.
    /// </summary>
    /// <param name="hexColor">The hex color string.</param>
    /// <returns>The resulting <see cref="Color"/>.</returns>
    public static Color ParseHexColor(string hexColor)
    {
        if (TryParseHexColor(hexColor, out var color))
            return color;

        throw new FormatException($"The string '{hexColor}' could not be parsed as a hex color.");
    }
    
    /// <summary>
    /// Parses a hsl color string into a <see cref="Color"/>.
    /// </summary>
    /// <param name="hslColor">The hsl color string.</param>
    /// <returns>The resulting <see cref="Color"/>.</returns>
    public static HslColor ParseHslColor(string hslColor)
    {
        if (TryParseHslColor(hslColor, out var color))
            return color;

        throw new FormatException($"The string '{hslColor}' could not be parsed as a hsl color.");
    }
    
    /// <summary>
    /// Parses a hsv color string into a <see cref="Color"/>.
    /// </summary>
    /// <param name="hsvColor">The hsv color string.</param>
    /// <returns>The resulting <see cref="Color"/>.</returns>
    public static HsvColor ParseHsvColor(string hsvColor)
    {
        if (TryParseHsvColor(hsvColor, out var color))
            return color;

        throw new FormatException($"The string '{hsvColor}' could not be parsed as a hsv color.");
    }

    /// <summary>
    /// Parses a screen color string into a <see cref="Color"/>.
    /// </summary>
    /// <param name="screenColor">The screen color string.</param>
    /// <returns>The resulting <see cref="Color"/>.</returns>
    public static Color ParseScreenColor(string screenColor)
    {
        if (TryParseScreenColor(screenColor, out var color))
            return color;

        throw new FormatException($"The string '{screenColor}' is not a valid ScreenColor string");
    }

    /// <summary>
    /// Parses a color by name.
    /// </summary>
    /// <param name="colorName">The color's name.</param>
    /// <returns>The resulting <see cref="Color"/>.</returns>
    /// <exception cref="ArgumentException">Throws if the color name is not recognized.</exception>
    public static Color ParseColorName(string colorName)
    {
        if (TryParseColorName(colorName, out var color))
            return color;

        throw new FormatException($"The name '{colorName}' not a valid color");
    }

    internal static (double h1, double chroma) CalculateHueAndChroma(Color color, out double min, out double max, out double alpha)
    {
        // This code is shared between both the conversion
        // to both HSL and HSV from RGB.

        var r = (double)color.R / 255;
        var g = (double)color.G / 255;
        var b = (double)color.B / 255;
        alpha = (double)color.A / 255;

        max = Math.Max(Math.Max(r, g), b);
        min = Math.Min(Math.Min(r, g), b);
        var chroma = max - min;

        double h1;
        if (chroma == 0)
        {
            // No max
            h1 = 0;
        }
        else if (max == r)
        {
            // Red is max
            h1 = ((g - b) / chroma + 6) % 6;
        }
        else if (max == g)
        {
            // Green is max
            h1 = 2 + ((b - r) / chroma);
        }
        else
        {
            // Blue is max
            h1 = 4 + ((r - g) / chroma);
        }

        return (h1, chroma);
    }

    internal static Color FromHueChroma(double h1, double chroma, double x, double m, double alpha)
    {
        // This code is shared between both the conversion
        // from both HSL and HSV to RGB.

        double r1, g1, b1;
        (r1, g1, b1) = h1 switch
        {
            < 1 => (chroma, x, 0d),
            < 2 => (x, chroma, 0d),
            < 3 => (0d, chroma, x),
            < 4 => (0d, x, chroma),
            < 5 => (x, 0d, chroma),
            _ => (chroma, 0d, x),
        };

        byte r = (byte)(255 * (r1 + m));
        byte g = (byte)(255 * (g1 + m));
        byte b = (byte)(255 * (b1 + m));
        byte a = (byte)(255 * alpha);

        return Color.FromArgb(a, r, g, b);
    }

    /// <summary>
    /// Parses a string to match the argument pattern "<paramref name="funcName"/>(args[0], args[1], ...)"
    /// </summary>
    private static bool MatchArgPattern<T>(string value, string funcName, out T?[] args)
        where T : IParsable<T>
    {
        args = [];

        // Find opening and closing parenthesis
        var argsStart = value.IndexOf('(');
        var argsEnd = value.Length - 1;
        if (argsStart is -1)
            return false;

        // Ensure the string begins with the function name
        if (value[0..argsStart] != funcName)
            return false;

        // Ensure the string ends with ')' 
        if (value[argsEnd] != ')')
            return false;

        // Split to substring between the parenthesis
        value = value[(argsStart + 1)..argsEnd];
        var argStrings = value.Split(',');

        // Parse the args into an array of doubles
        args = new T[argStrings.Length];
        for (var i = 0; i < argStrings.Length; i++)
        {
            if (!T.TryParse(argStrings[i], null, out args[i]))
                return false;
        }

        return true;
    }
}
