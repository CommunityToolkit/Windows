// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#if WINAPPSDK
using Microsoft.UI.Text;
#else
using Windows.UI.Text;
#endif
using CommunityToolkit.Common.Deferred;

namespace CommunityToolkit.WinUI.Controls;

/// <summary>
/// Provides data for the <see cref="RichSuggestBox.SuggestionChosen"/> event.
/// </summary>
public class SuggestionChosenEventArgs : DeferredEventArgs
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    /// <summary>
    /// Gets the query used for this token.
    /// </summary>
    public string QueryText { get; internal set; }

    /// <summary>
    /// Gets the prefix character used for this token.
    /// </summary>
    public string Prefix { get; internal set; }

    /// <summary>
    /// Gets or sets the display text.
    /// </summary>
    public string DisplayText { get; set; }

    /// <summary>
    /// Gets the suggestion item associated with this token.
    /// </summary>

    public object SelectedItem { get; internal set; }

    /// <summary>
    /// Gets token ID.
    /// </summary>
    public Guid Id { get; internal set; }

    /// <summary>
    /// Gets or sets the <see cref="ITextCharacterFormat"/> object used to format the display text for this token.
    /// </summary>
    public ITextCharacterFormat Format { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}
