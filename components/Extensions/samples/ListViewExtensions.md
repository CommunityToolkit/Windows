---
title: ListViewExtensions
author: nmetulev
description: ListViewExtensions extensions provide a lightweight way to extend every control that inherits the ListViewBase class with attached properties.
keywords: windows 10, uwp, windows community toolkit, uwp community toolkit, uwp toolkit, ListViewBase, extensions
dev_langs:
  - csharp
category: Extensions
subcategory: Layout
discussion-id: 0
issue-id: 0
---

# ListViewExtensions

The [`ListViewExtensions`](/dotnet/api/microsoft.toolkit.uwp.ui.listviewextensions) class provide a lightweight way to extend every control that inherits the [`ListViewBase`](/uwp/api/Windows.UI.Xaml.Controls.ListViewBase) class with attached properties. This means that all the extensions in this class can apply to both [`ListView`](/uwp/api/windows.ui.xaml.controls.listview), [`GridView`](/uwp/api/windows.ui.xaml.controls.gridview) and other controls.

> **Platform APIs:** [`ListViewExtensions`](/dotnet/api/microsoft.toolkit.uwp.ui.listviewextensions), [`ItemContainerStretchDirection`](/dotnet/api/microsoft.toolkit.uwp.ui.ItemContainerStretchDirection)

