---
title: LayoutTransformControl
author: odonno
description: The LayoutTransformControl is a control that support transformations on FrameworkElement as if applied by LayoutTransform.
keywords: LayoutTransformControl, Control, Layout, RenderTransform, RotateTransform, ScaleTransform, SkewTransform, Transform
dev_langs:
  - csharp
category: Controls
subcategory: Layout
discussion-id: 0
issue-id: 0
---

# LayoutTransformControl

The [LayoutTransformControl](/dotnet/api/microsoft.toolkit.uwp.ui.controls.layouttransformcontrol) is a control that applies Matrix transformations on any `FrameworkElement` of your application.

The transformations that can be applied are one of the following:

* [RotateTransform](/uwp/api/windows.ui.xaml.media.rotatetransform)
* [ScaleTransform](/uwp/api/windows.ui.xaml.media.scaletransform)
* [SkewTransform](/uwp/api/windows.ui.xaml.media.skewtransform)
* [MatrixTransform](/uwp/api/windows.ui.xaml.media.matrixtransform)
* [TransformGroup](/uwp/api/windows.ui.xaml.media.transformgroup)

> [!Sample LayoutTransformControlSample]

## Syntax

```xaml
<controls:LayoutTransformControl Background="Black" 
                                 HorizontalAlignment="Center" 
                                 VerticalAlignment="Center"
                                 RenderTransformOrigin="0.5,0.5">
    <controls:LayoutTransformControl.Transform>
        <RotateTransform Angle="90" />
    </controls:LayoutTransformControl.Transform>

    <Border HorizontalAlignment="Center" 
            VerticalAlignment="Center"
            BorderBrush="Red"
            BorderThickness="5">
        <Grid>
            <TextBlock Padding="10" Foreground="White" Text="This is a test message." />
        </Grid>
    </Border>
</controls:LayoutTransformControl>
```

## Properties

| Property | Type | Description |
| -- | -- | -- |
| Child | FrameworkElement | The content of the control that will receive matrix transformations |
| Transform | Transform | The transformations to apply on the `Content`. It can be a single transformation like `RotateTransform`, `ScaleTransform` or `SkewTransform` or it can be a combo of multiple transformations using `TransformGroup` |
