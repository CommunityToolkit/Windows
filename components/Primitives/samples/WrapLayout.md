---
title: WrapLayout
author: skendrot
description: The WrapLayout virtualizes child elements in sequential position from left to right, breaking content to the next line at the edge of the containing box.
keywords: WrapLayout
dev_langs:
  - csharp
category: Layouts
subcategory: Layout
discussion-id: 0
issue-id: 0
icon: Assets/WrapLayout.png
---

Subsequent ordering happens sequentially from top to bottom or from right to left, depending on the value of the Orientation property.

The WrapLayout positions child controls based on orientation, horizontal orientation (default) positions controls from left to right and vertical orientation positions controls from top to bottom, and once the max-width or height is reached the control automatically creates rows or columns based on the orientation.

Spacing can be automatically added between items using the HorizontalSpacing and VerticalSpacing properties. When the Orientation is Horizontal, HorizontalSpacing adds uniform horizontal spacing between each individual item, and VerticalSpacing adds uniform spacing between each row of items.

When the Orientation is Vertical, HorizontalSpacing adds uniform spacing between each column of items, and VerticalSpacing adds uniform vertical spacing between individual items.

> [!SAMPLE WrapLayoutSample]


## Examples

The following example of adding the WrapLayout layout panel.

```xaml
<Page ....
      xmlns:controls="using:CommunityToolkit.WinUI.Controls"
      xmlns:winui="using:Microsoft.UI.Xaml.Controls">

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
            <winui:ItemsRepeater ItemsSource="{x:Bind _horizontalItems}" Grid.Row="1" Margin="2">
                <winui:ItemsRepeater.Layout>
                    <controls:WrapLayout VerticalSpacing="10"
                                         HorizontalSpacing="10"/>
                </winui:ItemsRepeater.Layout>
            </winui:ItemsRepeater>
        </Grid>

        <Grid Grid.Row="1">
            <Grid.RowDefinitions>
                <RowDefinition Height="50" />
                <RowDefinition />
            </Grid.RowDefinitions>
            <Button Name="VerticalButton" Click="VerticalButton_Click" Content="Add Vertical Control" />
            <winui:ItemsRepeater ItemsSource="{x:Bind _verticalItems}" Grid.Row="1" Margin="2">
                <winui:ItemsRepeater.Layout>
                    <controls:WrapLayout Orientation="Vertical" 
                                         VerticalSpacing="10"
                                         HorizontalSpacing="10"/>
                </winui:ItemsRepeater.Layout>
            </winui:ItemsRepeater>
        </Grid>
    </Grid>
</Page>
```
