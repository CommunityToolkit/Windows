---
title: ImageCropper
author: HHChaos
description: Control to crop rectangular and circular images.
keywords: ImageCropper, Control, Layout
dev_langs:
  - csharp
category: Controls
subcategory: Layout
discussion-id: 0
issue-id: 0
icon: assets/icon.png
---

# ImageCropper

The [ImageCropper Control](/dotnet/api/microsoft.toolkit.uwp.ui.controls.imagecropper) allows user to freely crop an image.

> [!Sample ImageCropperSample]

## Syntax

```xaml
<Page ...
     xmlns:controls="using:CommunityToolkit.WinUI.Controls"/>
    <controls:ImageCropper x:Name="ImageCropper" />
</Page>
```



## Properties

| Property | Type | Description |
| -- | -- | -- |
| MinCroppedPixelLength | double          | Gets or sets the minimum cropped length(in pixel).           |
| MinSelectedLength     | double          | Gets or sets the minimum selectable length.                  |
| CroppedRegion         | Rect            | Gets the current cropped region.                             |
| Source                | WriteableBitmap | Gets or sets the source of the cropped image.                |
| AspectRatio           | double?         | Gets or sets the aspect ratio of the cropped image, the default value is null. |
| CropShape             | CropShape       | Gets or sets the shape to use when cropping.                 |
| Mask                  | Brush           | Gets or sets the mask on the cropped image.                  |
| PrimaryThumbStyle     | Style           | Gets or sets a value for the style to use for the primary thumbs of the ImageCropper. |
| SecondaryThumbStyle   | Style           | Gets or sets a value for the style to use for the secondary thumbs of the ImageCropper. |
| ThumbPlacement        | ThumbPlacement  | Gets or sets a value for thumb placement.                    |

## Methods

| Methods  | Return Type | Description |
| -- | -- | -- |
| LoadImageFromFile(StorageFile) | Task | Load an image from a file. |
| SaveAsync(IRandomAccessStream,BitmapFileFormat,bool) | Task | Saves the cropped image to a stream with the specified format. Setting the boolean argument to True will save pixel values to the extent of the cropped area regardless of the crop shape, otherwise transparent or black pixels will fill the uncropped area depending on file format. |
| Reset()  | void | Reset the cropped area. |
| TrySetCroppedRegion(Rect rect) | bool | Tries to set a new value for the cropped region, returns true if it succeeded, false if the region is invalid |

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
