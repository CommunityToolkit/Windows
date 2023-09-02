---
title: ScreenUnitHelper
author: 
description: Convert screen units to another screen unit.
keywords: Helpers, screenunithelper,
dev_langs:
  - csharp
category: Helpers
subcategory: Converters
discussion-id: 0
issue-id: 0
icon: Assets/ScreenUnitHelper.png
---

The `ScreenUnitHelper` helps to convert a screen unit to another screen unit (ex: 1cm => 39.7953px).

```csharp
float result = ScreenUnitHelper.Convert(ScreenUnit.Inch, ScreenUnit.Pixel, 1); // 96
```
