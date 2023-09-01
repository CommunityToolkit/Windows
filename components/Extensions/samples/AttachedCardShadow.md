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

```xaml
    xmlns:ui="using:CommunityToolkit.WinUI"
    xmlns:media="using:CommunityToolkit.WinUI.Media"/>

    <Page.Resources>
        <media:AttachedCardShadow x:Key="CommonShadow" Offset="4"/>

        <Style TargetType="Button" BasedOn="{StaticResource DefaultButtonStyle}">
            <Setter Property="ui:Effects.Shadow" Value="{StaticResource CommonShadow}"/>
            <Setter Property="HorizontalAlignment" Value="Center"/>
        </Style>
    </Page.Resources>

    <StackPanel Spacing="32" VerticalAlignment="Center">
        <Button Content="I Have a Shadow!"/>
        <Image ui:Effects.Shadow="{StaticResource CommonShadow}"
               Height="100" Width="100"
               Source="ms-appx:///Assets/Photos/Owl.jpg"/>
    </StackPanel>
```
