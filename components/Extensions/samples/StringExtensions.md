---
title: String Extensions
author: avknaidu
description: Learn about string extension methods from the community toolkit. See code examples, requirements, and API information.

keywords: windows 10, uwp, windows community toolkit, uwp community toolkit, uwp toolkit, Extensions, string
dev_langs:
  - csharp
category: Extensions
subcategory: Input
discussion-id: 0
issue-id: 0
---

# String Extensions

The [`StringExtensions`](/dotnet/api/microsoft.toolkit.StringExtensions) type contains helpers and extensions for `string` objects, including validation methods for common scenarios.

> **Platform APIs:** [`StringExtensions`](/dotnet/api/microsoft.toolkit.StringExtensions)

## Syntax

```csharp
using Microsoft.Toolkit;

string str = "test@test.com";
bool isvalid = str.IsEmail();    //Returns true

string str = "123+888";
bool isvalid = str.IsDecimal();  //Returns false
```

### Formats Supported for **IsPhoneNumber** Extension

```
(987) 654-3210
(987)654-3210
987-654-3210
9876543210
+1 9876543210
001 9876543210
001 987-654-3210
19876543210
1-987-654-3210
```

## Examples

You can find more examples in the [unit tests](https://github.com/windows-toolkit/WindowsCommunityToolkit/tree/rel/7.1.0/UnitTests).
