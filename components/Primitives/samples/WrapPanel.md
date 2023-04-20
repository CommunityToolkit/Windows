---
title: WrapPanel
author: nmetulev
description: The WrapPanel Control positions child elements in sequential position from left to right, breaking content to the next line at the edge of the containing box.
keywords: WrapPanel
dev_langs:
  - csharp
category: Controls
subcategory: Layout
discussion-id: 0
issue-id: 0
---

# WrapPanel

The [WrapPanel](/dotnet/api/microsoft.toolkit.uwp.ui.controls.wrappanel) control positions child elements in sequential position from left to right, breaking content to the next line at the edge of the containing box. Subsequent ordering happens sequentially from top to bottom or from right to left, depending on the value of the Orientation property.

The WrapPanel position child controls based on orientation, horizontal orientation (default) positions controls from left to right and vertical orientation positions controls from top to bottom, and once the max width or height is reached the control automatically create row or column based on the orientation.

Spacing can be automatically added between items using the HorizontalSpacing and VerticalSpacing properties. When the Orientation is Horizontal, HorizontalSpacing adds uniform horizontal spacing between each individual item, and VerticalSpacing adds uniform spacing between each row of items.

When the Orientation is Vertical, HorizontalSpacing adds uniform spacing between each column of items, and VerticalSpacing adds uniform vertical spacing between individual items.

> [!SAMPLE WrapPanelSample]

## Properties

| Property | Type | Description |
| -- | -- | -- |
| HorizontalSpacing | double  | Gets or sets the horizontal distance between items |
| Orientation | Orientation | Gets or sets the orientation of the WrapPanel, Horizontal or vertical means that child controls will be added horizontally until the width of the panel can't fit more control then a new row is added to fit new horizontal added child controls, vertical means that child will be added vertically until the height of the panel is received then a new column is added |
| Padding | Thickness  | Gets or sets the distance between the border and its child object |
| VerticalSpacing | double  | Gets or sets the vertical distance between items |

## Examples

The following example of adding WrapPanel Control.

```xaml
<Page ....
      xmlns:controls="using:CommunityToolkit.WinUI.Controls">

    <Grid Background="{StaticResource Brush-Grey-05}">
        <Grid.RowDefinitions>
            <RowDefinition />
            <RowDefinition />
        </Grid.RowDefinitions>
        <Grid>
            <Grid.RowDefinitions>
                <RowDefinition Height="50" />
                <RowDefinition />
            </Grid.RowDefinitions>
            <Button Name="HorizontalButton" Click="HorizontalButton_Click" Content="Add Horizontal Control" />
            <controls:WrapPanel Name="HorizontalWrapPanel" Grid.Row="1" Margin="2" />
        </Grid>

        <Grid Grid.Row="1">
            <Grid.RowDefinitions>
                <RowDefinition Height="50" />
                <RowDefinition />
            </Grid.RowDefinitions>
            <Button Name="VerticalButton" Click="VerticalButton_Click" Content="Add Vertical Control" />
            <controls:WrapPanel Name="VerticalWrapPanel" Grid.Row="1" Margin="2"
                                 VerticalSpacing="10" HorizontalSpacing="10" Orientation="Vertical" />
        </Grid>
    </Grid>
</Page>
```
