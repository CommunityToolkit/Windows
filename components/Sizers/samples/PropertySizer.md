---
title: PropertySizer
author: mhawker
description: The PropertySizer is a control which can be used to manipulate the value of another double based property.
keywords: PropertySizer, SizerBase, Control, Layout, NavigationView, Splitter
dev_langs:
  - csharp
category: Controls
subcategory: Layout
discussion-id: 96
issue-id: 101
---

# PropertySizer

The PropertySizer is a control which can be used to manipulate the value of another double based property. For instance manipulating the `OpenPaneLength` of a `NavigationView` control. If you are using a `Grid`, use `GridSplitter` instead.

# Examples 

The main use-case is for `PropertySizer` to allow you to manipulate the `OpenPaneLength` property of a `NavigationView` control to create a user customizable size shelf. This is handy when using `NavigationView` with a tree of items that represents some project or folder structure for your application.

Both `GridSplitter` and `ContentSizer` are insufficient as they would force the `NavigationView` to a specific size and not allow it to remember its size when it expands or collapses.

> [!SAMPLE PropertySizerNavigationViewPage]
