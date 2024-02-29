---
title: DesignTimeHelpers
author: 
description: Class used to provide helpers for design time.
keywords: Helpers, designtimehelpers,
dev_langs:
  - csharp
category: Helpers
subcategory: Developer
discussion-id: 0
issue-id: 0
icon: Assets/ColorHelper.png
---

The `DesignTimeHelpers` helps to detect if your code is running in execution or designtime mode.

```csharp
if (DesignTimeHelpers.IsRunningInLegacyDesignerMode || DesignTimeHelpers.IsRunningInEnhancedDesignerMode)
{
  // Design time
}
```
