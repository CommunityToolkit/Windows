---
title: ListViewBase extensions
author: nmetulev
ms.date: 08/20/2017
description: ListViewBase extensions provide a lightweight way to extend every control that inherits the ListViewBase class with attached properties.
keywords: windows 10, uwp, uwp community toolkit, uwp toolkit, ListViewBase, extensions
---

# ListViewBase extensions

> [!WARNING]
> This docs page is outdated, please refer to the new one for the [`ListViewExtensions`](ListViewExtensions.md) type.
ListViewBase extensions provide a lightweight way to extend every control that inherits the <a href="/uwp/api/windows.ui.xaml.controls.listviewbase" target="_blank">ListViewBase</a> class with attached properties.

## AlternateColor

The AlternateColor property provides a way to assign a background color to every other item.

> The <a href="/uwp/api/windows.ui.xaml.controls.listviewbase#Windows_UI_Xaml_Controls_ListViewBase_ContainerContentChanging" target="_blank">ContainerContentChanging</a> event used for this extension to work, will not be raised when the ItemsPanel is replaced with another type of panel than ItemsStackPanel or ItemsWrapGrid.

### Example

```xaml
    <ListView
        extensions:ListViewBase.AlternateColor="Silver"
        ItemsSource="{x:Bind MainViewModel.Items, Mode=OneWay}" />
```

## AlternateItemTemplate

The AlternateItemTemplate property provides a way to assign an alternate <a href="/uwp/api/windows.ui.xaml.datatemplate" target="_blank">datatemplate</a> to every other item. It is also possible to combine with the AlternateColor property.

> The <a href="/uwp/api/windows.ui.xaml.controls.listviewbase#Windows_UI_Xaml_Controls_ListViewBase_ContainerContentChanging" target="_blank">ContainerContentChanging</a> event used for this extension to work, will not be raised when the ItemsPanel is replaced with another type of panel than ItemsStackPanel or ItemsWrapGrid.

### Example

```xaml
    <Page.Resources>
        <DataTemplate x:Name="NormalTemplate">
            <TextBlock Text="{Binding }" Foreground="Green"></TextBlock>
        </DataTemplate>
        
        <DataTemplate x:Name="AlternateTemplate">
            <TextBlock Text="{Binding }" Foreground="Orange"></TextBlock>
        </DataTemplate>
    </Page.Resources>
    <ListView
        ItemTemplate="{StaticResource NormalTemplate}"
        extensions:ListViewBase.AlternateItemTemplate="{StaticResource AlternateTemplate}"
        ItemsSource="{x:Bind MainViewModel.Items, Mode=OneWay}" />
```

## StretchItemContainerDirection

The StretchItemContainerDirection property provides a way to stretch the ItemContainer in horizontal, vertical or both ways. Possible values for this property are **Horizontal**, **Vertical** and **Both**.

> The <a href="/uwp/api/windows.ui.xaml.controls.listviewbase#Windows_UI_Xaml_Controls_ListViewBase_ContainerContentChanging" target="_blank">ContainerContentChanging</a> event used for this extension to work, will not be raised when the ItemsPanel is replaced with another type of panel than ItemsStackPanel or ItemsWrapGrid.

### Example

```xaml
    <ListView
        extensions:ListViewBase.StretchItemContainerDirection="Horizontal"
        ItemsSource="{x:Bind MainViewModel.Items, Mode=OneWay}" />
```

## Requirements (Windows 10 Device Family)

| [Device family](/windows/uwp/get-started/universal-application-platform-guide) | Universal, 10.0.14393.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp.UI.Extensions |

## API

* [ListViewBase source code](https://github.com/windows-toolkit/WindowsCommunityToolkit/blob/rel/7.1.0/Microsoft.Toolkit.Uwp.UI/Extensions/ListViewBase)
