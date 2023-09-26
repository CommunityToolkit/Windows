---
title: TabbedCommandBar
author: yoshiask
description: A control for displaying multiple CommandBars in the same space, like Microsoft Office's ribbon.
keywords: TabbedCommandBar, Control, Layout, commandbar, ribbon
dev_langs:
  - csharp
category: Controls
subcategory: Layout
discussion-id: 0
issue-id: 0
icon: Assets/TabbedCommandBar.png
---

The [TabbedCommandBar](/dotnet/api/microsoft.toolkit.uwp.ui.controls.tabbedcommandbar) displays a set of [TabbedCommandBarItem](/dotnet/api/microsoft.toolkit.uwp.ui.controls.tabbedcommandbaritem) in a shared container found in many productivity type apps. It is based off of [NavigationView](/windows/uwp/design/controls-and-patterns/navigationview).

`TabbedCommandBarItem` can be used to display certain items, and its `IsContextual` property can be set to change the default style into an item that is known from the Office apps to highlight to a user that certain context options are available. 
> [!Sample TabbedCommandBarSample]

## Remarks

The TabbedCommandBar automatically applies styles to known common controls inside an `AppBarElementContainer`. The following elements have styles:

- ComboBox
- SplitButton

> [!NOTE]
> The ComboBox does not allow changing its selection while it is in the overflow flyout.

The `TabbedCommandBar` does not add any of its own properties. See [NavigationView](/uwp/api/windows.ui.xaml.controls.navigationview#properties) for a list of accessible properties.
