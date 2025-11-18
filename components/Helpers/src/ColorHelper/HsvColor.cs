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
    /// <summary>
    /// The hue value.
    /// </summary>
    [Obsolete("This field is marked deprecated, and will be removed in a future version. Please replace with the HsvColor.Hue property")]
    public double H;

    /// <summary>
    /// The saturation value.
    /// </summary>
    [Obsolete("This field is marked deprecated, and will be removed in a future version. Please replace with the HsvColor.Saturation property")]
    public double S;

    /// <summary>
    /// The "value" value.
    /// </summary>
    [Obsolete("This field is marked deprecated, and will be removed in a future version. Please replace with the HsvColor.Value property")]
    public double V;

    /// <summary>
    /// The alpha/opacity value.
    /// </summary>
    [Obsolete("This field is marked deprecated, and will be removed in a future version. Please replace with the HsvColor.Alpha property")]
    public double A;

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
        Hue = 60 * h1;
        Saturation = saturation;
        Value = value;
        Alpha = alpha;
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
        color.Hue = hue;
        color.Saturation = saturation;
        color.Value = value;
        color.Alpha = alpha;
        return color;
    }


    // This class contains deprecated public backing fields to be removed in a future version.
    // Suppress the warnings from using them in their new wrapping properties.
#pragma warning disable 0618

    /// <summary>
    /// Gets or sets the hue.
    /// </summary>
    /// <remarks>
    /// This value is clamped between 0 and 360.
    /// </remarks>
    public double Hue
    {
        readonly get => Math.Clamp(H, 0, 360);
        set => H = Math.Clamp(value, 0, 360);
    }
    
    /// <summary>
    /// Gets or sets the saturation.
    /// </summary>
    /// <remarks>
    /// This value is clamped between 0 and 1.
    /// </remarks>
    public double Saturation
    {
        readonly get => Math.Clamp(S, 0, 1);
        set => S = Math.Clamp(value, 0, 1);
    }
    
    /// <summary>
    /// Gets or sets the color value.
    /// </summary>
    /// <remarks>
    /// This value is clamped between 0 and 1.
    /// </remarks>
    public double Value
    {
        readonly get => Math.Clamp(V, 0, 1);
        set => V = Math.Clamp(value, 0, 1);
    }
    
    /// <summary>
    /// Gets or sets the alpha/opacity.
    /// </summary>
    /// <remarks>
    /// This value is clamped between 0 and 1.
    /// </remarks>
    public double Alpha
    {
        readonly get => Math.Clamp(A, 0, 1);
        set => A = Math.Clamp(value, 0, 1);
    }

#pragma warning restore 0618


    /// <summary>
    /// Converts the <see cref="HsvColor"/> to a <see cref="Color"/>.
    /// </summary>
    /// <returns>The <see cref="HsvColor"/> as a <see cref="Color"/>.</returns>
    public readonly Color ToColor()
    {
        double chroma = Value * Saturation;
        double h1 = Hue / 60;
        double x = chroma * (1 - Math.Abs((h1 % 2) - 1));
        double m = Value - chroma;

        return ColorHelper.FromHueChroma(h1, chroma, x, m, Alpha);
    }

    /// <inheritdoc/>
    public override readonly string ToString() => $"hsv({Hue:N0}, {Saturation}, {Value})";

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
