---
title: MetadataControl
author: vgromfeld
description: The MetadataControl control displays a list of labels and hyper-links separated by a bullet.
keywords: MetadataControl, Control, metadata
dev_langs:
  - csharp
category: Controls
subcategory: StatusAndInfo
discussion-id: 0
issue-id: 0
icon: Assets/MetadataControl.png
---

The bullet separator can be customized using the `Separator` property.
`AccessibleSeparator` is used as a replacement for `Separator` to generate the accessible string.

The control needs a list of [MetadataItem](https://github.com/windows-toolkit/WindowsCommunityToolkit/blob/main/Microsoft.Toolkit.Uwp.UI.Controls.Core/MetadataControl/MetadataItem.cs).
Each item will be displayed either as a text or as an hyper-link (if the `Command`property is set).

The default control template is using on a `TextBlock`. The style of this `TextBlock` can be customized using the `TextBlockStyle` property.

> [!SAMPLE MetadataControlSample]

## Example

Add the control in the page:

```xaml
<controls:MetadataControl
    x:Name="metadataControl"
    Separator="   "
    AccessibleSeparator=", "/>
```

Add items to control:

```cs
metadataControl.Items = new[]
{
    new MetadataItem { Label = "Hello" },
    new MetadataItem { Label = "World", Command = myCommand },
};
```

## MetadataItem

A `MetadataItem` contains the information about one entry which will be displayed in the `MetadataControl`
