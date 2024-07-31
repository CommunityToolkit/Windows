// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace CommunityToolkit.WinUI.Controls;

/// <summary>
/// <see cref="StyleSelector"/> used by <see cref="TokenizingTextBox"/> to choose the proper <see cref="TokenizingTextBoxItem"/> container style (text entry or token).
/// </summary>
public partial class TokenizingTextBoxStyleSelector : StyleSelector
{
    /// <summary>
    /// Gets or sets the <see cref="Style"/> of a token item.
    /// </summary>
    public Style TokenStyle { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="Style"/> of a text entry item.
    /// </summary>
    public Style TextStyle { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="TokenizingTextBoxStyleSelector"/> class.
    /// </summary>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public TokenizingTextBoxStyleSelector()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
    }

    /// <inheritdoc/>
    protected override Style SelectStyleCore(object item, DependencyObject container)
    {
        if (item is ITokenStringContainer)
        {
            return TextStyle;
        }

        return TokenStyle;
    }
}
