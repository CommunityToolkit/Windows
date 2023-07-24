---
title: ScreenUnitHelper
author: 
description: Convert screen units to another screen unit.
keywords: Helpers, screenunithelper,
dev_langs:
  - csharp
category: Helpers
subcategory: System
discussion-id: 0
issue-id: 0
icon: Assets/ColorHelper.png
---

The [ScreenUnitHelper](/dotnet/api/microsoft.toolkit.uwp.helpers.screenunithelper) helps to convert a screen unit to another screen unit (ex: 1cm => 39.7953px).

```csharp
float result = ScreenUnitHelper.Convert(ScreenUnit.Inch, ScreenUnit.Pixel, 1); // 96
```
