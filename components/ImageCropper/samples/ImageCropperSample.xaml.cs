// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.WinUI.Controls;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace ImageCropperExperiment.Samples;

[ToolkitSampleMultiChoiceOption("ThumbPlacementSetting", "All", "Corners", Title = "Thumb Placement")]
[ToolkitSampleMultiChoiceOption("CropShapeSetting", "Rectangular", "Circular", Title = "Crop Shape")]
[ToolkitSampleMultiChoiceOption("AspectRatioSetting", "Custom", "Square", "Landscape(16:9)", "Portrait(9:16)", "4:3", "3:2", Title = "Aspect Ratio")]

[ToolkitSample(id: nameof(ImageCropperSample), "ImageCropper", description: $"A sample for showing how to create and use a {nameof(ImageCropper)}.")]
public sealed partial class ImageCropperSample : Page
{
    public ImageCropperSample()
    {
        this.InitializeComponent();
        _ = Load();
    }

    private async Task Load()
    {
        var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/Owl.jpg"));
        await imageCropper.LoadImageFromFile(file);
    }

    private async Task PickImage()
    {
        var filePicker = new FileOpenPicker
        {
            ViewMode = PickerViewMode.Thumbnail,
            SuggestedStartLocation = PickerLocationId.PicturesLibrary,
            FileTypeFilter =
                {
                    ".png", ".jpg", ".jpeg"
                }
        };
        var file = await filePicker.PickSingleFileAsync();
        if (file != null && imageCropper != null)
        {
            await imageCropper.LoadImageFromFile(file);
        }
    }

    private async Task SaveCroppedImage()
    {
        var savePicker = new FileSavePicker
        {
            SuggestedStartLocation = PickerLocationId.PicturesLibrary,
            SuggestedFileName = "Cropped_Image",
            FileTypeChoices =
                {
                    { "PNG Picture", new List<string> { ".png" } },
                    { "JPEG Picture", new List<string> { ".jpg" } }
                }
        };
        var imageFile = await savePicker.PickSaveFileAsync();
        if (imageFile != null)
        {
            BitmapFileFormat bitmapFileFormat;
            switch (imageFile.FileType.ToLower())
            {
                case ".png":
                    bitmapFileFormat = BitmapFileFormat.Png;
                    break;
                case ".jpg":
                    bitmapFileFormat = BitmapFileFormat.Jpeg;
                    break;
                default:
                    bitmapFileFormat = BitmapFileFormat.Png;
                    break;
            }

            using (var fileStream = await imageFile.OpenAsync(FileAccessMode.ReadWrite, StorageOpenOptions.None))
            {
                await imageCropper.SaveAsync(fileStream, bitmapFileFormat);
            }
        }
    }

    public static ThumbPlacement ConvertStringToThumbPlacement(string placement) => placement switch
    {
        "All" => ThumbPlacement.All,
        "Corners" => ThumbPlacement.Corners,
        _ => throw new System.NotImplementedException(),
    };


    public static CropShape ConvertStringToCropShape(string cropshape) => cropshape switch
    {
        "Circular" => CropShape.Circular,
        "Rectangular" => CropShape.Rectangular,
        _ => throw new System.NotImplementedException(),
    };

    public static double? ConvertStringToAspectRatio(string ratio) => ratio switch
    {
        "Custom" => null,
        "Square" => 1,
        "Landscape(16:9)" => 16d / 9d,
        "Portrait(9:16)" => 9d / 16d,
        "4:3" => 4d / 3d,
        "3:2" => 3d / 2d,
        _ => throw new System.NotImplementedException(),
    };

    private async void PickButton_Click(object sender, RoutedEventArgs e)
    {
        await PickImage();
    }

    private async void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        await SaveCroppedImage();
    }

    private void ResetButton_Click(object sender, RoutedEventArgs e)
    {
        imageCropper.Reset();
    }
}
