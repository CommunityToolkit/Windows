---
title: RichSuggestBox
author: huynhsontung
description: A rich text input control that auto-suggests and stores token items in a document.
keywords: RichSuggestBox, Control,
dev_langs:
  - csharp
category: Controls
subcategory: Text
discussion-id: 0
issue-id: 0
icon: Assets/RichSuggestBox.png
---

The `RichSuggestBox` is a combination of [AutoSuggestBox](https://learn.microsoft.com/uwp/api/windows.ui.xaml.controls.autosuggestbox) and [RichEditBox](https://learn.microsoft.com/uwp/api/windows.ui.xaml.controls.richeditbox) that can provide suggestions based on customizable prefixes. Selected suggestions are embedded and tracked in the document as tokens.

RichSuggestBox resembles text controls commonly found in social applications where you type "@" to mention people.

> [!Sample RichSuggestBoxMultiplePrefixesSample]

## Syntax

```xaml
<controls:RichSuggestBox
  PlaceholderText="Leave a comment"
  ItemTemplate="{StaticResource SuggestionTemplate}"
  Prefixes="@#" />
```

## Remarks

When a suggestion is selected, `RichSuggestBox` assigns the selected item a unique [Guid](https://learn.microsoft.com/dotnet/api/system.guid) and a display text (provided by the developer) to make up a token. The display text is then padded with [Zero Width Space](https://unicode-table.com/200B/)s (ZWSP) and inserted into the document as a hyperlink using the identifier as the link address. These hyperlinks are tracked and validated on every text change.

The token text inserted into the document has the following layout: ZWSP - Prefix character - Display text - ZWSP.

For example, a token with "@" as the prefix and "John Doe" as the display text is inserted as:

```cs
"\u200b@John Doe\u200b"
```

> [!IMPORTANT]
> Token text contains [Zero Width Space](https://unicode-table.com/200B/)s, which are Unicode characters.

> [!NOTE]
> To support Undo/Redo function, `RichSuggestBox` keeps all the tokens in an internal collection even when the token text is deleted from the document. These token are marked as inactive and are not included in the `Tokens` collection. Use `ClearUndoRedoSuggestionHistory()` method to clear inactive tokens or `Clear()` method to clear all tokens.

## Examples

### Handle multiple token types

The example below creates a `RichSuggestBox` that can tokenize both mentions (query starts with `@`) and hashtags (query starts with `#`).

```xaml
<controls:RichSuggestBox
  PlaceholderText="Leave a comment"
  ItemTemplate="{StaticResource SuggestionTemplate}"
  SuggestionChosen="OnSuggestionChosen"
  SuggestionRequested="OnSuggestionRequested"
  Prefixes="@#" />
```

```cs
private void OnSuggestionChosen(RichSuggestBox sender, SuggestionChosenEventArgs args)
{
  if (args.Prefix == "#")
  {
    // User selected a hashtag item
    args.DisplayText = ((SampleHashtagDataType)args.SelectedItem).Text;
  }
  else
  {
    // User selected a mention item
    args.DisplayText = ((SampleEmailDataType)args.SelectedItem).DisplayName;
  }
}

private void OnSuggestionRequested(RichSuggestBox sender, SuggestionRequestedEventArgs args)
{
  sender.ItemsSource = args.Prefix == "#"
    ? _hashtags.Where(x => x.Text.Contains(args.QueryText, StringComparison.OrdinalIgnoreCase))
    : _emails.Where(x => x.DisplayName.Contains(args.QueryText, StringComparison.OrdinalIgnoreCase));
}
```

### Plain text only

The example below creates a `RichSuggestBox` that only allows users to enter plain text. The only formatted texts in the document are tokens.

```xaml
<controls:RichSuggestBox
  ClipboardCopyFormat="PlainText"
  ClipboardPasteFormat="PlainText"
  DisabledFormattingAccelerators="All" />
```

> [!Sample RichSuggestBoxPlainTextSample]
