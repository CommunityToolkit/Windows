---
title: Attached Card Shadow
author: michael-hawker
description: Attach performant and easy to use shadows powered by Win2D.
keywords: shadow, shadows, dropshadow, dropshadowpanel, attachedshadow, attacheddropshadow, attachedcardshadow
dev_langs:
  - csharp
category: Extensions
subcategory: Shadows
discussion-id: 0
issue-id: 0
icon: Assets/Shadow.png
---

The `AttachedCardShadow` is the easiest to use and most performant shadow. It is recommended to use it where possible, if taking a Win2D dependency is not a concern. It's only drawbacks are the extra dependency required and that it only supports rectangular and rounded-rectangular geometries (as described in the table above).

The great benefit to the `AttachedCardShadow` is that no extra surface or element is required to add the shadow. This reduces the complexity required in development and allows shadows to easily be added at any point in the development process. It also supports transparent elements, without displaying the shadow behind them!

The example shows how easy it is to not only apply an `AttachedCardShadow` to an element, but use it in a style to apply to multiple elements as well:

> [!SAMPLE AttachedCardShadowBasicSample]

You can see the `AttachedCardShadow` defined as a resource so it can be shared across multiple components. It also supports binding/animations to update at runtime.

## Layer Ordering

There can be cases, especially direct usage on untemplated elements, where the AttachedCardShadow may require a parent element to create the desired effect, as seen in this example:

> [!SAMPLE AttachedCardShadowUntemplatedSample]
