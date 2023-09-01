---
title: Icon extensions
author: sergio0694
description: The FontIcon, FontIconSource and BitmapIcon markup extensions allow developers to easily declare these types of icons directly from XAML in a compact manner.
keywords: markup extension, XAML, markup, fonticon, fonticonsource, bitmapicon
dev_langs:
  - csharp
category: Extensions
subcategory: Markup
discussion-id: 0
issue-id: 0
icon: Assets/Extensions.png
---

The icon extensions are a group of markup extensions meant to simplify the creation of various icon types (specifically [`BitmapIcon`](https://learn.microsoft.com/uwp/api/Windows.UI.Xaml.Controls.BitmapIcon), [`BitmapIconSource`](https://learn.microsoft.com/uwp/api/Windows.UI.Xaml.Controls.BitmapIconSource), [`FontIcon`](https://learn.microsoft.com/uwp/api/Windows.UI.Xaml.Controls.FontIcon), [`FontIconSource`](https://learn.microsoft.com/uwp/api/Windows.UI.Xaml.Controls.FontIconSource), [`SymbolIcon`](https://learn.microsoft.com/uwp/api/Windows.UI.Xaml.Controls.SymbolIcon), and [`SymbolIconSource`](https://learn.microsoft.com/uwp/api/Windows.UI.Xaml.Controls.SymbolIconSource)) used across a variety of XAML controls. Using these extensions doesn't enable new capabilities per se, but it greatly simplifies the XAML syntax needed to create instances of these icon types.

## BitmapIconExtension

The `BitmapIconExtension` markup extension is similar in structure to the two previous extensions, but it produces `BitmapIcon` instances instead of font-based icons. Here is how it can be used:

```xaml
<MenuFlyout xmlns:ui="using:CommunityToolkit.WinUI">

    <!--Before-->
    <MenuFlyoutItem Text="Click me!">
        <MenuFlyoutItem.Icon>
            <BitmapIcon Source="/Assets/myicon.png"/>
        </MenuFlyoutItem.Icon>
    </MenuFlyoutItem>

    <!--After-->
    <MenuFlyoutItem
        Text="No, click me!"
        Icon="{ui:BitmapIcon Source=/Assets/myicon.png}" />
</MenuFlyout>
```

## BitmapIconSourceExtension

The `BitmapIconSourceExtension` class mirrors the `BitmapIconExtension` type, with the only difference being that it returns a `BitmapIconSource` instance. Here is how it can be used:

```xaml
<SwipeItems
    xmlns:ui="using:CommunityToolkit.WinUI"
    Mode="Reveal">
    <SwipeItem Text="Send" IconSource="{ui:BitmapIconSource Source=/Assets/myicon.png}"/>
</SwipeItems>
```

## FontIconExtension

The `FontIconExtension` type provides the ability to create `FontIcon` instances from XAML with a more compact representation than by explicitly creating a new `FontIcon` object to assign to the target property. The property also maps all the available `FontIcon` properties, so the two APIs expose the same set of customization options, just through a different XAML syntax:

```xaml
<CommandBar xmlns:ui="using:CommunityToolkit.WinUI">

    <!--Before-->
    <AppBarButton>
        <AppBarButton.Icon>
            <FontIcon Glyph="&#xE102;" FontFamily="{ThemeResource SymbolThemeFontFamily}"/>
        </AppBarButton.Icon>
    </AppBarButton>

    <!--After-->
    <AppBarButton Icon="{ui:FontIcon Glyph=&#xE102;}"/>
</CommandBar>
```

## FontIconSourceExtension

The `FontIconSourceExtension` class mirrors the `FontIconExtension` type, but producing `FontIconSource` instances instead of `FontIcon`:

```xaml
<SwipeItems
    xmlns:ui="using:CommunityToolkit.WinUI"
    Mode="Reveal">

    <!--Before-->
    <SwipeItem Text="Accept">
        <SwipeItem.IconSource>
            <FontIconSource Glyph="&#xE10B;"/>
        </SwipeItem.IconSource>
    </SwipeItem>
    
    <!--After-->
    <SwipeItem Text="Accept" IconSource="{ui:FontIconSource Glyph=&#xE10B;}"/>
</SwipeItems>
```

## SymbolIconExtension

The `SymbolIconExtension` type mirrors the `FontIcon` markup extension, with the main difference being that it uses a [`Symbol`](https://learn.microsoft.com/uwp/api/windows.ui.xaml.controls.symbol) value to specify the icon. All the other properties from `FontIconExtension` are available, with the exception of the font family, which is always set to "Segoe MDL2 Assets". Here is how it can be used:

```xaml
<CommandBar xmlns:ui="using:CommunityToolkit.WinUI">

    <!--Before-->
    <AppBarButton>
        <AppBarButton.Icon>
            <SymbolIcon Symbol="Play"/>
        </AppBarButton.Icon>
    </AppBarButton>

    <!--After-->
    <AppBarButton Icon="{ui:SymbolIcon Symbol=Play}"/>
</CommandBar>
```

> [!NOTE]
> The `SymbolIconExtension` actually returns a `FontIcon` value instead of a `SymbolIcon` one. This is done to include the additional properties (eg. `FontSize`, `FontWeight`, etc.) that would otherwise not have been available. When those are not modified, the look of the resulting icon will still be the same as the one that would've resulted from the use of a `SymbolIcon` instance.

## SymbolIconSource

The `SymbolIconSourceExtension` type is an alternative for `FontIconSourceExtension` that takes a `Symbol` value instead of a text, and displays the icon with the "Segoe MDL2 Assets". It's equivalent to the `SymbolIconExtension` type, except for the fact that it returns a [`FontIconSource`](https://learn.microsoft.com/uwp/api/windows.ui.xaml.controls.fonticonsource) instance:

```xaml
<SwipeItems
    xmlns:ui="using:CommunityToolkit.WinUI"
    Mode="Reveal">
    <SwipeItem Text="Play" IconSource="{ui:SymbolIconSource Symbol=Play}"/>
</SwipeItems>
```

## Remarks

All the values returned by these markup extensions belong to the `Windows.UI.Xaml.*` namespace. This means that they will only work properly when used with controls from that namespace, and not from `Microsoft.UI.Xaml.*` (the WinUI namespace). For instance, trying to use the `FontIconSourceExtension` to set the `IconSource` property on the `Microsoft.UI.Xaml.Controls.SwipeItems` will not work correctly, as the extension will produce a `Windows.UI.Xaml.Controls.FontIconSource` value instead of a `Microsoft.UI.Xaml.Controls.FontIconSource` one. When working with WinUI controls, you'll need to manually declare the icons you need with the explicit XAML syntax.

## Examples

You can find more examples in the [unit tests](https://github.com/windows-toolkit/WindowsCommunityToolkit/tree/rel/7.1.0/UnitTests).
