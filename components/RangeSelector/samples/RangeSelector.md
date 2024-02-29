---
title: RangeSelector
author: nmetulev
description: The RangeSelector Control is a Double Slider control that allows the user to select a sub-range of values from a larger range of possible values. The user can slide from the left or right of the range.
keywords: RangeSelector, Control, double slider, slider rangeslider
dev_langs:
  - csharp
category: Controls
subcategory: Input
discussion-id: 0
issue-id: 0
icon: Assets/RangeSelector.png
---

A `RangeSelector` is pretty similar to a regular `Slider`, and shares some of its properties such as `Minimum`, `Maximum` and `StepFrequency`.

> [!Sample RangeSelectorSample]

> [!NOTE]
> If you are using a RangeSelector within a ScrollViewer you'll need to add some codes. This is because by default, the ScrollViewer will block the thumbs of the RangeSelector to capture the pointer.

Here is an example of using RangeSelector within a ScrollViewer:

```xaml
<controls:RangeSelector x:Name="Selector" ThumbDragStarted="Selector_OnDragStarted" ThumbDragCompleted="Selector_OnDragCompleted"/>
```

```csharp
private void Selector_OnDragStarted(object sender, DragStartedEventArgs e)
{
 ScrollViewer.HorizontalScrollMode = ScrollMode.Disabled;
 ScrollViewer.VerticalScrollMode = ScrollMode.Disabled;
}

private void Selector_OnDragCompleted(object sender, DragCompletedEventArgs e)
{
 ScrollViewer.HorizontalScrollMode = ScrollMode.Auto;
 ScrollViewer.VerticalScrollMode = ScrollMode.Auto;
}
```
