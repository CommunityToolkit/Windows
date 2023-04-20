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

The [StaggeredLayout](/dotnet/api/microsoft.toolkit.uwp.ui.controls.staggeredlayout) allows for layout of items in a column approach where an item will be added to whichever column has used the least amount of space.

> [!SAMPLE StaggeredLayoutSample]

# StaggeredPanel

The [StaggeredPanel](/dotnet/api/microsoft.toolkit.uwp.ui.controls.staggeredpanel) allows for layout of items in a column approach where an item will be added to whichever column has used the least amount of space.

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
