---
title: Theme Listener
author: williamabradley
description: The ThemeListener allows you to keep track of changes to the System Theme.
keywords: Helpers, Theming, theme listerner, themes
dev_langs:
  - csharp
category: Helpers
subcategory: System
discussion-id: 0
issue-id: 0
icon: assets/icon.png
---

# Theme Listener

The [Theme Listener](/dotnet/api/microsoft.toolkit.uwp.ui.helpers.themelistener) class allows you to determine the current Application Theme, and when it is changed via System Theme changes.

KNOWN ISSUE: `ThemeListener` might not work in WinUI3 applications.

> [!Sample ThemeListenerSample]

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
