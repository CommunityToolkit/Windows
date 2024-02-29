---
title: CameraHelper
author: skommireddi
description: The CameraHelper provides helper methods to easily use the available camera frame sources to preview video, capture video frames and software bitmaps.
keywords: Helpers, CameraHelper, Camera, Frame Source, Video Frame, Software Bitmap
dev_langs:
  - csharp
category: Helpers
subcategory: System
discussion-id: 0
issue-id: 0
icon: Assets/CameraHelper.png
---

The helper currently shows camera frame sources that support color video preview or video record streams.

> [!IMPORTANT]
> Make sure you have the [webcam capability](https://learn.microsoft.com/windows/uwp/packaging/app-capability-declarations#device-capabilities) enabled for your app to access the device's camera.

> [!Sample CameraHelperSample]

## Syntax

```csharp
// Creates a Camera Helper and gets video frames from an available frame source.
using CommunityToolkit.WinUI.Helpers.CameraHelper;

CameraHelper _cameraHelper = new CameraHelper();
var result = await _cameraHelper.InitializeAndStartCaptureAsync();

// Camera Initialization and Capture failed for some reason
if(result != CameraHelperResult.Success)
{
  // get error information
  var errorMessage = result.ToString();
}
else 
{
  // Subscribe to get frames as they arrive
  _cameraHelper.FrameArrived += CameraHelper_FrameArrived;
}

private void CameraHelper_FrameArrived(object sender, FrameEventArgs e)
{
  // Gets the current video frame
  VideoFrame currentVideoFrame  = e.VideoFrame;

  // Gets the software bitmap image
  SoftwareBitmap softwareBitmap = currentVideoFrame.SoftwareBitmap;
}
```

## Cleaning up resources

As a developer, you will need to make sure the CameraHelper resources are cleaned up when appropriate. For example, if the CameraHelper is only used on one page, make sure to clean up the CameraHelper when navigating away from the page.

Likewise, make sure to handle app [suspending](https://learn.microsoft.com/windows/uwp/launch-resume/suspend-an-app) and [resuming](https://learn.microsoft.com/windows/uwp/launch-resume/resume-an-app) - CameraHelper should be cleaned up when suspending and re-initialized when resuming.

Call `CameraHelper.CleanupAsync()` to clean up all internal resources. See the [CameraHelper sample page in the sample app](https://github.com/windows-toolkit/WindowsCommunityToolkit/tree/rel/7.1.0/Microsoft.Toolkit.Uwp.SampleApp/SamplePages/CameraHelper) for full example.

## Examples

Demonstrates using Camera Helper to get video frames from a specific media frame source group.

```csharp

using CommunityToolkit.WinUI.Helpers.CameraHelper;

var availableFrameSourceGroups = await CameraHelper.GetFrameSourceGroupsAsync();
if(availableFrameSourceGroups != null)
{
  CameraHelper cameraHelper = new CameraHelper() { FrameSourceGroup = availableFrameSourceGroups.FirstOrDefault() };
  var result = await cameraHelper.InitializeAndStartCaptureAsync();

  // Camera Initialization succeeded
  if(result == CameraHelperResult.Success)
  {
    // Subscribe to get frames as they arrive
    cameraHelper.FrameArrived += CameraHelper_FrameArrived;

    // Optionally set a different frame source format
    var newFormat = cameraHelper.FrameFormatsAvailable.Find((format) => format.VideoFormat.Width == 640);
    if (newFormat != null)
    {
      await cameraHelper.PreviewFrameSource.SetFormatAsync(newFormat);
    }
  }
}
```
