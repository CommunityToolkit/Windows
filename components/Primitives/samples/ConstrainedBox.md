---
title: ConstrainedBox
author: michael-hawker
description: The ConstrainedBox is a FrameworkElement which can allow a developer to constrain the scale, multiple of, or aspect ratio of its content.
keywords: viewbox, content decorator, ConstrainedBox
dev_langs:
  - csharp
category: Controls
subcategory: Layout
discussion-id: 0
issue-id: 0
---

# ConstrainedBox

The [ConstrainedBox](/dotnet/api/microsoft.toolkit.uwp.ui.controls.constrainedbox) is a simple `FrameworkElement` content decorator control which allows the developer to constrain its child content by one or more various properties including aspect ratio, scale, and aligning to a multiple boundary.

> [!NOTE]
> For technical reasons this control inherits from `ContentPresenter`; however, it should be treated as a `FrameworkElement` and its border and template properties should not be used for compatibility in the future when it can inherit from FrameworkElement directly.

> **Platform APIs:** [`ConstrainedBox`](/dotnet/api/microsoft.toolkit.uwp.ui.controls.constrainedbox), [`AspectRatio`](/dotnet/api/microsoft.toolkit.uwp.ui.controls.aspectratio)

> [!SAMPLE ConstrainedBoxSample]

The three constraints provided by the `ConstrainedBox` control can be used individually & independently or combined to provide a wide-variety of responsive layout options. When used in combination, for the properties used, they are always applied in the following order:

1. `ScaleX`/`ScaleY`: Scaling is applied first to restrict the overall available size in each axis from the parent container based on a percentage value from 0.0-1.0. The default value is 1.0 to use all available size.

2. `MultipleX`/`MultipleY`: The multiple values allow a developer to snap the layout size of the child to a specific multiple value. For instance, by providing a value of 4, you would ensure the child element is closest to the size of 16, 20, 24, etc... The floor is taken so the child element is always smaller within the bounds of its parent. By default this value is not set so that no extra layout rounding occurs.

3. `AspectRatio`: The aspect ratio can be provided by a double value or a colon separated aspect (e.g. "16:9") and will restrict the layout of the child element to that available space. Therefore if you stretch your child element you can ensure it maintains the desired aspect ratio. By default, no aspect ratio constraint is applied.

If a `ConstrainedBox` is placed in a container which doesn't restrict its size in both the horizontal and vertical directions, it will try to determine its constraints based on the desired size of its child element. If only one direction has infinite size, the control will attempt to use the fixed dimension to measure all constraints against.

The Min/Max and Alignment properties of the `ConstrainedBox` itself and its child can also be set to provide other constraints on how layout is performed with the control, as with any regular XAML layout.

## Example

The following example shows how to align a 16:9 view of an image with the top of its container (like a page) and center on the image's content:

```xaml
  <controls:ConstrainedBox AspectRatio="16:9" VerticalAlignment="Top">
    <Image Source="/Assets/Photos/WestSeattleView.jpg"
           Stretch="UniformToFill"
           VerticalAlignment="Center"/> <!-- Center viewport on image -->
  </controls:ConstrainedBox>
```
