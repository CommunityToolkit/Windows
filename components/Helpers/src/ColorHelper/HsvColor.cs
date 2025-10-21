// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Windows.UI;
using ColorHelper = CommunityToolkit.WinUI.Helpers.ColorHelper;

namespace CommunityToolkit.WinUI;

/// <summary>
/// Defines a color in Hue/Saturation/Value (HSV) space with alpha.
/// </summary>
public struct HsvColor
{
    private double _hue;
    private double _saturation;
    private double _value;
    private double _alpha;

    /// <summary>
    /// Initializes a new instance of the <see cref="HsvColor"/> struct.
    /// </summary>
    /// <param name="color">The <see cref="Color"/> to convert to a <see cref="HsvColor"/>.</param>
    private HsvColor(Color color)
    {
        // Calculate hue, chroma, and supplementary values
        (double h1, double chroma) = ColorHelper.CalculateHueAndChroma(color, out var _, out var max, out var alpha);

        // Calculate saturation and value
        double saturation = chroma == 0 ? 0 : chroma / max;
        double value = max;

        // Set hsv properties
        H = 60 * h1;
        S = saturation;
        V = value;
        A = alpha;
    }

    /// <summary>
    /// Creates a new <see cref="HsvColor"/>.
    /// </summary>
    /// <param name="hue">The color's hue.</param>
    /// <param name="saturation">The color's saturation.</param>
    /// <param name="value">The color's value.</param>
    /// <param name="alpha">The alpha/opacity.</param>
    /// <returns>The new <see cref="HsvColor"/>.</returns>
    public static HsvColor Create(double hue, double saturation, double value, double alpha = 1)
    {
        HsvColor color = default;
        color.H = hue;
        color.S = saturation;
        color.V = value;
        color.A = alpha;
        return color;
    }
    
    /// <summary>
    /// Gets or sets the hue.
    /// </summary>
    /// <remarks>
    /// This value is clamped between 0 and 360.
    /// </remarks>
    public double H
    {
        readonly get => _hue;
        set => _hue = Math.Clamp(value, 0, 360);
    }
    
    /// <summary>
    /// Gets or sets the saturation.
    /// </summary>
    /// <remarks>
    /// This value is clamped between 0 and 1.
    /// </remarks>
    public double S
    {
        readonly get => _saturation;
        set => _saturation = Math.Clamp(value, 0, 1);
    }
    
    /// <summary>
    /// Gets or sets the color value.
    /// </summary>
    /// <remarks>
    /// This value is clamped between 0 and 1.
    /// </remarks>
    public double V
    {
        readonly get => _value;
        set => _value = Math.Clamp(value, 0, 1);
    }
    
    /// <summary>
    /// Gets or sets the alpha/opacity.
    /// </summary>
    /// <remarks>
    /// This value is clamped between 0 and 1.
    /// </remarks>
    public double A
    {
        readonly get => _alpha;
        set => _alpha = Math.Clamp(value, 0, 1);
    }

    /// <summary>
    /// Converts the <see cref="HsvColor"/> to a <see cref="Color"/>.
    /// </summary>
    /// <returns>The <see cref="HsvColor"/> as a <see cref="Color"/>.</returns>
    public readonly Color ToColor()
    {
        double chroma = V * S;
        double h1 = H / 60;
        double x = chroma * (1 - Math.Abs((h1 % 2) - 1));
        double m = V - chroma;

        return ColorHelper.FromHueChroma(h1, chroma, x, m, A);
    }

    /// <inheritdoc/>
    public override readonly string ToString() => $"hsv({H:N0}, {S}, {V})";

    /// <summary>
    /// Cast a <see cref="HsvColor"/> to a <see cref="Color"/>.
    /// </summary>
    public static implicit operator Color(HsvColor hsv) => hsv.ToColor();

    /// <summary>
    /// Cast a <see cref="Color"/> to <see cref="HsvColor"/>
    /// </summary>
    public static explicit operator HsvColor(Color color) => new(color);

    /// <summary>
    /// Cast a <see cref="HslColor"/> to <see cref="HsvColor"/>
    /// </summary>
    public static explicit operator HsvColor(HslColor color) => new(color);
}
