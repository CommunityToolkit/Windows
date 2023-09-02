---
title: ColorHelper
author: 
description: Convert colors from text names, HTML hex, HSV, or HSL to Windows UI Colors (and back again).
keywords: Helpers, Theming, theme listener, themes, screenunithelper, colorhelper
dev_langs:
  - csharp
category: Helpers
subcategory: Converters
discussion-id: 0
issue-id: 0
icon: Assets/ColorHelper.png
---

The `ColorHelper` can convert various formats (text names, HTML hex, HSV and HSL) to `Windows.UI.Colors` and back. Some examples:

```csharp
Color color = "#FFFF0000".ToColor();
Color color = "Red".ToColor();
string hex = Colors.Red.ToHex();
HslColor hsl = Colors.Red.ToHsl();
HsvColor hsv = Colors.Red.ToHsv();
int i = Colors.Red.ToInt();
```
