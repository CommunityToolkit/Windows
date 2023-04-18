---
title: ContentSizer
author: mhawker
description: The ContentSizer is a control which can be used to resize any element, usually its parent.
keywords: ContentSizer, SizerBase, Control, Layout, Expander, Splitter
dev_langs:
  - csharp
category: Controls
subcategory: Layout
discussion-id: 96
issue-id: 101
---

# ContentSizer

The ContentSizer is a control which can be used to resize any element, usually its parent. If you are using a `Grid`, use [GridSplitter](GridSplitter.md) instead.

The main use-case for a ContentSizer is to create an expandable shelf for your application. This allows the `Expander` itself to remember its opening/closing sizes.

A GridSplitter would be insufficient as it would force the grid to remember the row size and maintain its position when the `Expander` collapses.

> [!SAMPLE ContentSizerTopShelfPage]

The following example shows how to use the ContentSizer to create a left-side shelf; however, this scenario can also be accomplished with a `GridSplitter`.

> [!SAMPLE ContentSizerLeftShelfPage]
