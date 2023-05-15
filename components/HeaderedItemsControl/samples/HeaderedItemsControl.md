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
---

# HeaderedItemsControl

The [HeaderedItemsControl](/dotnet/api/microsoft.toolkit.uwp.ui.controls.headereditemscontrol) is a UI control that allows content to be displayed with a specified header. The `Header` property can be any object and you can use the `HeaderTemplate` to specify a custom look to the header.

> [!NOTE]
> Setting the `Background`, `BorderBrush` and `BorderThickness` properties will not have any effect on the HeaderedItemsControl. This is to maintain the same functionality as the ItemsControl.

> [!Sample HeaderedItemsControlSample]

## Syntax

```xaml
<Page ...
     xmlns:controls="using:Microsoft.Toolkit.Uwp.UI.Controls"/>

<controls:HeaderedItemsControl>
    <!-- Header content or HeaderTemplate content -->
</controls:HeaderedItemsControl>
```


## Properties

| Property | Type | Gets or sets the data used for the header of each control |
| -- | -- | -- |
| Header | object | Gets or sets the data used for the header of each control |
| HeaderTemplate | DataTemplate | Gets or sets the template used to display the content of the control's header |
| Orientation | Orientation | Gets or sets the Orientation to use for layout of the header. If set to Vertical the Header will be above the items. If set to Horizontal the Header will be to the left of the items. |

### Examples

- The `Header` property can be set to a string, or any xaml elements. If binding the `Header` to an object that is not a string, use the `HeaderTemplate` to control how the content is rendered.

    *Sample Code*

    ```xaml
    <controls:HeaderedItemsControl Header="This is the header!"/>

    <controls:HeaderedItemsControl>
        <controls:HeaderedItemsControl.Header>
            <Border Background="Gray">
                <TextBlock Text="This is the header!" FontSize="16">
            </Border>
        </controls:HeaderedItemsControl.Header>
    </controls:HeaderedItemsControl>
    ```

- Used to control the look of the header. The default value for the `HeaderTemplate` will display the string representation of the `Header`. Set this property if you need to bind the `Header` to an object.

    *Sample Code*

    ```xaml
    <controls:HeaderedItemsControl Header="{Binding CustomObject}">
        <controls:HeaderedItemsControl.HeaderTemplate>
            <DataTemplate>
                <TextBlock Text="{Binding Title}">
            </DataTemplate>
        </controls:HeaderedItemsControl.HeaderTemplate>
    </controls:HeaderedItemsControl>
    ```
