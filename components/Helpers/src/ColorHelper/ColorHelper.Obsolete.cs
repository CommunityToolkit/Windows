// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Windows.UI;

namespace CommunityToolkit.WinUI.Helpers;

public static partial class ColorHelper
{
    /// <summary>
    /// Creates a <see cref="Color"/> from the specified hue, saturation, lightness, and alpha values.
    /// </summary>
    /// <param name="hue">0..360 range hue</param>
    /// <param name="saturation">0..1 range saturation</param>
    /// <param name="lightness">0..1 range lightness</param>
    /// <param name="alpha">0..1 alpha</param>
    /// <returns>The created <see cref="Color"/>.</returns>
    [Obsolete("This method is marked deprecated, and will be removed in a future version. Please replace with (Color)HslColor.Create().")]
    public static Color FromHsl(double hue, double saturation, double lightness, double alpha = 1.0)
        => HslColor.Create(hue, saturation, lightness, alpha);

    /// <summary>
    /// Creates a <see cref="Color"/> from the specified hue, saturation, value, and alpha values.
    /// </summary>
    /// <param name="hue">0..360 range hue</param>
    /// <param name="saturation">0..1 range saturation</param>
    /// <param name="value">0..1 range value</param>
    /// <param name="alpha">0..1 alpha</param>
    /// <returns>The created <see cref="Color"/>.</returns>
    [Obsolete("This method is marked deprecated, and will be removed in a future version. Please replace with (Color)HsvColor.Create().")]
    public static Color FromHsv(double hue, double saturation, double value, double alpha = 1.0)
        => HsvColor.Create(hue, saturation, value, alpha);
}
