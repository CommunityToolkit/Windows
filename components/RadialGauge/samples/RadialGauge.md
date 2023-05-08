---
title: RadialGauge
author: xamlbrewer
description: The Radial Gauge Control displays a value in a certain range using a needle on a circular face.
keywords: RadialGauge, Control, Input
dev_langs:
  - csharp
category: Controls
subcategory: Input
discussion-id: 0
issue-id: 0
---

# RadialGauge

The [Radial Gauge](/dotnet/api/microsoft.toolkit.uwp.ui.controls.radialgauge) control displays a value in a certain range using a needle on a circular face. This control will make data visualizations and dashboards more engaging with rich style and interactivity.
The round gauges are powerful, easy to use, and highly configurable to present dashboards capable of displaying clocks, industrial panels, automotive dashboards, and even aircraft cockpits.

The Radial Gauge supports animated transitions between configuration states. The control gradually animates as it redraws changes to the needle, needle position, scale range, color range, and more.

> [!Sample RadialGaugeSample]

## Properties

| Property | Type | Description |
| -- | -- | -- |
| Column | double | Gets or sets the column of the scale |
| IsInteractive | bool | Gets or sets a value indicating whether the control accepts setting its value through interaction |
| MaxAngle | int | Gets or sets the end angle of the scale, which corresponds with the Maximum value, in degrees |
| Maximum | double | Gets or sets the maximum value of the scale |
| MinAngle | int | Gets or sets the start angle of the scale, which corresponds with the Minimum value, in degrees |
| Minimum | double | Gets or sets the minimum value of the scale |
| NeedleBrush | SolidColorBrush | Gets or sets the needle background |
| NeedleBorderBrush | SolidColorBrush | Gets or sets the needle border |
| NeedleBorderThickness | double | Gets or sets the thickness of the border |
| NeedleLength | double | Gets or sets the needle length, in percentage of the gauge radius |
| NeedleWidth | double | Gets or sets the needle width, in percentage of the gauge radius |
| NormalizedMaxAngle | double | Gets the normalized maximum angle |
| NormalizedMinAngle | double | Gets the normalized minimum angle |
| ScaleBrush | Brush | Gets or sets the scale brush |
| ScalePadding | double | Gets or sets the distance of the scale from the outside of the control, in percentage of the gauge radius |
| ScaleTickCornerRadius | double | Gets or sets the cornerradius of the scale tick
| ScaleTickBrush | SolidColorBrush | Gets or sets the scale tick brush |
| ScaleTickLength | double | Gets or sets the length of the scaleticks, in percentage of the gauge radius |
| ScaleTickWidth | double | Gets or sets the width of the scale ticks, in percentage of the gauge radius |
| ScaleWidth | double | Gets or sets the width of the scale, in percentage of the gauge radius |
| StepSize | double | Gets or sets the rounding interval for the Value |
| TickBrush | SolidColorBrush | Gets or sets the outer tick brush |
| TickCornerRadius | double | Gets or sets the cornerradius of the tick |
| TickLength | double | Gets or sets the length of the ticks, in percentage of the gauge radius |
| TickPadding | double | Gets or sets the distance of the ticks from the outside of the control, in percentage of the gauge radius |
| TickSpacing | int | Gets or sets the tick spacing, in units |
| TickWidth | double | Gets or sets the width of the ticks, in percentage of the gauge radius |
| TrailBrush | Brush | Gets or sets the trail brush |
| Unit | string | Gets or sets the displayed unit measure |
| Value | double | Gets or sets the current value |
| ValueAngle | double | Gets or sets the current angle of the needle (between MinAngle and MaxAngle). Setting the angle will update the Value |
| ValueStringFormat | string | Gets or sets the value string format |
