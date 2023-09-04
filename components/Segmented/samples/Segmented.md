---
title: Segmented
author: niels9001
description: A common UI control to configure a view or setting. 
keywords: SegmentedControl, Control, Layout, Segmented
dev_langs:
  - csharp
category: Controls
subcategory: Input
discussion-id: 314
issue-id: 392
icon: Assets/Segmented.png
---

## The basics

The `Segmented` control is best used with 2-5 items and does not support overflow. The `Icon` and `Content` property can be set on the `SegmentedItems`.

> [!Sample SegmentedBasicSample]

## Selection

`Segmented` supports single and multi-selection. When `SelectionMode` is set to `Single` the first item will be selected by default. This can be overridden by setting `AutoSelection` to `false`.

## Other styles

The `Segmented` control contains various additional styles, to match the look and feel of your application. The `PivotSegmentedStyle` matches a modern `Pivot` style while the `ButtonSegmentedStyle` represents buttons. To load these styles, make sure to add the `ResourceDictionary` as a resource (see `Page.Resources` sample below).

> [!SAMPLE SegmentedStylesSample]
