---
title: Helpers
author: 
description: Various helpers to convert various formats from and to `Color`.
keywords: Helpers, Theming, theme listerner, themes, screenunithelper, colorhelper
dev_langs:
  - csharp
category: Helpers
subcategory: Layout
discussion-id: 0
issue-id: 0
---

```csharp
Color color = "#FFFF0000".ToColor();
Color color = "Red".ToColor();
string hex = Colors.Red.ToHex();
HslColor hsl = Colors.Red.ToHsl();
HsvColor hsv = Colors.Red.ToHsv();
int i = Colors.Red.ToInt();
```

## ScreenUnitHelper

The [ScreenUnitHelper](/dotnet/api/microsoft.toolkit.uwp.helpers.screenunithelper) helps to convert a screen unit to another screen unit (ex: 1cm => 39.7953px).

```csharp
float result = ScreenUnitHelper.Convert(ScreenUnit.Inch, ScreenUnit.Pixel, 1); // 96
```

## DesignTimeHelpers

The [DesignTimeHelpers](/dotnet/api/microsoft.toolkit.uwp.ui.designtimehelpers) helps to detect if your code is running in execution or designtime mode.

```csharp
if (DesignTimeHelpers.IsRunningInLegacyDesignerMode || DesignTimeHelpers.IsRunningInEnhancedDesignerMode)
{
  // Design time
}
```
