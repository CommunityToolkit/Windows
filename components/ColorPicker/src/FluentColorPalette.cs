// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Color = Windows.UI.Color; // Type can be changed to CoreColor, etc.

namespace CommunityToolkit.WinUI.Controls;

/// <summary>
/// Implements the standard Windows 10 color palette.
/// </summary>
public class FluentColorPalette : IColorPalette
{
    /* Values were taken from the Settings App, Personalization > Colors which match with
     * https://docs.microsoft.com/en-us/windows/uwp/whats-new/windows-docs-december-2017
     *
     * The default ordering and grouping of colors was undesirable so was modified.
     * Colors were transposed: the colors in rows within the Settings app became columns here.
     * This is because columns in an IColorPalette generally should contain different shades of
     * the same color. In the settings app this concept is somewhat loosely reversed.
     * The first 'column' ordering, after being transposed, was then reversed so 'red' colors
     * were near to each other.
     *
     * This new ordering most closely follows the Windows standard while:
     *
     *  1. Keeping colors in a 'spectrum' order
     *  2. Keeping like colors next to each both in rows and columns
     *     (which is unique for the windows palette).
     *     For example, similar red colors are next to each other in both
     *     rows within the same column and rows within the column next to it.
     *     This follows a 'snake-like' pattern as illustrated below.
     *  3. A downside of this ordering is colors don't follow strict 'shades'
     *     as in other palettes.
     *
     * The colors will be displayed in the below pattern.
     * This pattern follows a spectrum while keeping like-colors near to one
     * another across both rows and columns.
     *
     *      ┌Red───┐      ┌Blue──┐      ┌Gray──┐
     *      │      │      │      │      │      |
     *      │      │      │      │      │      |
     * Yellow      └Violet┘      └Green─┘      Brown
     */
    private static Color[,] colorChart = new Color[,]
    {
        {
            // Ordering reversed for this section only
            Color.FromArgb(255, 255, 185, 0), /* #ffb900 */
            Color.FromArgb(255, 209, 52, 56), /* #d13438 */
            Color.FromArgb(255, 227, 0, 140), /* #e3008c */
            Color.FromArgb(255, 142, 140, 216), /* #8e8cd8 */
            Color.FromArgb(255, 0, 153, 188), /* #0099bc */
            Color.FromArgb(255, 0, 204, 106), /* #00cc6a */
            Color.FromArgb(255, 86, 124, 115), /* #567c73 */
            Color.FromArgb(255, 105, 121, 126), /* #69797e */
        },
        {
            Color.FromArgb(255, 255, 140, 0), /* #ff8c00 */
            Color.FromArgb(255, 255, 67, 67), /* #ff4343 */
            Color.FromArgb(255, 191, 0, 119), /* #bf0077 */
            Color.FromArgb(255, 107, 105, 214), /* #6b69d6 */
            Color.FromArgb(255, 45, 125, 154), /* #2d7d9a */
            Color.FromArgb(255, 16, 137, 62), /* #10893e */
            Color.FromArgb(255, 72, 104, 96), /* #486860 */
            Color.FromArgb(255, 74, 84, 89), /* #4a5459 */
        },
        {
            Color.FromArgb(255, 247, 99, 12), /* #f7630c */
            Color.FromArgb(255, 231, 72, 86), /* #e74856 */
            Color.FromArgb(255, 194, 57, 179), /* #c239b3 */
            Color.FromArgb(255, 135, 100, 184), /* #8764b8 */
            Color.FromArgb(255, 0, 183, 195), /* #00b7c3 */
            Color.FromArgb(255, 122, 117, 116), /* #7a7574 */
            Color.FromArgb(255, 73, 130, 5), /* #498205 */
            Color.FromArgb(255, 100, 124, 100), /* #647c64 */
        },
        {
            Color.FromArgb(255, 202, 80, 16), /* #ca5010 */
            Color.FromArgb(255, 232, 17, 35), /* #e81123 */
            Color.FromArgb(255, 154, 0, 137), /* #9a0089 */
            Color.FromArgb(255, 116, 77, 169), /* #744da9 */
            Color.FromArgb(255, 3, 131, 135), /* #038387 */
            Color.FromArgb(255, 93, 90, 88), /* #5d5a58 */
            Color.FromArgb(255, 16, 124, 16), /* #107c10 */
            Color.FromArgb(255, 82, 94, 84), /* #525e54 */
        },
        {
            Color.FromArgb(255, 218, 59, 1), /* #da3b01 */
            Color.FromArgb(255, 234, 0, 94), /* #ea005e */
            Color.FromArgb(255, 0, 120, 212), /* #0078d4 */
            Color.FromArgb(255, 177, 70, 194), /* #b146c2 */
            Color.FromArgb(255, 0, 178, 148), /* #00b294 */
            Color.FromArgb(255, 104, 118, 138), /* #68768a */
            Color.FromArgb(255, 118, 118, 118), /* #767676 */
            Color.FromArgb(255, 132, 117, 69), /* #847545 */
        },
        {
            Color.FromArgb(255, 239, 105, 80), /* #ef6950 */
            Color.FromArgb(255, 195, 0, 82), /* #c30052 */
            Color.FromArgb(255, 0, 99, 177), /* #0063b1 */
            Color.FromArgb(255, 136, 23, 152), /* #881798 */
            Color.FromArgb(255, 1, 133, 116), /* #018574 */
            Color.FromArgb(255, 81, 92, 107), /* #515c6b */
            Color.FromArgb(255, 76, 74, 72), /* #4c4a48 */
            Color.FromArgb(255, 126, 115, 95), /* #7e735f */
        }
    };

    /***************************************************************************************
     *
     * Color Indexes
     *
     ***************************************************************************************/

    /// <summary>
    /// Gets the index of the default shade of colors in this palette.
    /// This has little meaning in this palette as colors are not strictly separated by shade.
    /// </summary>
    public const int DefaultShadeIndex = 0;

    /***************************************************************************************
     *
     * Property Accessors
     *
     ***************************************************************************************/

    ///////////////////////////////////////////////////////////
    // Palette
    ///////////////////////////////////////////////////////////

    /// <summary>
    /// Gets the total number of colors in this palette.
    /// A color is not necessarily a single value and may be composed of several shades.
    /// This has little meaning in this palette as colors are not strictly separated.
    /// </summary>
    public int ColorCount
    {
        get { return colorChart.GetLength(0); }
    }

    /// <summary>
    /// Gets the total number of shades for each color in this palette.
    /// Shades are usually a variation of the color lightening or darkening it.
    /// This has little meaning in this palette as colors are not strictly separated by shade.
    /// </summary>
    public int ShadeCount
    {
        get { return colorChart.GetLength(1); }
    }

    /***************************************************************************************
     *
     * Methods
     *
     ***************************************************************************************/

    /// <summary>
    /// Gets a color in the palette by index.
    /// </summary>
    /// <param name="colorIndex">The index of the color in the palette.
    /// The index must be between zero and <see cref="ColorCount"/>.</param>
    /// <param name="shadeIndex">The index of the color shade in the palette.
    /// The index must be between zero and <see cref="ShadeCount"/>.</param>
    /// <returns>The color at the specified index or an exception.</returns>
    public Color GetColor(
        int colorIndex,
        int shadeIndex)
    {
        return colorChart[
            Math.Clamp(colorIndex, 0, colorChart.GetLength(0)),
            Math.Clamp(shadeIndex, 0, colorChart.GetLength(1))];
    }
}
