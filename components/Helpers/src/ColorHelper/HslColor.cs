// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Windows.UI;
using ColorHelper = CommunityToolkit.WinUI.Helpers.ColorHelper;

namespace CommunityToolkit.WinUI;

/// <summary>
/// Defines a color in Hue/Saturation/Lightness (HSL) space with alpha.
/// </summary>
public struct HslColor
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
    /// The lightness value.
    /// </summary>
    [Obsolete("This field is marked deprecated, and will be removed in a future version. Please replace with the HsvColor.Lightness property")]
    public double L;

    /// <summary>
    /// The alpha/opacity value.
    /// </summary>
    [Obsolete("This field is marked deprecated, and will be removed in a future version. Please replace with the HsvColor.Alpha property")]
    public double A;

    /// <summary>
    /// Initializes a new instance of the <see cref="HsvColor"/> struct.
    /// </summary>
    /// <param name="color">The <see cref="Color"/> to convert to a <see cref="HsvColor"/>.</param>
    private HslColor(Color color)
    {
        // Calculate hue, chroma, and supplementary values
        (double h1, double chroma) = ColorHelper.CalculateHueAndChroma(color, out var min, out var max, out var alpha);
        
        // Calculate saturation and lightness
        double lightness = 0.5 * (max + min);
        double saturation = chroma == 0 ? 0 : chroma / (1 - Math.Abs((2 * lightness) - 1));
        
        // Set hsl properties
        H = 60 * h1;
        S = saturation;
        L = lightness;
        A = alpha;
    }

    /// <summary>
    /// Creates a new <see cref="HslColor"/>.
    /// </summary>
    /// <param name="hue">The color's hue.</param>
    /// <param name="saturation">The color's saturation.</param>
    /// <param name="lightness">The color's lightness.</param>
    /// <param name="alpha">The alpha/opacity.</param>
    /// <returns>The new <see cref="HslColor"/>.</returns>
    public static HslColor Create(double hue, double saturation, double lightness, double alpha = 1)
    {
        HslColor color = default;
        color.H = hue;
        color.S = saturation;
        color.L = lightness;
        color.A = alpha;
        return color;
    }

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
    /// Gets or sets the lightness.
    /// </summary>
    /// <remarks>
    /// This value is clamped between 0 and 1.
    /// </remarks>
    public double Lightness
    {
        readonly get => Math.Clamp(L, 0, 1);
        set => L = Math.Clamp(value, 0, 1);
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

    /// <summary>
    /// Converts the <see cref="HslColor"/> to a <see cref="Color"/>.
    /// </summary>
    /// <returns>The <see cref="HslColor"/> as a <see cref="Color"/>.</returns>
    public readonly Color ToColor()
    {
        double chroma = (1 - Math.Abs((2 * L) - 1)) * S;
        double h1 = H / 60;
        double x = chroma * (1 - Math.Abs((h1 % 2) - 1));
        double m = L - (0.5 * chroma);
        
        return ColorHelper.FromHueChroma(h1, chroma, x, m, A);
    }

    /// <inheritdoc/>
    public override readonly string ToString() => $"hsl({H:N0}, {S}, {L})";

    /// <summary>
    /// Cast a <see cref="HslColor"/> to a <see cref="Color"/>.
    /// </summary>
    public static implicit operator Color(HslColor hsl) => hsl.ToColor();

    /// <summary>
    /// Cast a <see cref="Color"/> to <see cref="HslColor"/>
    /// </summary>
    public static explicit operator HslColor(Color color) => new(color);

    /// <summary>
    /// Cast a <see cref="HsvColor"/> to <see cref="HslColor"/>
    /// </summary>
    public static explicit operator HslColor(HsvColor color) => new(color);
}
