// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Windows.Storage;

namespace ImageCropperExperiment.Samples;

[ToolkitSampleMultiChoiceOption("ThumbPlacementSetting", "All", "Corners", Title = "Thumb Placement")]
[ToolkitSampleMultiChoiceOption("CropShapeSetting", "Circular", "Rectangular", Title = "Crop Shape")]
[ToolkitSampleMultiChoiceOption("AspectRatioSetting", "Custom", "Square", "Landscape(16:9)", "Portrait(9:16)", "4:3", "3:2", Title = "Aspect Ratio")]

[ToolkitSample(id: nameof(ImageCropperOverlaySample), "ImageCropper Overlay", description: $"A sample for showing how to use the overlay feature of {nameof(ImageCropper)}.")]
public sealed partial class ImageCropperOverlaySample : Page
{
    public ImageCropperOverlaySample()
    {
        this.InitializeComponent();

        _ = Load();
    }

    private async Task Load()
    {
        var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/Owl.jpg"));
        await ImageCropper.LoadImageFromFile(file);
    }
}