> [!div class="nextstepaction"]
> [Try it in the sample app](uwpct://Extensions?sample=ListViewExtensions)

## ListViewBase Extensions

- [AlternateColor](#alternatecolor)
- [AlternateItemTemplate](#alternateitemtemplate)
- [Command](#command)
- [StretchItemContainerDirection](#stretchitemcontainerdirection)
- [SmoothScrollIntoView](#smoothscrollintoview)

## AlternateColor

The `AlternateColor` property provides a way to assign a background color to every other item.

> [!WARNING]
> The [`ContainerContentChanging`](/uwp/api/windows.ui.xaml.controls.listviewbase#Windows_UI_Xaml_Controls_ListViewBase_ContainerContentChanging) event used for this extension to work, will not be raised when the [`ItemsPanel`](/uwp/api/windows.ui.xaml.controls.itemscontrol.itemspanel) is replaced with another type of panel than [`ItemsStackPanel`](/uwp/api/windows.ui.xaml.controls.itemsstackpanel) or [`ItemsWrapGrid`](/uwp/api/windows.ui.xaml.controls.itemswrapgrid).

Here is how this property can be used in XAML:

```xaml
<Page ...
     xmlns:ui="using:Microsoft.Toolkit.Uwp.UI">

<ListView
    ui:ListViewExtensions.AlternateColor="Silver"
    ItemsSource="{x:Bind MainViewModel.Items, Mode=OneWay}" />
```

## AlternateItemTemplate

The `AlternateItemTemplate` property provides a way to assign an alternate [`DataTemplate`](/uwp/api/windows.ui.xaml.datatemplate) to every other item. It is also possible to combine with the `AlternateColor` property.

> [!WARNING]
> The [`ContainerContentChanging`](/uwp/api/windows.ui.xaml.controls.listviewbase#Windows_UI_Xaml_Controls_ListViewBase_ContainerContentChanging) event used for this extension to work, will not be raised when the [`ItemsPanel`](/uwp/api/windows.ui.xaml.controls.itemscontrol.itemspanel) is replaced with another type of panel than [`ItemsStackPanel`](/uwp/api/windows.ui.xaml.controls.itemsstackpanel) or [`ItemsWrapGrid`](/uwp/api/windows.ui.xaml.controls.itemswrapgrid).

Here is how this property can be used in XAML:

```xaml
<Page ...
     xmlns:ui="using:Microsoft.Toolkit.Uwp.UI">

<Page.Resources>
    <DataTemplate x:Name="NormalTemplate">
        <TextBlock Text="{Binding " Foreground="Green"></TextBlock>
    </DataTemplate>
    
    <DataTemplate x:Name="AlternateTemplate">
        <TextBlock Text="{Binding}" Foreground="Orange"></TextBlock>
    </DataTemplate>
</Page.Resources>

<ListView
    ItemTemplate="{StaticResource NormalTemplate}"
    ui:ListViewExtensions.AlternateItemTemplate="{StaticResource AlternateTemplate}"
    ItemsSource="{x:Bind MainViewModel.Items, Mode=OneWay}" />
```

## Command

`ListViewExtensions` provides extension method that allow attaching an [`ICommand`](/uwp/api/Windows.UI.Xaml.Input.ICommand) to handle `ListViewBase` item interaction by means of [`ItemClick`](/uwp/api/windows.ui.xaml.controls.listviewbase#Windows_UI_Xaml_Controls_ListViewBase_ItemClick) event.

> [!IMPORTANT]
> ListViewBase [`IsItemClickEnabled`](/uwp/api/windows.ui.xaml.controls.listviewbase#Windows_UI_Xaml_Controls_ListViewBase_IsItemClickEnabled) must be set to `true`.

Here is how this property can be used in XAML:

```xaml
<Page ...
     xmlns:ui="using:Microsoft.Toolkit.Uwp.UI">
     
<ListView
    ui:ListViewExtensions.Command="{x:Bind MainViewModel.ItemSelectedCommand, Mode=OneWay}"
    IsItemClickEnabled="True"
    ItemsSource="{x:Bind MainViewModel.Items, Mode=OneWay}"
    SelectionMode="None" />
```

## StretchItemContainerDirection

The `ItemContainerStretchDirection` property provides a way to stretch the `ItemContainer` in horizontal, vertical or both ways. The allowed values are items from the `ItemContainerStretchDirection` type.

> [!WARNING]
> The [`ContainerContentChanging`](/uwp/api/windows.ui.xaml.controls.listviewbase#Windows_UI_Xaml_Controls_ListViewBase_ContainerContentChanging) event used for this extension to work, will not be raised when the [`ItemsPanel`](/uwp/api/windows.ui.xaml.controls.itemscontrol.itemspanel) is replaced with another type of panel than [`ItemsStackPanel`](/uwp/api/windows.ui.xaml.controls.itemsstackpanel) or [`ItemsWrapGrid`](/uwp/api/windows.ui.xaml.controls.itemswrapgrid).

Here is how this property can be used from XAML:

```xaml
<Page ...
     xmlns:ui="using:Microsoft.Toolkit.Uwp.UI">

<ListView
    ui:ListViewExtensions.StretchItemContainerDirection="Horizontal"
    ItemsSource="{x:Bind MainViewModel.Items, Mode=OneWay}" />
```

## SmoothScrollIntoView

Use SmoothScrollIntoView helps to scroll the item into the view with animation. Specify the ItemPosition property to align the item.

### Syntax

```csharp
// Scrolling with index
await MyGridView.SmoothScrollIntoViewWithIndexAsync(index: int, itemPlacement: ItemPlacement, disableAnimation: bool, scrollIfVisibile: bool, additionalHorizontalOffset: int, additionalVerticalOffset: int);

// Scrolling with item
await MyGridView.SmoothScrollIntoViewWithItemAsync(item: object, itemPlacement: ItemPlacement, disableAnimation: bool, scrollIfVisibile: bool, additionalHorizontalOffset: int, additionalVerticalOffset: int);
```

```vb
' Scrolling with index
Await MyGridView.SmoothScrollIntoViewWithItemAsync(index:=Integer, itemPlacement:=ItemPlacement.Bottom, disableAnimation:=Boolean, scrollIfVisibile:=Boolean, additionalHorizontalOffset:=Integer, additionalVerticalOffset:=Integer)

' Scrolling with item
Await MyGridView.SmoothScrollIntoViewWithItemAsync(item:=Object, itemPlacement:=ItemPlacement.Bottom, disableAnimation:=Boolean, scrollIfVisibile:=Boolean, additionalHorizontalOffset:=Integer, additionalVerticalOffset:=Integer)
```

### Sample Output

![SmoothScrollIntoView Helper](../resources/images/Extensions/SmoothScrollIntoView.gif)

### Methods

| Methods | Return Type | Description |
| -- | -- | -- |
| SmoothScrollIntoViewWithIndexAsync(int, ScrollItemPlacement, bool, bool, int, int) | Task | Smooth scroll item into view With index number |
| SmoothScrollIntoViewWithItemAsync(object, ScrollItemPlacement, bool, bool, int, int) | Task | Smooth scroll item into view With item object |

### Method params

| Properties | Type | Description |
|------------|------|-------------|
| intex      | int  | Intex of the item to be scrolled. Index can be negative |
| item      | int  | Intex of the item to be scrolled |
| itemPosition | ScrollItemPlacement | Specify the position of the Item after scrolling |
| disableAnimation | bool | To disable the scrolling animation |
| scrollIfVisibile | bool | Set `true` to scroll even if the scroll to item is visible so that the item will be aligned depend upon `itemPosition` |
| additionalHorizontalOffset | bool | Give addition horizontal offset |
| additionalVerticalOffset | bool | Give addition vertical offset |

### ItemPosition

| ScrollItemPlacement | Description |
|--------------|-------------|
| Default | If visible then it will not scroll, if not then item will be aligned to the nearest edge |
| Left | Aligned left |
| Top | Aligned top |
| Center | Aligned center |
| Right | Aligned right |
| Bottom | Aligned bottom |

### Examples

- We can use this extension to make the selected item always centered.

    **Sample Code**

    ```xaml
    <ListView ItemsSource="{x:Bind itemSources}" SelectionChanged="ListView_SelectionChanged">
        <ListView.ItemTemplate>
            <DataTemplate>
                <!-- Your Template -->
            </DataTemplate>
        </ListView.ItemTemplate>
        <ListView.ItemsPanel>
            <ItemsPanelTemplate>
                <StackPanel Orientation="Horizontal"/>
            </ItemsPanelTemplate>
        </ListView.ItemsPanel>
    </ListView>
    ```

    ```csharp
    private async void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var listView = (sender as ListView);
        await listView.SmoothScrollIntoViewWithIndexAsync(listView.SelectedIndex, ScrollItemPlacement.Center, false, true);
    }
    ```

    ```vb
    Private Async Sub ListView_SelectionChanged(ByVal sender As Object, ByVal e As SelectionChangedEventArgs)
        Dim listView = (TryCast(sender, ListView))
        Await listView.SmoothScrollIntoViewWithIndexAsync(listView.SelectedIndex, ScrollItemPlacement.Center, False, True)
    End Sub
    ```

    **Sample Output**

    ![Use Case 1 Output](../resources/images/Extensions/SmoothScrollIntoView-CenterSelected.gif)

## Requirements

| Device family | Universal, 10.0.16299.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp.UI.Extensions |
| NuGet package | [Microsoft.Toolkit.Uwp.UI](https://www.nuget.org/packages/Microsoft.Toolkit.Uwp.UI/) |

## API

- [ListViewExtensions source code](https://github.com/windows-toolkit/WindowsCommunityToolkit/tree/rel/7.1.0/Microsoft.Toolkit.Uwp.UI/Extensions/ListViewBase)
