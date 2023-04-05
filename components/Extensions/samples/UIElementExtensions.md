---
title: UIElementExtensions
author: vgromfeld
description: UIElementExtensions provides a simple way to extend the UIElement class
keywords: windows 10, uwp, windows community toolkit, uwp community toolkit, uwp toolkit, UIElement, extensions
dev_langs:
  - csharp
category: Extensions
subcategory: Layout
discussion-id: 0
issue-id: 0
---

# UIElementExtensions

The [`UIElementExtensions`](/dotnet/api/microsoft.toolkit.uwp.ui.extensions.uielementextensions) provide helpers and attached properties for the [`UIElement`](/uwp/api/windows.ui.xaml.uielement) class.

> **Platform APIs:** [`UIElementExtensions`](/dotnet/api/microsoft.toolkit.uwp.ui.extensions.uielementextensions)

## ClipToBounds

The `ClipToBounds` property allows you to indicate whether to clip the content of this element (or content coming from the child elements of this element) to fit into the size of the containing element. Here is how it can be used in XAML to clip a target element:

> [!Sample ClipToBoundsSample]

## Examples

You can find more examples in the [unit tests](https://github.com/windows-toolkit/WindowsCommunityToolkit/tree/rel/7.1.0/UnitTests).
