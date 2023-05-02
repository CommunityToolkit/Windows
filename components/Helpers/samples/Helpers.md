---
title: Helpers
author: 
description: Various helpers to make conversions and other checks easier.
keywords: Helpers, Theming, theme listerner, themes, screenunithelper, colorhelper
dev_langs:
  - csharp
category: Helpers
subcategory: Layout
discussion-id: 0
issue-id: 0
---

## Syntax

```csharp
if (DesignTimeHelpers.IsRunningInLegacyDesignerMode || DesignTimeHelpers.IsRunningInEnhancedDesignerMode)
{
  // Design time
}
```

# ColorHelper

The [ColorHelper](/dotnet/api/microsoft.toolkit.uwp.helpers.colorhelper) helps to convert various formats from and to `Color`.

## Syntax

```csharp
Color color = "#FFFF0000".ToColor();
Color color = "Red".ToColor();
string hex = Colors.Red.ToHex();
HslColor hsl = Colors.Red.ToHsl();
HsvColor hsv = Colors.Red.ToHsv();
int i = Colors.Red.ToInt();
```

# ScreenUnitHelper

The [ScreenUnitHelper](/dotnet/api/microsoft.toolkit.uwp.helpers.screenunithelper) helps to convert a screen unit to another screen unit (ex: 1cm => 39.7953px).

## Syntax

```csharp
float result = ScreenUnitHelper.Convert(ScreenUnit.Inch, ScreenUnit.Pixel, 1); // 96
```

# DesignTimeHelpers

The [DesignTimeHelpers](/dotnet/api/microsoft.toolkit.uwp.ui.designtimehelpers) helps to convert a screen unit to another screen unit (ex: 1cm => 37.7953px).
