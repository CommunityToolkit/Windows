---
title: HeaderedTreeView
author: michaelhawker
description: The HeaderedTreeView allows a treeview to be displayed with a specified header.
keywords: HeaderedTreeView, Control, Layout, treeview
dev_langs:
  - csharp
category: Controls
subcategory: Layout
discussion-id: 0
issue-id: 0
icon: Assets/HeaderedTreeView.png
---
The `Header` property can be any object and you can use the `HeaderTemplate` to specify a custom look to the header. Similiar objects can be set for the `Footer` and `FooterTemplate`.


> [!Sample HeaderedTreeViewSample]

## Syntax

```xaml
<Page ...
     xmlns:controls="using:CommunityToolkit.WinUI.Controls"/>

<controls:HeaderedTreeView>
    <!-- Header content or HeaderTemplate content -->
</controls:HeaderedTreeView>
```