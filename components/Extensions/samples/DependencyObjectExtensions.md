---
title: DependencyObjectExtensions
author: Sergio0694
description: The DependencyObjectExtensions type provides a collection of extensions methods for DependencyObject objects to aid in using the VisualTreeHelper class.
keywords: windows 10, uwp, windows community toolkit, uwp community toolkit, uwp toolkit, Visual Tree, extensions
dev_langs:
  - csharp
  - vb
category: Extensions
subcategory: Layout
discussion-id: 0
issue-id: 0
icon: Assets/Extensions.png
---

The `DependencyObjectExtensions` type provides a collection of extensions methods for [`DependencyObject`](https://learn.microsoft.com/uwp/api/windows.ui.xaml.dependencyobject) objects. This class exposes several APIs to aid in using the [`VisualTreeHelper`](https://learn.microsoft.com/uwp/api/Windows.UI.Xaml.Media.VisualTreeHelper) class. There are a number of reasons why walking the visual tree might be useful, which are mentioned [in the docs](https://learn.microsoft.com/uwp/api/windows.ui.xaml.media.visualtreehelper?#traversing-a-visual-tree).

## Syntax

```csharp
// Include the namespace to access extensions
using Microsoft.Toolkit.Uwp.UI;

// Find a visual descendant control using its name
var control = uiElement.FindDescendant("MyTextBox");

// Find the first visual descendant control of a specified type
control = uiElement.FindDescendant<ListView>();

// Find all visual descendant controls of the specified type.
// We use LINQ here to filter children of a specific type.
using System.Linq;

foreach (var child in uiElement.FindDescendants().OfType<ListViewItem>())
{
    // ...
}

// Find the first visual ascendant control using its name
control = uiElement.FindAscendant("MyScrollViewer");

// Find the first visual ascendant control of a specified type
control = uiElement.FindAscendant<ScrollViewer>();
```

## Examples

You can find more examples in the [unit tests](https://github.com/windows-toolkit/WindowsCommunityToolkit/tree/rel/7.1.0/UnitTests).
