---
title: ListViewExtensions
author: nmetulev
description: ListViewExtensions extensions provide a lightweight way to extend every control that inherits the ListViewBase class with attached properties.
keywords: ListViewBase, extensions
dev_langs:
  - csharp
category: Extensions
subcategory: Controls
discussion-id: 0
issue-id: 0
icon: Assets/Extensions.png
---

The `ListViewExtensions` class provide a lightweight way to extend every control that inherits the [`ListViewBase`](https://learn.microsoft.com/uwp/api/Windows.UI.Xaml.Controls.ListViewBase) class with attached properties. This means that all the extensions in this class can apply to both [`ListView`](https://learn.microsoft.com/uwp/api/windows.ui.xaml.controls.listview), [`GridView`](https://learn.microsoft.com/uwp/api/windows.ui.xaml.controls.gridview) and other controls.

## ListViewBase Extensions

- [AlternateColor](#alternatecolor)
- [AlternateItemTemplate](#alternateitemtemplate)
- [Command](#command)
- [StretchItemContainerDirection](#stretchitemcontainerdirection)
- [SmoothScrollIntoView](#smoothscrollintoview)

## AlternateColor

The `AlternateColor` property provides a way to assign a background color to every other item.

> [!WARNING]
> The [`ContainerContentChanging`](https://learn.microsoft.com/uwp/api/windows.ui.xaml.controls.listviewbase#Windows_UI_Xaml_Controls_ListViewBase_ContainerContentChanging) event used for this extension to work, will not be raised when the [`ItemsPanel`](https://learn.microsoft.com/uwp/api/windows.ui.xaml.controls.itemscontrol.itemspanel) is replaced with another type of panel than [`ItemsStackPanel`](https://learn.microsoft.com/uwp/api/windows.ui.xaml.controls.itemsstackpanel) or [`ItemsWrapGrid`](https://learn.microsoft.com/uwp/api/windows.ui.xaml.controls.itemswrapgrid).

Here is how this property can be used in XAML:

```xaml
<Page ...
     xmlns:ui="using:CommunityToolkit.WinUI">

<ListView
    ui:ListViewExtensions.AlternateColor="Silver"
    ItemsSource="{x:Bind MainViewModel.Items, Mode=OneWay}" />
```

## AlternateItemTemplate

The `AlternateItemTemplate` property provides a way to assign an alternate [`DataTemplate`](https://learn.microsoft.com/uwp/api/windows.ui.xaml.datatemplate) to every other item. It is also possible to combine with the `AlternateColor` property.

> [!WARNING]
> The [`ContainerContentChanging`](https://learn.microsoft.com/uwp/api/windows.ui.xaml.controls.listviewbase#Windows_UI_Xaml_Controls_ListViewBase_ContainerContentChanging) event used for this extension to work, will not be raised when the [`ItemsPanel`](https://learn.microsoft.com/uwp/api/windows.ui.xaml.controls.itemscontrol.itemspanel) is replaced with another type of panel than [`ItemsStackPanel`](https://learn.microsoft.com/uwp/api/windows.ui.xaml.controls.itemsstackpanel) or [`ItemsWrapGrid`](https://learn.microsoft.com/uwp/api/windows.ui.xaml.controls.itemswrapgrid).

Here is how this property can be used in XAML:

```xaml
<Page ...
     xmlns:ui="using:CommunityToolkit.WinUI">

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

`ListViewExtensions` provides extension method that allow attaching an [`ICommand`](https://learn.microsoft.com/uwp/api/Windows.UI.Xaml.Input.ICommand) to handle `ListViewBase` item interaction by means of [`ItemClick`](https://learn.microsoft.com/uwp/api/windows.ui.xaml.controls.listviewbase#Windows_UI_Xaml_Controls_ListViewBase_ItemClick) event.

> [!IMPORTANT]
> ListViewBase [`IsItemClickEnabled`](https://learn.microsoft.com/uwp/api/windows.ui.xaml.controls.listviewbase#Windows_UI_Xaml_Controls_ListViewBase_IsItemClickEnabled) must be set to `true`.

Here is how this property can be used in XAML:

```xaml
<Page ...
     xmlns:ui="using:CommunityToolkit.WinUI">
     
<ListView
    ui:ListViewExtensions.Command="{x:Bind MainViewModel.ItemSelectedCommand, Mode=OneWay}"
    IsItemClickEnabled="True"
    ItemsSource="{x:Bind MainViewModel.Items, Mode=OneWay}"
    SelectionMode="None" />
```

## StretchItemContainerDirection

The `ItemContainerStretchDirection` property provides a way to stretch the `ItemContainer` in horizontal, vertical or both ways. The allowed values are items from the `ItemContainerStretchDirection` type.

> [!WARNING]
> The [`ContainerContentChanging`](https://learn.microsoft.com/uwp/api/windows.ui.xaml.controls.listviewbase#Windows_UI_Xaml_Controls_ListViewBase_ContainerContentChanging) event used for this extension to work, will not be raised when the [`ItemsPanel`](https://learn.microsoft.com/uwp/api/windows.ui.xaml.controls.itemscontrol.itemspanel) is replaced with another type of panel than [`ItemsStackPanel`](https://learn.microsoft.com/uwp/api/windows.ui.xaml.controls.itemsstackpanel) or [`ItemsWrapGrid`](https://learn.microsoft.com/uwp/api/windows.ui.xaml.controls.itemswrapgrid).

Here is how this property can be used from XAML:

```xaml
<Page ...
     xmlns:ui="using:CommunityToolkit.WinUI">

<ListView
    ui:ListViewExtensions.StretchItemContainerDirection="Horizontal"
    ItemsSource="{x:Bind MainViewModel.Items, Mode=OneWay}" />
```

## SmoothScrollIntoView

Use SmoothScrollIntoView helps to scroll the item into the view with animation. Specify the ItemPosition property to align the item.

```csharp
// Scrolling with index
await MyGridView.SmoothScrollIntoViewWithIndexAsync(index: int, itemPlacement: ItemPlacement, disableAnimation: bool, scrollIfVisible: bool, additionalHorizontalOffset: int, additionalVerticalOffset: int);

// Scrolling with item
await MyGridView.SmoothScrollIntoViewWithItemAsync(item: object, itemPlacement: ItemPlacement, disableAnimation: bool, scrollIfVisible: bool, additionalHorizontalOffset: int, additionalVerticalOffset: int);
```

We can use this extension to make the selected item always centered:

> [!SAMPLE SmoothScrollIntoViewSample]
