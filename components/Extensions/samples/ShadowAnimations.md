---
title: Shadow animations
author: michael-hawker
description: Animating Attached Shadows
keywords: shadow, shadows, dropshadow, dropshadowpanel, attachedshadow, attacheddropshadow, attachedcardshadow
dev_langs:
  - csharp
category: Animations
subcategory: Shadows
discussion-id: 0
issue-id: 0
icon: Assets/ShadowAnimation.png
---

Either type of Attached Shadow can be easily animated using the Toolkit's [`AnimationSet`](../animations/AnimationSet.md) api. These provide an easy XAML based way to animate a wide variety of elements, including a variety of shadow properties. They can also be animated with any other composition animation technique in code-behind as well using either the [`AnimationBuilder`](../animations/AnimationBuilder.md) or built-in composition animations.

> **Platform APIs:** `BlurRadiusDropShadowAnimation`, `ColorDropShadowAnimation`, `OffsetDropShadowAnimation`, `OpacityDropShadowAnimation`

> [!NOTE]
> `AttachedCardShadow` has better support for animations which involve translation of the element along with the shadow. If animating an `AttachedDropShadow` it is best to only animate the shadow itself vs. moving the element it is attached to. This can cause the shadow and element to be desynchronized.

### Example

The following example uses a combination of behaviors and animations apis to create an animated shadow effect when hovering over an image with an `OffsetDropShadowAnimation`:

```xaml
    xmlns:ui="using:CommunityToolkit.WinUI"
    xmlns:media="using:CommunityToolkit.WinUI.Media"
    xmlns:interactivity="using:Microsoft.Xaml.Interactivity"
    xmlns:interactions="using:Microsoft.Xaml.Interactions.Core"
    xmlns:ani="using:CommunityToolkit.WinUI.Animations"
    xmlns:behaviors="using:CommunityToolkit.WinUI.Behaviors"/>

    <Image Height="100" Width="100"
           Source="ms-appx:///Assets/Photos/Owl.jpg">
        <ui:Effects.Shadow>
            <media:AttachedCardShadow Offset="4" CornerRadius="0"/>
        </ui:Effects.Shadow>
        <ani:Explicit.Animations>
            <ani:AnimationSet x:Name="ShadowEnterAnimation">
                <ani:OffsetDropShadowAnimation To="12"/>
            </ani:AnimationSet>

            <ani:AnimationSet x:Name="ShadowExitAnimation">
                <ani:OffsetDropShadowAnimation To="4"/>
            </ani:AnimationSet>
        </ani:Explicit.Animations>
        <interactivity:Interaction.Behaviors>
            <interactions:EventTriggerBehavior EventName="PointerEntered">
                <behaviors:StartAnimationAction Animation="{x:Bind ShadowEnterAnimation}"/>
            </interactions:EventTriggerBehavior>
            <interactions:EventTriggerBehavior EventName="PointerExited">
                <behaviors:StartAnimationAction Animation="{x:Bind ShadowExitAnimation}"/>
            </interactions:EventTriggerBehavior>
        </interactivity:Interaction.Behaviors>
    </Image>
```
