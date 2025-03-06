---
title: PipelineVisualFactory
author: erinwoo  
description: A helper type that can be used to create sprite visuals with custom Win2D/Composition effects chains and attach them to UI elements. 
keywords: pipeline, visual, factory, pipe
dev_langs:
  - csharp
category: Xaml
subcategory: Effects
discussion-id: 0
issue-id: 0
icon: Assets/EffectAnimations.png
---

PipelineVisualFactory can create the same visual brushes as the PipelineBrush type, but it can attach them directly on the underlying Visual instance backing a UI element. This can make the XAML code less verbose and more efficient, as there is no need to insert additional elements just so that a brush can be applied to them.

> [!SAMPLE PipelineVisualFactorySample]
