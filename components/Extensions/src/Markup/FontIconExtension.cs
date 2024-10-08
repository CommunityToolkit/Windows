// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace CommunityToolkit.WinUI;

/// <summary>
/// Custom <see cref="MarkupExtension"/> which can provide <see cref="FontIcon"/> values.
/// </summary>
[MarkupExtensionReturnType(ReturnType = typeof(FontIcon))]
public partial class FontIconExtension : TextIconExtension
{
    /// <summary>
    /// Gets or sets the <see cref="string"/> value representing the icon to display.
    /// </summary>
    public string? Glyph { get; set; }

    /// <summary>
    /// Gets or sets the font family to use to display the icon. If <see langword="null"/>, "Segoe MDL2 Assets" will be used.
    /// </summary>
    public FontFamily? FontFamily { get; set; }

    /// <inheritdoc/>
    protected override object ProvideValue()
    {
        default(ArgumentNullException).ThrowIfNull(Glyph);

        var fontIcon = new FontIcon
        {
            Glyph = Glyph,
            FontFamily = FontFamily ?? SymbolThemeFontFamily,
            FontWeight = FontWeight,
            FontStyle = FontStyle,
            IsTextScaleFactorEnabled = IsTextScaleFactorEnabled,
            MirroredWhenRightToLeft = MirroredWhenRightToLeft
        };

        if (FontSize > 0)
        {
            fontIcon.FontSize = FontSize;
        }

        if (Foreground != null)
        {
            fontIcon.Foreground = Foreground;
        }

        return fontIcon;
    }
}
