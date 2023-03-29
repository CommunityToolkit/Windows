---
title: OnDeviceExtension
author: sonnemaf
description: The OnDevice markup extension allows you to customize UI appearance on a per-DeviceFamily basis.
keywords: windows 10, uwp, uwp community toolkit, uwp toolkit, device family, markup extension, XAML, markup
dev_langs:
  - csharp
category: Extensions
subcategory: Markup
discussion-id: 0
issue-id: 0
---

# OnDeviceExtension

The [`OnDeviceExtension`](/dotnet/api/microsoft.toolkit.uwp.ui.ondeviceextension) type allows you to customize UI appearance on a per-DeviceFamily basis. It is inspired on the [OnPlatform](https://github.com/xamarin/Xamarin.Forms/issues/2608) markup extensions from Xamarin.Forms 3.2

> **Platform APIs:** [`OnDeviceExtension`](/dotnet/api/microsoft.toolkit.uwp.ui.ondeviceextension)

Here is how the property can be used in XAML:

```xaml
<TextBlock
   xmlns:ui="using:Microsoft.Toolkit.Uwp.UI"
   Text="{ui:OnDevice Default=Hi, Desktop=Hello, Xbox=World}"/>

```
