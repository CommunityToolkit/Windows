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
    private double _hue;
    private double _saturation;
    private double _lightness;
    private double _alpha;

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
    /// Gets or sets the lightness.
    /// </summary>
    /// <remarks>
    /// This value is clamped between 0 and 1.
    /// </remarks>
    public double L
    {
        readonly get => _lightness;
        set => _lightness = Math.Clamp(value, 0, 1);
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

    /// <summary>
    /// Cast a <see cref="HslColor"/> to a <see cref="Color"/>.
    /// </summary>
    public static implicit operator Color(HslColor hsl) => hsl.ToColor();
}
