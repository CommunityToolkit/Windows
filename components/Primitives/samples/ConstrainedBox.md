---
title: ConstrainedBox
author: michael-hawker
description: The ConstrainedBox is a FrameworkElement which can allow a developer to constrain the aspect ratio, scale, or multiple boundary alignment of its content.
keywords: viewbox, content decorator, ConstrainedBox
dev_langs:
  - csharp
category: Layouts
subcategory: Media
discussion-id: 0
issue-id: 0
icon: Assets/ConstrainedBox.png
---

> [!NOTE]
> For technical reasons this control inherits from `ContentPresenter`; however, it should be treated as a `FrameworkElement` and its border and template properties should not be used for compatibility in the future when it can inherit from FrameworkElement directly.

> **Platform APIs:** `ConstrainedBox`, `AspectRatio`

The three constraints provided by the `ConstrainedBox` control can be used individually & independently or combined to provide a wide-variety of responsive layout options. When used in combination, for the properties used, they are always applied in the following order:

1. `ScaleX`/`ScaleY`: Scaling is applied first to restrict the overall available size in each axis from the parent container based on a percentage value from 0.0-1.0. The default value is 1.0 to use all available size.

2. `MultipleX`/`MultipleY`: The multiple values allow a developer to snap the layout size of the child to a specific multiple value. For instance, by providing a value of 4, you would ensure the child element is closest to the size of 16, 20, 24, etc... The floor is taken so the child element is always smaller within the bounds of its parent. By default this value is not set so that no extra layout rounding occurs.

3. `AspectRatio`: The aspect ratio can be provided by a double value or a colon separated aspect (e.g. "16:9") and will restrict the layout of the child element to that available space. Therefore if you stretch your child element you can ensure it maintains the desired aspect ratio. By default, no aspect ratio constraint is applied.

If a `ConstrainedBox` is placed in a container which doesn't restrict its size in both the horizontal and vertical directions, it will try to determine its constraints based on the desired size of its child element. If only one direction has infinite size, the control will attempt to use the fixed dimension to measure all constraints against.

The Min/Max and Alignment properties of the `ConstrainedBox` itself and its child can also be set to provide other constraints on how layout is performed with the control, as with any regular XAML layout.

## Aspect Ratios

The most common use-case for the `ConstrainedBox` is to maintain the aspect ratio of an image. For instance the following example maintains a 16:3 aspect ratio of the image at the top of its container (like a page) and center on the image's content:

> [!SAMPLE ConstrainedBoxAspectSample]

## Scaling

Another scenario may be for keeping a 'safe' margin around the content on your page. You may want this to not be a fixed margin but be proportional to a viewport.

This sample demonstrates that using `ScaleX`/`ScaleY`:

> [!SAMPLE ConstrainedBoxScaleSample]

## Multiples

The next sample shows how you can use the `MultipleX` property to snap the size of a component to the pattern of an image:

> [!SAMPLE ConstrainedBoxMultipleSample]
