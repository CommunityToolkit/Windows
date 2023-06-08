---
title: Developer Tools
author: nmetulev
description: The FocusTracker and AlignmentGrid help you to get more information about and aligning UI elements.
keywords: DeveloperTools, FocusTracker, AlignmentGrid, dev tools, controls
dev_langs:
  - csharp
category: Xaml
subcategory: Layout
discussion-id: 0
issue-id: 0
icon: assets/icon.png
---

# AlignmentGrid XAML Control

The [AlignmentGrid Control](/dotnet/api/microsoft.toolkit.uwp.developertools.alignmentgrid) can be used to display a grid to help with aligning controls.

You can control the grid's steps with `HorizontalStep` and `VerticalStep` properties. Line color can be defined with `LineBrush` property.

> [!Sample AlignmentGridSample]

## Properties

| Property | Type | Description |
| -- | -- | -- |
| HorizontalStep | double | Gets or sets the step to use horizontally |
| LineBrush | Brush | Gets or sets line Brush |
| VerticalStep | double | Gets or sets the step to use vertically |


# FocusTracker

The [FocusTracker Control](/dotnet/api/microsoft.toolkit.uwp.developertools.focustracker) can be used to display information about the current focused XAML element (if any).

FocusTracker will display the following information (when available) about the current focused XAML element:

- Name
- Type
- AutomationProperties.Name
- Name of the first parent in hierarchy with a name

> [!Sample FocusTrackerSample]
