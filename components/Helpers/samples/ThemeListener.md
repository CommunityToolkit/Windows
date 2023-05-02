---
title: Theme Listener
author: williamabradley
description: The ThemeListener allows you to keep track of changes to the System Theme.
keywords: Helpers, Theming, theme listerner, themes
dev_langs:
  - csharp
category: Helpers
subcategory: Layout
discussion-id: 0
issue-id: 0
---

# Theme Listener

The [Theme Listener](/dotnet/api/microsoft.toolkit.uwp.ui.helpers.themelistener) class allows you to determine the current Application Theme, and when it is changed via System Theme changes.

KNOWN ISSUE: `ThemeListener` might not work in WinUI3 applications.

> [!Sample ThemeListenerSample]

## Syntax

```csharp
var Listener = new ThemeListener();
Listener.ThemeChanged += Listener_ThemeChanged;

private void Listener_ThemeChanged(ThemeListener sender)
{
    var theme = sender.CurrentTheme;
    // Use theme dependent code.
}
```

```vb
Dim listener = New ThemeListener()
AddHandler listener.ThemeChanged, AddressOf Listener_ThemeChanged

Private Sub Listener_ThemeChanged(ByVal sender As ThemeListener)
    Dim theme = sender.CurrentTheme
    ' Use theme dependent code.
End Sub
```

## Properties

| Property | Type | Description |
| -- | -- | -- |
| CurrentTheme | [ApplicationTheme](/uwp/api/Windows.UI.Xaml.ApplicationTheme) | Gets or sets the Current Theme. |
| CurrentThemeName | string | Gets the Name of the Current Theme. |
| IsHighContrast | bool | Gets or sets a value indicating whether the current theme is high contrast. |

## Events

| Events | Description |
| -- | -- |
| ThemeChanged | An event that fires if the Theme changes. |
