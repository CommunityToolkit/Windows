---
title: Mouse.Cursor attached property
author: martinsuchan
description: Mouse.Cursor attached property enables you to easily change the mouse cursor over specific Framework elements.
keywords: windows 10, uwp, windows community toolkit, uwp community toolkit, uwp toolkit, Mouse, cursor, extensions
---

# Mouse.Cursor attached property

> [!WARNING]
> This docs page is outdated, please refer to the new one for the [`FrameworkElementExtensions`](FrameworkElementExtensions.md) type.
The [Mouse.Cursor attached property](/dotnet/api/microsoft.toolkit.uwp.ui.extensions.mouse.cursor) enables you to easily change the mouse cursor over specific Framework elements.

> [!div class="nextstepaction"]
> [Try it in the sample app](uwpct://Extensions?sample=Mouse)

## Syntax

```xaml
<Page ...
     xmlns:extensions="using:Microsoft.Toolkit.Uwp.UI.Extensions">
<UIElement extensions:Mouse.Cursor="Hand"/>
```

## Properties

| Property | Type | Description |
| -- | -- | -- |
| Mouse.Cursor | [CoreCursorType](/uwp/api/Windows.UI.Core.CoreCursorType) | Set cursor type when mouse cursor over a Framework elements |

## Example

Here is a example of setting Mouse.Cursor

```xaml
<Page x:Class="Microsoft.Toolkit.Uwp.SampleApp.SamplePages.MouseCursorPage"
    xmlns="https://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="https://schemas.microsoft.com/winfx/2006/xaml"
    xmlns:extensions="using:Microsoft.Toolkit.Uwp.UI.Extensions">
    <Grid Background="{ThemeResource ApplicationPageBackgroundThemeBrush}">
        <Border extensions:Mouse.Cursor="Hand"
            Width="220" Height="120" Background="DeepSkyBlue"
            HorizontalAlignment="Center" VerticalAlignment="Center"/>
    </Grid>
</Page>
```

> [!NOTE]
> Even though Microsoft recommends in [UWP Design guidelines](/windows/uwp/input-and-devices/mouse-interactions#cursors) hover effects instead of custom cursors over interactive elements, custom cursors can be useful in some specific scenarios.

## Limitations

Because the UWP framework does not support metadata on Attached Properties, specifically the [FrameworkPropertyMetadata.Inherits](/dotnet/api/system.windows.frameworkpropertymetadata.-ctor#System_Windows_FrameworkPropertyMetadata__ctor_System_Object_System_Windows_FrameworkPropertyMetadataOptions_System_Windows_PropertyChangedCallback_System_Windows_CoerceValueCallback_) flag, the Mouse.Cursor might not work properly in some very specific XAML layout scenarios when combining nested FrameworkElements with different Mouse.Cursor values set on them.

## Sample Project

[Mouse Sample Page](https://github.com/windows-toolkit/WindowsCommunityToolkit/tree/rel/7.1.0/Microsoft.Toolkit.Uwp.SampleApp/SamplePages/Mouse). You can [see this in action](uwpct://Extensions?sample=Mouse) in the [Windows Community Toolkit Sample App](https://aka.ms/windowstoolkitapp).

## Requirements

| Device family | Universal, 10.0.16299.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp.Extensions |
| NuGet package | [Microsoft.Toolkit.Uwp.UI](https://www.nuget.org/packages/Microsoft.Toolkit.Uwp.UI/) |

## API

* [Mouse.Cursor source code](https://github.com/windows-toolkit/WindowsCommunityToolkit/blob/rel/7.1.0/Microsoft.Toolkit.Uwp.UI/Extensions/Mouse)
