---
title: PipelineBrush
author: erinwoo  
description: A brush that renders a customizable Composition/Win2D effects pipeline. This allows for applying multiple effects into a single effect chain.
keywords: brush, pipeline, pipe
dev_langs:
  - csharp
category: Xaml
subcategory: Effects
discussion-id: 0
issue-id: 0
icon: Assets/EffectAnimations.png
---

`Source` gets or sets the source for the current pipeline (defaults to a `BackdropSourceExtension` with `Microsoft.UI.Xaml.Media.AcrylicBackgroundSource.Backdrop` source).
Multiple effects can be nested within the `PipelineBrush` component. 

> [!SAMPLE PipelineBrushSample]
