---
title: ImageCropper
author: HHChaos
description: Control to crop rectangular and circular images.
keywords: ImageCropper, Control, Layout
dev_langs:
  - csharp
category: Controls
subcategory: Media
discussion-id: 0
issue-id: 0
icon: Assets/ImageCropper.png
---

The `ImageCropper Control` allows user to freely crop an image.

> [!Sample ImageCropperSample]

## Syntax

```xaml
<Page ...
     xmlns:controls="using:CommunityToolkit.WinUI.Controls"/>
    <controls:ImageCropper x:Name="ImageCropper" />
</Page>
```

## Examples

### Use ImageCropper

You can set the cropped image source by using the `LoadImageFromFile(StorageFile)` method or setting the `Source` property.

```csharp
//Load an image.
await ImageCropper.LoadImageFromFile(file);

//Another way to load an image.
ImageCropper.Source = writeableBitmap;

//Saves the cropped image to a stream.
using (var fileStream = await someFile.OpenAsync(FileAccessMode.ReadWrite, StorageOpenOptions.None))
{
    await _imageCropper.SaveAsync(fileStream, BitmapFileFormat.Png);
}
```

### Use Circular ImageCropper

You can set `CropShape` property to use the circular ImageCropper.

```csharp
ImageCropper.CropShape = CropShape.Circular;
```

### Change Aspect Ratio

You can set `AspectRatio` property to change the aspect ratio of the cropped image.

```csharp
ImageCropper.AspectRatio = 16d / 9d;
```

Or you can crop image without aspect ratio.

```csharp
ImageCropper.AspectRatio = null;
```
