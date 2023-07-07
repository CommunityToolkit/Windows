---
title: HeaderedContentControl
author: skendrot
description: The HeaderedContentControl allows content to be displayed with a specified header.
keywords: HeaderedContentControl, Control, headered
dev_langs:
  - csharp
category: Controls
subcategory: Layout
discussion-id: 0
issue-id: 0
icon: Assets/HeaderedContentControl.png
---

The `Header` property can be any object and you can use the `HeaderTemplate` to specify a custom look to the header. Content for the HeaderedContentControl will align to the top left. This is to maintain the same functionality as the ContentControl.

> [!NOTE]
> Setting the `Background`, `BorderBrush` and `BorderThickness` properties will not have any effect on the HeaderedContentControl. This is to maintain the same functionality as the ContentControl.

> [!Sample HeaderedContentControlSample]

> [!Sample HeaderedContentControlTextSample]

> [!Sample HeaderedContentControlImageSample]

> [!Sample HeaderedContentControlComplexSample]

## Syntax

```xaml
<Page ...
     xmlns:controls="using:CommunityToolkit.WinUI.Controls"/>

<controls:HeaderedContentControl>
    <!-- Header content or HeaderTemplate content -->
</controls:HeaderedContentControl>
```
