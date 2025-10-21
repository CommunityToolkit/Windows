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
    /// Creates a <see cref="Color"/> from a XAML color string.
    /// Any format used in XAML should work.
    /// </summary>
    /// <param name="colorString">The XAML color string.</param>
    /// <returns>The created <see cref="Color"/>.</returns>
    public static Color ParseColor(string colorString)
    {
        if (string.IsNullOrEmpty(colorString))
        {
            ThrowArgumentException();
        }

        if (colorString[0] == '#')
        {
            switch (colorString.Length)
            {
                case 9:
                {
                    var cuint = Convert.ToUInt32(colorString.Substring(1), 16);
                    var a = (byte)(cuint >> 24);
                    var r = (byte)((cuint >> 16) & 0xff);
                    var g = (byte)((cuint >> 8) & 0xff);
                    var b = (byte)(cuint & 0xff);

                    return Color.FromArgb(a, r, g, b);
                }

                case 7:
                {
                    var cuint = Convert.ToUInt32(colorString.Substring(1), 16);
                    var r = (byte)((cuint >> 16) & 0xff);
                    var g = (byte)((cuint >> 8) & 0xff);
                    var b = (byte)(cuint & 0xff);

                    return Color.FromArgb(255, r, g, b);
                }

                case 5:
                {
                    var cuint = Convert.ToUInt16(colorString.Substring(1), 16);
                    var a = (byte)(cuint >> 12);
                    var r = (byte)((cuint >> 8) & 0xf);
                    var g = (byte)((cuint >> 4) & 0xf);
                    var b = (byte)(cuint & 0xf);
                    a = (byte)(a << 4 | a);
                    r = (byte)(r << 4 | r);
                    g = (byte)(g << 4 | g);
                    b = (byte)(b << 4 | b);

                    return Color.FromArgb(a, r, g, b);
                }

                case 4:
                {
                    var cuint = Convert.ToUInt16(colorString.Substring(1), 16);
                    var r = (byte)((cuint >> 8) & 0xf);
                    var g = (byte)((cuint >> 4) & 0xf);
                    var b = (byte)(cuint & 0xf);
                    r = (byte)(r << 4 | r);
                    g = (byte)(g << 4 | g);
                    b = (byte)(b << 4 | b);

                    return Color.FromArgb(255, r, g, b);
                }

                default: return ThrowFormatException();
            }
        }

        if (colorString.Length > 3 && colorString[0] == 's' && colorString[1] == 'c' && colorString[2] == '#')
        {
            var values = colorString.Split(',');

            if (values.Length == 4)
            {
                var scA = double.Parse(values[0].Substring(3), CultureInfo.InvariantCulture);
                var scR = double.Parse(values[1], CultureInfo.InvariantCulture);
                var scG = double.Parse(values[2], CultureInfo.InvariantCulture);
                var scB = double.Parse(values[3], CultureInfo.InvariantCulture);

                return Color.FromArgb((byte)(scA * 255), (byte)(scR * 255), (byte)(scG * 255), (byte)(scB * 255));
            }

            if (values.Length == 3)
            {
                var scR = double.Parse(values[0].Substring(3), CultureInfo.InvariantCulture);
                var scG = double.Parse(values[1], CultureInfo.InvariantCulture);
                var scB = double.Parse(values[2], CultureInfo.InvariantCulture);

                return Color.FromArgb(255, (byte)(scR * 255), (byte)(scG * 255), (byte)(scB * 255));
            }

            return ThrowFormatException();
        }

        var prop = typeof(Colors).GetTypeInfo().GetDeclaredProperty(colorString);

        if (prop != null)
        {
#pragma warning disable CS8605 // Unboxing a possibly null value.
            return (Color)prop.GetValue(null);
#pragma warning restore CS8605 // Unboxing a possibly null value.
        }

        return ThrowFormatException();

        static void ThrowArgumentException() => throw new ArgumentException("The parameter \"colorString\" must not be null or empty.");
        static Color ThrowFormatException() => throw new FormatException("The parameter \"colorString\" is not a recognized Color format.");
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
        } else if (max == g)
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
            < 2 => (x,  chroma, 0d),
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
}
