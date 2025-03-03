---
title: AcrylicBrush
author: erinwoo  
description: A brush that can apply an acrylic background effect.
keywords: brush, acrylic, acrylicbrush
dev_langs:
  - csharp
category: Media
subcategory: Brushes
discussion-id: 0
issue-id: 0
icon: Assets/MediaBrushes.png
---

Acrylic is a type of Brush that creates a translucent texture. You can apply acrylic to app surfaces to add depth and help establish a visual hierarchy.
To learn more about the acrylic material in Windows: [link](https://learn.microsoft.com/en-us/windows/apps/design/style/acrylic)

> [!SAMPLE AcrylicBrushSample]

#### *UWP only:*
There are two modes for setting the BackgroundSource property when using an `AcrylicBrush`.
Setting it to `Backdrop` results in the acrylic effect being applied to whatever is *behind the brush* in the application.

Setting BackgroundSource to `HostBackdrop` results in the acrylic effect being applied *behind the current application*.
