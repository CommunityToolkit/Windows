---
title: HyperlinkExtensions
author: nmetulev
description: The HyperlinkExtensions allows for a Hyperlink element to invoke the execute method on a bound ICommand instance when clicked.
keywords: windows 10, uwp, windows community toolkit, uwp community toolkit, uwp toolkit, Hyperlink, extensions
dev_langs:
  - csharp
category: Extensions
subcategory: Controls
discussion-id: 0
issue-id: 0
icon: Assets/Extensions.png
---

The `HyperlinkExtensions` class allows for a [`Hyperlink`](https://learn.microsoft.com/uwp/api/Windows.UI.Xaml.Documents.Hyperlink) element to invoke the execute method on a bound [`ICommand`](https://learn.microsoft.com/uwp/api/Windows.UI.Xaml.Input.ICommand) instance when clicked.


## How it works

Using the `HyperlinkExtensions` attached properties simply requires you to assign them to a `Hyperlink` control in XAML, and bind to an `ICommand` instance in a viewmodel. Both `{Binding}` and `{x:Bind}` modes are supported, and you can freely choose which one to use depending on your scenario. Here is a small example of these attached properties in action:

```xaml
<!-- Use Hyperlink in a wrapped TextBlock with text either side and ensure it executes a
     command when clicked, passing the current data context as the command parameter -->
<TextBlock xmlns:ui="using:CommunityToolkit.WinUI">
    <Run>Some leading text with a</Run>
        <Hyperlink
            ui:HyperlinkExtensions.Command="{Binding HyperlinkClicked}"
            ui:HyperlinkExtensions.CommandParameter="{Binding}">hyperlink</Hyperlink>
    <Run>in the middle.</Run>
</TextBlock>
```

## Examples

You can find more examples in the [unit tests](https://github.com/windows-toolkit/WindowsCommunityToolkit/blob/rel/7.1.0/UnitTests/UnitTests.UWP/Extensions/Test_EnumValuesExtension.cs).
