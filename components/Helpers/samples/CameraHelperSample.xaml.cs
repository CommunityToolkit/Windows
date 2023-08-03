// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.WinUI.Helpers;
using Windows.Graphics.Imaging;
using Windows.Media;
using Windows.Media.Capture.Frames;
using Windows.ApplicationModel;

#if WINAPPSDK
using Microsoft.UI.Xaml.Media.Imaging;
#else
using Windows.UI.Xaml.Media.Imaging;
#endif

namespace HelpersExperiment.Samples;

[ToolkitSample(id: nameof(CameraHelperSample), "CameraHelper", description: $"A sample for showing how to use {nameof(CameraHelper)}.")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1001:Types that own disposable fields should be disposable", Justification = "Controls dispose resources on unload")]
public sealed partial class CameraHelperSample : UserControl
{
    private CameraHelper _cameraHelper;
    private VideoFrame _currentVideoFrame;
    private SoftwareBitmapSource _softwareBitmapSource;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public CameraHelperSample()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
        this.InitializeComponent();

        Loaded += CameraHelperSample_Loaded;
        Unloaded += CameraHelperSample_Unloaded;
    }

    private async void CameraHelperSample_Loaded(object sender, RoutedEventArgs e)
    {
        Loaded -= CameraHelperSample_Loaded;

        Setup();
        await InitializeAsync();
    }

    private async void CameraHelperSample_Unloaded(object sender, RoutedEventArgs e)
    {
        Unloaded -= CameraHelperSample_Unloaded;

#if !WINAPPSDK
        Application.Current.Suspending -= Application_Suspending;
        Application.Current.Resuming -= Application_Resuming;
#endif
        await CleanUpAsync();
    }

    private void Setup()
    {
        _softwareBitmapSource = new SoftwareBitmapSource();
        CurrentFrameImage.Source = _softwareBitmapSource;
#if !WINAPPSDK
        // Application.Current.Suspending += Application_Suspending;
        // Application.Current.Resuming += Application_Resuming;
#endif
    }

    private async void Application_Suspending(object? sender, SuspendingEventArgs e)
    {
        if (IsLoaded)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            await CleanUpAsync();
            deferral.Complete();
        }
    }

    private async void Application_Resuming(object? sender, object e)
    {
        await InitializeAsync();
    }

    private void CameraHelper_FrameArrived(object sender, FrameEventArgs e)
    {
        _currentVideoFrame = e.VideoFrame;
    }

    private async Task InitializeAsync()
    {
        var frameSourceGroups = await CameraHelper.GetFrameSourceGroupsAsync();
        if (_cameraHelper == null)
        {
            _cameraHelper = new CameraHelper();
        }

        var result = await _cameraHelper.InitializeAndStartCaptureAsync();
        if (result == CameraHelperResult.Success)
        {
            // Subscribe to the video frame as they arrive
            _cameraHelper.FrameArrived += CameraHelper_FrameArrived!;
            FrameSourceGroupCombo.ItemsSource = frameSourceGroups;
            FrameSourceGroupCombo.SelectionChanged += FrameSourceGroupCombo_SelectionChanged;
            FrameSourceGroupCombo.SelectedIndex = 0;
        }

        SetUIControls(result);
    }

    private async void FrameSourceGroupCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (FrameSourceGroupCombo.SelectedItem is MediaFrameSourceGroup selectedGroup && _cameraHelper != null)
        {
            _cameraHelper.FrameSourceGroup = selectedGroup;
            var result = await _cameraHelper.InitializeAndStartCaptureAsync();
            SetUIControls(result);
        }
    }

    private void SetUIControls(CameraHelperResult result)
    {
        var success = result == CameraHelperResult.Success;
        if (!success)
        {
            _currentVideoFrame = null!;
        }

        ErrorBar.Title = result.ToString();
        ErrorBar.IsOpen = !success;

        CaptureButton.IsEnabled = success;
        CurrentFrameImage.Opacity = success ? 1 : 0.5;
    }

    private async void CaptureButton_Click(object sender, RoutedEventArgs e)
    {
        var softwareBitmap = _currentVideoFrame?.SoftwareBitmap;

        if (softwareBitmap != null)
        {
            if (softwareBitmap.BitmapPixelFormat != BitmapPixelFormat.Bgra8 || softwareBitmap.BitmapAlphaMode == BitmapAlphaMode.Straight)
            {
                softwareBitmap = SoftwareBitmap.Convert(softwareBitmap, BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied);
            }
            if (_softwareBitmapSource != null)
            {
                await _softwareBitmapSource.SetBitmapAsync(softwareBitmap);
            }
        }
    }

    private async Task CleanUpAsync()
    {
        if (FrameSourceGroupCombo != null)
        {
            FrameSourceGroupCombo.SelectionChanged -= FrameSourceGroupCombo_SelectionChanged;
        }

        if (_cameraHelper != null)
        {
            _cameraHelper.FrameArrived -= CameraHelper_FrameArrived!;
            await _cameraHelper.CleanUpAsync();
            _cameraHelper = null!;
        }

        _softwareBitmapSource?.Dispose();
    }
}
