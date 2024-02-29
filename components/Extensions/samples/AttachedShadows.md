---
title: Shadows overview
author: michael-hawker
description: Attached Shadows allow you to easily create shadow effects on elements.
keywords: shadow, shadows, dropshadow, dropshadowpanel, attachedshadow, attacheddropshadow, attachedcardshadow
dev_langs:
  - csharp
category: Extensions
subcategory: Shadows
discussion-id: 0
issue-id: 0
icon: Assets/Shadow.png
---
> **Platform APIs:** `AttachedCardShadow`, `AttachedDropShadow`

There are two types of attached shadows available today, the `AttachedCardShadow` and the `AttachedDropShadow`. It is recommended to use the `AttachedCardShadow` where possible, if you don't mind the dependency on Win2D. The `AttachedCardShadow` provides an easier to use experience that is more performant and easier to apply across an entire set of elements, assuming those elements are rounded-rectangular in shape. The `AttachedDropShadow` provides masking support and can be leveraged in any UWP app without adding an extra dependency.

## Capability Comparison

The following table outlines the various capabilities of each shadow type in addition to comparing to the previous `DropShadowPanel` implementation:

| Capability                    | AttachedCardShadow                                                 | AttachedDropShadow                                              | DropShadowPanel (deprecated/removed)                                                       |
|-------------------------------|--------------------------------------------------------------------|-----------------------------------------------------------------|-----------------------------------------------------------------------------------------|
| Dependency/NuGet Package      | 🟡 Win2D via<br>`CommunityToolkit.WinUI.Media`                       | ✅ Framework Only (Composition Effect)<br>`CommunityToolkit.WinUI.Effects`  | ✅ Framework Only (Composition Effect)<br>`Microsoft.Toolkit.Uwp.UI.Controls` (legacy)                              |
| Layer                         | Inline Composition +<br>Win2D Clip Geometry                           | Composition via<br>Target Element Backdrop                         | Composition via<br>`ContentControl` Container                                                |
| Modify Visual Tree            | ✅ No                                                              | 🟡 Usually requires single target element, even for multiple shadows    | ❌ Individually wrap each element needing shadow                                         |
| Extra Visual Tree Depth       | ✅ 0                                                               | 🟡 1 per sibling element to cast one or more shadows to          | ❌ _**4** per Shadowed Element_                                                                |
| Masking/Geometry              | 🟡 Rectangular/Rounded-Rectangles only                               | ✅ Can mask images, text, and shapes (performance penalty)     | ✅ Can mask images, text, and shapes (performance penalty)                                 |
| Performance                   | ✅ Fast, applies rectangular clipped geometry                       | 🟡 Slower, especially when masking (default);<br>can use rounded-rectangles optimization                  | ❌ Slowest, no optimization for rounded-rectangles                                                     |
| ResourceDictionary Support    | ✅ Yes                                                              | ✅ Yes                                                           | ❌ Limited, via complete custom control style +<br>still need to wrap each element to apply |
| Usable in Styles              | ✅ Yes, anywhere, including app-level                               | 🟡 Yes, but limited in scope due to element target | ❌ No                                                                                    |
| Supports Transparent Elements | ✅ Yes, shadow is clipped and not visible                           | ❌ No, shadow shows through transparent element                  | ❌ No, shadow shows through transparent element                                          |
| Animation Support             | ✅ Yes, in XAML via [`AnimationSet`](../animations/AnimationSet.md) | 🟡 Partial, translating/moving element may desync shadow         | ❌ No                                                                                    |

## AttachedCardShadow (Win2D)

The `AttachedCardShadow` is the easiest to use and most performant shadow. It is recommended to use it where possible, if taking a Win2D dependency is not a concern. It's only drawbacks are the extra dependency required and that it only supports rectangular and rounded-rectangular geometries (as described in the table above).

The great benefit to the `AttachedCardShadow` is that no extra surface or element is required to add the shadow. This reduces the complexity required in development and allows shadows to easily be added at any point in the development process. It also supports transparent elements, without displaying the shadow behind them!

## AttachedDropShadow (Composition)

The `AttachedDropShadow` provides masking support to a wide variety of elements including transparent images, shapes, and text. Masking to a custom shape that's not rectangular or rounded-rectangular is the main scenario where an `AttachedDropShadow` will be useful. Unlike it's predecessor, the `DropShadowPanel`, the `AttachedDropShadow` doesn't need to wrap the element being shadowed; however, it does require another element to cast the shadow on to.

This makes it a bit more complex to use than the `AttachedCardShadow` and the `DropShadowPanel`, but since multiple `AttachedDropShadow` elements can share the same surface, this makes it much more performant than its predecessor.

### Remarks

While `DropShadowPanel` encapsulated the complexities of adding a shadow, it added a lot of depth and complexity to the visual tree. `AttachedDropShadow` instead requires that you provide the surface where the shadows should be cast, such as a common backdrop element. This means that each shadow can use the same shared surface rather than creating its own backdrop element for each shadow (like `DropShadowPanel` did). This slight trade-off for ease-of-use is a gain in performance. However, it does mean it may not be as flexible for certain use cases with manipulation of an element. In those cases we recommend to try and use `AttachedCardShadow` instead or wrapping an element and its backdrop in a custom control.
