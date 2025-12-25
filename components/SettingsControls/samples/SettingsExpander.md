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

### Dragging Settings Cards

You may use a list of `SettingsCard` or `SettingsExpander` to represent configurable items within a tool. The order of these may be something you want to track.

In this case there is a conflict between the interactions with the settings card and the drag and drop interactions of a parent containing `ListView`, for instance.

Therefore, it is recommended to use the drag-handle type UI approach for this scenario in having a dedicated space for re-ordering manipulation vs. interaction with the Settings control.

You can see how to do this with `SettingsExpander` in the example below, however it equally works with a collection of `SettingsCard` as the main data template component as well.

> [!SAMPLE SettingsExpanderDragHandleSample]

The main important pieces of this example are:

1. Enabling the three drag properties on the `ListView`: `CanDragItems`, `CanReorderItems`, and `AllowDrop`.
2. Setting `SelectionMode` to `None` to prevent selection and the selection indicator from appearing.
3. Using a simple UIElement to act as a drag handle, the pass-through of the mouse on this element to `ListView` allows the normal drag experience to work uninterrupted.
4. Modifying the `Margin` and `Padding` values of the `ItemContainerStyle` to align cards how we want within the ListView.
5. Overriding the `ListViewItemBackgroundPointerOver` resource to prevent the hover effect across the entire list item, the Settings Controls already have an effect here on hover.
6. Optionally, add a behavior to highlight the drag region when the pointer is over it.

Note: If controls within the dragged SettingsCard/Expander state are not bound, they will be reset upon drop in some cases. E.g. a ToggleSwitch. It is best to ensure you have bound your control's states to your data model.

### Settings page example

The following sample provides a typical design page, following the correct Windows 11 design specifications for things like spacing, section headers and animations.

> [!SAMPLE SettingsPageExample]
