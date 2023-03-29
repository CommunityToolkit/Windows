---
title: Array Extensions
author: michael-hawker
description: Learn about array extension methods from the community toolkit. See code examples and view requirements.
keywords: windows 10, uwp, uwp community toolkit, uwp toolkit, Extensions, array
dev_langs:
  - csharp
category: Extensions
subcategory: Math
discussion-id: 0
issue-id: 0
---

# Array Extensions

> [!WARNING]
> These extensions have been partially moved to the `Microsoft.Toolkit.HighPerformance` package. Please refer to the .NET API browser for a comprehensive list of available APIs.

Provides a few helpers for dealing with multidimensional and jagged arrays. Also, provides string helpers for debug output.

## Syntax

```csharp
using Microsoft.Toolkit.Extensions;

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

## Methods

| Methods | Return Type | Description |
| -- | -- | -- |
| Fill | void | Fills elements of a rectangular array at the given position and size to a specific value. |
| GetRow | IEnumerable | Yields a row from a rectangular array. |
| GetColumn | IEnumerable | Yields a column from a rectangular or jagged array. |
| ToArrayString | string | Returns a simple string representation of an array. |

## Requirements (Windows 10 Device Family)

| [Device family](/windows/uwp/get-started/universal-application-platform-guide) | Universal, 10.0.16299.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit |
| NuGet package | [Microsoft.Toolkit](https://www.nuget.org/packages/Microsoft.Toolkit/) |

The Array Extensions supports .NET Standard

## API

* [ArrayExtensions source code](https://github.com/windows-toolkit/WindowsCommunityToolkit/blob/rel/7.1.0/Microsoft.Toolkit/Extensions/ArrayExtensions.cs)
