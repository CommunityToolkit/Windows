---
title: HeaderedItemsControl
author: skendrot
description: The HeaderedItemsControl allows items to be displayed with a specified header.
keywords: HeaderedItemsControl, Control, Layout
dev_langs:
  - csharp
category: Controls
subcategory: Layout
discussion-id: 0
issue-id: 0
icon: Assets/HeaderedItemsControl.png
---
The `Header` property can be any object and you can use the `HeaderTemplate` to specify a custom look to the header. Similiar objects can be set for the `Footer` and `FooterTemplate`.

> [!NOTE]
> Setting the `Background`, `BorderBrush` and `BorderThickness` properties will not have any effect on the HeaderedItemsControl. This is to maintain the same functionality as the ItemsControl.

> [!Sample HeaderedItemsControlSample]

## Syntax

```xaml
<Page ...
     xmlns:controls="using:CommunityToolkit.WinUI.Controls"/>

<controls:HeaderedItemsControl>
    <!-- Header content or HeaderTemplate content -->
</controls:HeaderedItemsControl>
```


## Properties

| Property | Type | Gets or sets the data used for the header of each control |
| -- | -- | -- |
| Header | object | Gets or sets the data used for the header of each control |
| HeaderTemplate | DataTemplate | Gets or sets the template used to display the content of the control's footer |
| Footer | object | Gets or sets the data used for the header of each control |
| FooterTemplate | DataTemplate | Gets or sets the template used to display the content of the control's footer |
| Orientation | Orientation | Gets or sets the Orientation to use for layout of the header. If set to Vertical the Header will be above the items. If set to Horizontal the Header will be to the left of the items. |
