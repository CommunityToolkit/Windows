---
title: ColorHelper
author: niels9001
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

The `ColorHelper` contains various methods for color parsing and color manipulation for `Windows.UI.Colors`

## Color parsing 

The `ColorHelper` contains various methods for parsing colors in specific formats, or simply `ParseColor` can be used to auto-detect the format.

Each parsing method has both a "Parse" and "TryParse" pattern version .

```csharp
// Parse by hex string
Color color = ColorHelper.ParseHexColor("#FFFF0000");

// Parse by hsl string
ColorHelper.ParseHslColor("hsl(0, 1, 0)");

// Try parse by name
ColorHelper.TryParseColorName("Red", out Color color);
```

## HSL/HSV Manipulation

The `ColorHelper` package includes methods for manipulating colors in the HSL or HSV color space.

```csharp
// Adjust a color's hue (to blue)
color = color.WithHue(240);

// Adjust a color's saturation (to fully saturated)
color = color.WithSaturation(1);
```

The package also includes models to store the color as either a HSV or HSL color.

A `Windows.UI.Color` can be converted to an a `HsvColor` or `HslColor` either by using the `.ToHsv()` and `.ToHsl()` methods, or by using an explicit cast on a color.

`HslColor` and `HsvColor` will implicity cast back to a `Windows.UI.Color` when needed.

```csharp
// Convert to hsl
Color color;
var hslColor = (HslColor)color;

// new SolidColorBrush takes a Color, but the HslColor can be implicitly cast to match
var brush = new SolidColorBrush(hslColor);
```
