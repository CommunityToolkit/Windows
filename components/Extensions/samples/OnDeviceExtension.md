---
title: OnDeviceExtension
author: sonnemaf
description: The OnDevice markup extension allows you to customize UI appearance on a per-DeviceFamily basis.
keywords: markup extension, XAML, markup
dev_langs:
  - csharp
category: Extensions
subcategory: Markup
discussion-id: 0
issue-id: 0
icon: Assets/Extensions.png
---

The `OnDeviceExtension` type allows you to customize UI appearance on a per-DeviceFamily basis. It is inspired on the [OnPlatform](https://github.com/xamarin/Xamarin.Forms/issues/2608) markup extensions from Xamarin.Forms 3.2

Here is how the property can be used in XAML:

```xaml
<TextBlock
   xmlns:ui="using:CommunityToolkit.WinUI"
   Text="{ui:OnDevice Default=Hi, Desktop=Hello, Xbox=World}"/>

```
