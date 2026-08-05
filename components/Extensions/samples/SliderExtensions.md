---
title: SliderExtensions
author: niels9001
description: SliderExtensions provides attached properties that let a Slider be adjusted by mouse-wheel input.
keywords: windows 10, windows 11, windows community toolkit, winui, Slider, extensions, mouse wheel
dev_langs:
  - csharp
category: Extensions
subcategory: Controls
discussion-id: 0
issue-id: 0
icon: Assets/Extensions.png
---

The `SliderExtensions` static class provides attached properties for the [`Slider`](https://learn.microsoft.com/windows/windows-app-sdk/api/winrt/microsoft.ui.xaml.controls.slider) control.

> **Platform APIs:** `SliderExtensions`

## IsMouseWheelEnabled

Setting `SliderExtensions.IsMouseWheelEnabled` to `true` opts a slider in to mouse-wheel input: scrolling the wheel while the pointer is over the slider increments or decrements `Value`. The wheel event is marked handled, so an enclosing [`ScrollViewer`](https://learn.microsoft.com/windows/windows-app-sdk/api/winrt/microsoft.ui.xaml.controls.scrollviewer) is not also scrolled.

```xaml
<Page xmlns:ui="using:CommunityToolkit.WinUI">
    <Slider Minimum="0"
            Maximum="100"
            ui:SliderExtensions.IsMouseWheelEnabled="True" />
</Page>
```

## MouseWheelChange

`SliderExtensions.MouseWheelChange` is the amount added to or subtracted from `Value` per wheel notch. The naming mirrors the built-in [`Slider.SmallChange`](https://learn.microsoft.com/windows/windows-app-sdk/api/winrt/microsoft.ui.xaml.controls.primitives.rangebase.smallchange) / [`LargeChange`](https://learn.microsoft.com/windows/windows-app-sdk/api/winrt/microsoft.ui.xaml.controls.primitives.rangebase.largechange) properties. When unset (the default of `double.NaN`), the slider's own `SmallChange` is used — so a slider already configured for arrow-key input "just works" for the wheel too.

```xaml
<Page xmlns:ui="using:CommunityToolkit.WinUI">
    <Slider Minimum="0"
            Maximum="100"
            ui:SliderExtensions.IsMouseWheelEnabled="True"
            ui:SliderExtensions.MouseWheelChange="5" />
</Page>
```

> [!SAMPLE SliderWheelSample]
