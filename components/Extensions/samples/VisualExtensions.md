---
title: Composition Visual Extensions
author: nmetulev
description: The Composition Visual Attached Properties Extension allow Composition Visual Properties to be modified directly in XAML
keywords: windows 10, uwp, windows community toolkit, uwp community toolkit, uwp toolkit, Visual, composition, xaml, attached property
dev_langs:
  - csharp
category: Extensions
subcategory: Layout
discussion-id: 0
issue-id: 0
---

# Composition Visual Attached Properties Extension

The [`VisualExtensions`](/dotnet/api/microsoft.toolkit.uwp.ui.VisualExtensions) type allows developers to modify common properties of the [`Visual`](/uwp/api/Windows.UI.Composition.Visual) object of an element directly in XAML.

> **Platform APIs:** [`VisualExtensions`](/dotnet/api/microsoft.toolkit.uwp.ui.VisualExtensions)

## Syntax

Here is an example of how the `VisualExtensions` type can be used to directly set values for some `Visual` properties for a given UI element directly from XAML:

```xaml
<Page ...
    xmlns:ui="using:Microsoft.Toolkit.Uwp.UI">

<Border
    Height="100"
    Width="100"
    Background="Purple"
    ui:VisualExtensions.CenterPoint="50,50,0"
    ui:VisualExtensions.Opacity="0.5"
    ui:VisualExtensions.RotationAngleInDegrees="80"
    ui:VisualExtensions.Scale="2, 0.5, 1"
    ui:VisualExtensions.NormalizedCenterPoint="0.5, 0.5" />
```

> [!NOTE]
> The `NormalizedCenterPoint` will also use a [Composition Expression animation](/uwp/api/windows.ui.composition.expressionanimation) behind the scenes to ensure the center point value being set is kept in sync with the size of the associated `Visual` object. As with all composition animations, this animation runs on the compositor thread and doesn't add any load to the UI thread of the application.

## Examples

You can find more examples in the [unit tests](https://github.com/windows-toolkit/WindowsCommunityToolkit/tree/rel/7.1.0/UnitTests).
