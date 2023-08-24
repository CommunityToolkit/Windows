---
title: Array Extensions
author: michael-hawker
description: Provides a few helpers for dealing with multidimensional and jagged arrays. Also, provides string helpers for debug output.
keywords: Extensions, array
dev_langs:
  - csharp
category: Extensions
subcategory: Math
discussion-id: 0
issue-id: 0
icon: Assets/Extensions.png
---

> [!WARNING]
> These extensions have been partially moved to the `Microsoft.Toolkit.HighPerformance` package. Please refer to the .NET API browser for a comprehensive list of available APIs.

Provides a few helpers for dealing with multidimensional and jagged arrays. Also, provides string helpers for debug output.

## Syntax

```csharp
using CommunityToolkit.WinUI.Extensions;

bool[,] inside = new bool[4, 5];

// Fill the inside of the boolean array with 'true' values.
inside.Fill(true, 1, 1, 3, 2);

Debug.WriteLine(inside.ToArrayString());

/*
Output:
[[False, False, False, False, False],
 [False, True,  True,  True,  False],
 [False, True,  True,  True,  False],
 [False, False, False, False, False]]
 */
```
