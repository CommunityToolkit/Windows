---
title: Developer Tools
author: nmetulev
description: The FocusTracker and AlignmentGrid help you to get more information about and aligning UI elements.
keywords: DeveloperTools, FocusTracker, AlignmentGrid, dev tools, controls
dev_langs:
  - csharp
category: Helpers
subcategory: Developer
discussion-id: 0
issue-id: 0
icon: Assets/DeveloperTools.png
---

## AlignmentGrid XAML Control

The `AlignmentGrid Control` can be used to display a grid to help with aligning controls.

You can control the grid's steps with `HorizontalStep` and `VerticalStep` properties. Line color can be defined with `LineBrush` property.

> [!Sample AlignmentGridSample]

## FocusTracker

The `FocusTracker Control` can be used to display information about the current focused XAML element (if any).

FocusTracker will display the following information (when available) about the current focused XAML element:

- Name
- Type
- AutomationProperties.Name
- Name of the first parent in hierarchy with a name

> [!Sample FocusTrackerSample]
