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
icon: Assets/HeaderedItemsControl.png
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


## Properties

| Property | Type | Gets or sets the data used for the header of each control |
| -- | -- | -- |
| Header | object | Gets or sets the data used for the header of each control |
| HeaderTemplate | DataTemplate | Gets or sets the template used to display the content of the control's footer |
| Footer | object | Gets or sets the data used for the header of each control |
| FooterTemplate | DataTemplate | Gets or sets the template used to display the content of the control's footer |