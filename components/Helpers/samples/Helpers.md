---
title: Helpers
author: 
description: Various helpers to make conversions and other checks easier.
keywords: Helpers, Theming, theme listerner, themes, screenunithelper, colorhelper
dev_langs:
  - csharp
category: Helpers
subcategory: Developer
discussion-id: 0
issue-id: 0
icon: assets/icon.png
---

# ColorHelper

The [ColorHelper](/dotnet/api/microsoft.toolkit.uwp.helpers.colorhelper) helps to convert various formats from and to `Color`.

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

```csharp
float result = ScreenUnitHelper.Convert(ScreenUnit.Inch, ScreenUnit.Pixel, 1); // 96
```

# DesignTimeHelpers

The [DesignTimeHelpers](/dotnet/api/microsoft.toolkit.uwp.ui.designtimehelpers) helps to detect if your code is running in execution or designtime mode.

```csharp
if (DesignTimeHelpers.IsRunningInLegacyDesignerMode || DesignTimeHelpers.IsRunningInEnhancedDesignerMode)
{
  // Design time
}
```
