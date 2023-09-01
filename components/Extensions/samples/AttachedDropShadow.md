---
title: Attached Drop Shadow
author: michael-hawker
description: Allows many elements to share a common backdrop for casting shadows.
keywords: shadow, shadows, dropshadow, dropshadowpanel, attachedshadow, attacheddropshadow, attachedcardshadow
dev_langs:
  - csharp
category: Extensions
subcategory: Shadows
discussion-id: 0
issue-id: 0
icon: Assets/Shadow.png
---

If you are looking to apply shadows to rectangle or card style elements, it is recommended to use `AttachedCardShadow` instead.

AttachedDropShadow is best used when:

- elements don't have common masking shapes
- you don't have a transparent element
- you're not animating/moving the element's position

You can find out more information about these comparisons in the general [`Attached Shadows`](AttachedShadows.md) documentation.

## Using AttachedDropShadow

Creating a shadow is fairly straight-forward. First, you need to designate a _sibling_ element to sit behind all elements
which you'd like to have casing shadows. This single element can host as many shadows as required by other elements casting shadows.

The parent element **cannot** be used, as otherwise your shadows will appear on top of your controls.

With the target element setup, you can simply attach a shadow to any number of other elements within your panel you'd like!

> [!SAMPLE AttachedDropShadowBasicSample]

> [!NOTE]
> If you need to scroll the element and its shadow, be sure to place your entire panel, including the sibling shadow target within the
ScrollViewer.

## Shadow as a Resource

Creating a shadow definition for each Shadow can be a bit cumbersome. Fortunately, you can define a shadow definition as a resource
and reuse the same shadow look-and-feel anywhere you need it!

> [!SAMPLE AttachedDropShadowResourceSample]

## Shadow as a Style

You can also use that same resource within a style on your page (for app-level see `AttachedCardShadow`).

Or define your own definition within your style as we've done here:

> [!SAMPLE AttachedDropShadowStyleSample]
