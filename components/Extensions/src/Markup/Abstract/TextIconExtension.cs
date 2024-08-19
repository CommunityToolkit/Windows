// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#if WINAPPSDK
using Microsoft.UI.Text;
#else
using Windows.UI.Text;
#endif

namespace CommunityToolkit.WinUI;

/// <summary>
/// An abstract <see cref="MarkupExtension"/> which to produce text-based icons.
/// </summary>
public abstract class TextIconExtension : MarkupExtension
{
    /// <summary>
    /// Gets or sets the size of the icon to display.
    /// </summary>
    public double FontSize { get; set; }

    [ThreadStatic]
	private static FontFamily? symbolThemeFontFamily;

    /// <summary>
    /// Gets the reusable "Segoe MDL2 Assets" <see cref="FontFamily"/> instance.
    /// </summary>
    protected static FontFamily SymbolThemeFontFamily
    {
        get => symbolThemeFontFamily ??= new FontFamily("Segoe Fluent Icons,Segoe MDL2 Assets");
    }

    /// <summary>
    /// Gets or sets the thickness of the icon glyph.
    /// </summary>
    #if WINUI3
    public Windows.UI.Text.FontWeight FontWeight { get; set; } = Microsoft.UI.Text.FontWeights.Normal;
    #elif WINUI2
    public Windows.UI.Text.FontWeight FontWeight { get; set; } = Windows.UI.Text.FontWeights.Normal;
    #endif

    /// <summary>
    /// Gets or sets the font style for the icon glyph.
    /// </summary>
    #if WINUI3
    public Windows.UI.Text.FontStyle FontStyle { get; set; } = Windows.UI.Text.FontStyle.Normal;
    #elif WINUI2
    public Windows.UI.Text.FontStyle FontStyle { get; set; } = Windows.UI.Text.FontStyle.Normal;
    #endif

    /// <summary>
    /// Gets or sets the foreground <see cref="Brush"/> for the icon.
    /// </summary>
    public Brush? Foreground { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether automatic text enlargement, to reflect the system text size setting, is enabled.
    /// </summary>
    public bool IsTextScaleFactorEnabled { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the icon is mirrored when the flow direction is right to left.
    /// </summary>
    public bool MirroredWhenRightToLeft { get; set; }
}
