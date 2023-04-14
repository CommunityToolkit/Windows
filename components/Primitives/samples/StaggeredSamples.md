---
title: StaggeredLayout / StaggeredPanel
author: skendrot
description: The StaggeredLayout / StaggeredPanel virtualizes items in a column approach where an item will be added to whichever column has used the least amount of space.
keywords: StaggeredPanel, StaggeredLayout, Layout
dev_langs:
  - csharp
category: Controls
subcategory: Layout
discussion-id: 0
issue-id: 0
---

# StaggeredLayout

The [StaggeredLayout](/dotnet/api/microsoft.toolkit.uwp.ui.controls.StaggeredLayout) allows for layout of items in a column approach where an item will be added to whichever column has used the least amount of space.

## Syntax

```xaml
<winui:ItemsRepeater>
    <winui:ItemsRepeater.Layout>
        <controls:StaggeredLayout />
    </winui:ItemsRepeater.Layout>
    <winui:ItemsRepeater.ItemTemplate>
        <DataTemplate>
            <Image Source="{Binding Thumbnail}"/>
        </DataTemplate>
    </winui:ItemsRepeater.ItemTemplate>
</winui:ItemsRepeater>

```

## Properties

| Property | Type | Description |
| -- | -- | -- |
| DesiredColumnWidth | double | The desired width of each column. The width of columns can exceed the DesiredColumnWidth if the HorizontalAlignment is set to Stretch. |
| ColumnSpacing | double  | Gets or sets the distance between columns |
| RowSpacing | double  | Gets or sets the vertical distance between items |

> [!SAMPLE StaggeredLayoutSample]


# StaggeredPanel

The [StaggeredPanel](/dotnet/api/microsoft.toolkit.uwp.ui.controls.staggeredpanel) allows for layout of items in a column approach where an item will be added to whichever column has used the least amount of space.

## Syntax

```xaml
<ItemsControl>
    <ItemsControl.ItemTemplate>
        <DataTemplate>
            <Image Source="{Binding Thumbnail}"/>
        </DataTemplate>
    </ItemsControl.ItemTemplate>
    <ItemsControl.ItemsPanel>
        <ItemsPanelTemplate>
            <controls:StaggeredPanel/>
        </ItemsPanelTemplate>
    </ItemsControl.ItemsPanel>
</ItemsControl>
```

## Properties

| Property | Type | Description |
| -- | -- | -- |
| ColumnSpacing | double  | Gets or sets the distance between columns |
| DesiredColumnWidth | double | The desired width of each column. The width of columns can exceed the DesiredColumnWidth if the HorizontalAlignment is set to Stretch. |
| Padding | Thickness | The dimensions of the space between the edge and its child as a Thickness value. Thickness is a structure that stores dimension values using pixel measures. |
| RowSpacing | double  | Gets or sets the vertical distance between items |
