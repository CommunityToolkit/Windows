---
title: View extensions
author: nmetulev
description: The ApplicationViewExtensions and TitleBarExtensions types provide a declarative way of setting ApplicationView, CoreApplicationView and ApplicationViewTitleBar properties from XAML.
keywords: windows 10, uwp, windows community toolkit, uwp community toolkit, uwp toolkit, ViewExtensions, ApplicationViewExtensions, StatusBarExtensions, TitleBarExtensions, statusbar, titlebar, xaml
---

# View extensions

The [`ApplicationViewExtensions`](/dotnet/api/microsoft.toolkit.uwp.ui.applicationviewextensions) and [`TitleBarExtensions`](/dotnet/api/microsoft.toolkit.uwp.ui.titlebarextensions) provide a declarative way of setting [`ApplicationView`](/uwp/api/windows.ui.viewmanagement.applicationview), [`CoreApplicationView`](/uwp/api/windows.applicationmodel.core.coreapplicationview) and [`ApplicationViewTitleBar`](/uwp/api/windows.ui.viewmanagement.applicationviewtitlebar) properties from XAML.

> **Platform APIs:** [`ApplicationViewExtensions`](/dotnet/api/microsoft.toolkit.uwp.ui.applicationviewextensions), [`TitleBarExtensions`](/dotnet/api/microsoft.toolkit.uwp.ui.titlebarextensions)

## Example

These attached properties all target a [`Page`](/uwp/api/windows.ui.xaml.controls.page) element, so they can be used directly from XAML when declaring a control of this type in an application:

```xaml
<Page x:Class="Microsoft.Toolkit.Uwp.SampleApp.SamplePages.ViewExtensionsPage"
      xmlns="https://schemas.microsoft.com/winfx/2006/xaml/presentation"
      xmlns:x="https://schemas.microsoft.com/winfx/2006/xaml"
      xmlns:d="https://schemas.microsoft.com/expression/blend/2008"
      xmlns:local="using:Microsoft.Toolkit.Uwp.SampleApp.SamplePages"
      xmlns:mc="https://schemas.openxmlformats.org/markup-compatibility/2006"
      xmlns:ui="using:Microsoft.Toolkit.Uwp.UI"
      ui:ApplicationViewExtensions.Title="View Extensions"
      ui:ApplicationViewExtensions.ExtendViewIntoTitleBar="True"
      ui:TitleBarExtensions.BackgroundColor="Gray"
      ui:TitleBarExtensions.ButtonBackgroundColor="Orange"
      ui:TitleBarExtensions.ButtonForegroundColor="Black"
      ui:TitleBarExtensions.ButtonHoverBackgroundColor="DarkOrange"
      ui:TitleBarExtensions.ButtonHoverForegroundColor="Gray"
      ui:TitleBarExtensions.ButtonPressedForegroundColor="DarkGray"
      mc:Ignorable="d">

    <!-- Page content here -->
</Page>
```

## Examples

You can find more examples in the [unit tests](https://github.com/windows-toolkit/WindowsCommunityToolkit/tree/rel/7.1.0/UnitTests).
