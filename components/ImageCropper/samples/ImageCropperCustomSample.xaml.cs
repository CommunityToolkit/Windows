// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.WinUI.Controls;

namespace ImageCropperExperiment.Samples;

[ToolkitSampleMultiChoiceOption("ThumbPlacementSetting", "All", "Corners", Title = "Thumb Placement")]
[ToolkitSampleMultiChoiceOption("CropShapeSetting", "Rectangular", "Circular", Title = "Crop Shape")]

[ToolkitSample(id: nameof(ImageCropperCustomSample), "ImageCropper", description: $"A sample for showing how to create and use a {nameof(ImageCropper)}.")]
public sealed partial class ImageCropperCustomSample : Page
{
    public ImageCropperCustomSample()
    {
        this.InitializeComponent();
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
}
