---
title: SettingsExpander
author: niels9001
description: An expander control that can be used to create Windows 11 style settings experiences.
keywords: SettingsCard, SettingsExpander, Expander, Control, Layout, Settings
dev_langs:
  - csharp
category: Controls
subcategory: Layout
discussion-id: 0
issue-id: 0
icon: Assets/SettingsExpander.png
---

The `SettingsExpander` can be used to group multiple `SettingsCard`s into a single collapsible group.

A `SettingsExpander` can have it's own content to display a setting on the right, just like a `SettingsCard`, but in addition can have any number of extra `Items` to include as additional settings. These items are `SettingsCard`s themselves, which means you can easily move a setting into or out of Expanders just by cutting and pasting their XAML!

> [!SAMPLE SettingsExpanderSample]

You can easily override certain properties to create custom experiences. For instance, you can customize the `ContentAlignment` of a `SettingsCard`, to align your content to the Right (default), Left (hiding the `HeaderIcon`, `Header` and `Description`) or Vertically (usually best paired with changing the `HorizontalContentAlignment` to `Stretch`).

`SettingsExpander` is also an `ItemsControl`, so its items can be driven by a collection and the `ItemsSource` property. You can use the `ItemTemplate` to define how your data object is represented as a `SettingsCard`, as shown below. The `ItemsHeader` and `ItemsFooter` property can be used to host custom content at the start or end of the items list.

> [!SAMPLE SettingsExpanderItemsSourceSample]

NOTE: Due to [a bug](https://github.com/microsoft/microsoft-ui-xaml/issues/3842) related to the `ItemsRepeater` used in `SettingsExpander`, there might be visual glitches whenever the `SettingsExpander` expands and a `MaxWidth` is set on a parent `StackPanel`. As a workaround, the `StackPanel` (that has the `MaxWidth` set) can be wrapped in a `Grid` to overcome this issue. See the `SettingsPageExample` for snippet.

### Settings page example

The following sample provides a typical design page, following the correct Windows 11 design specifications for things like spacing, section headers and animations.

> [!SAMPLE SettingsPageExample]
