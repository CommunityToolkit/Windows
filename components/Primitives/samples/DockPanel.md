---
title: DockPanel
author: IbraheemOsama
description: Defines an area where you can arrange child elements either horizontally or vertically, relative to each other.
keywords: DockPanel
dev_langs:
  - csharp
category: Layouts
subcategory: Panel
discussion-id: 0
issue-id: 0
icon: Assets/DockPanel.png
---

The DockPanel position child controls based on the child Dock property, you have 4 options to Dock, left (Default), right, top, bottom.
You can set DockPanel LastChildFill property to true if you want the last item added to the DockPanel to fill the rest empty space.

> [!SAMPLE DockPanelSample]

## Syntax

```xaml
<Page ...
     xmlns:controls="using:CommunityToolkit.WinUI.Controls"/>

<controls:DockPanel Name="SampleDockPanel" Margin="2" Background="LightGray" LastChildFill="False" >
  <StackPanel Height="100" controls:DockPanel.Dock="Top" Background="Black"></StackPanel>
  <StackPanel Width="100" controls:DockPanel.Dock="Left" Background="Red"></StackPanel>
  <StackPanel Height="100" controls:DockPanel.Dock="Bottom" Background="Green"></StackPanel>
  <StackPanel Width="100" controls:DockPanel.Dock="Right" Background="Blue"></StackPanel>
</controls:DockPanel>
```
