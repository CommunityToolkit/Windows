// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.WinUI;
#if WINAPPSDK
using DispatcherQueue = Microsoft.UI.Dispatching.DispatcherQueue;
using DispatcherQueueTimer = Microsoft.UI.Dispatching.DispatcherQueueTimer;
#else
using DispatcherQueue = Windows.System.DispatcherQueue;
using DispatcherQueueTimer = Windows.System.DispatcherQueueTimer;
#endif

namespace ExtensionsExperiment.Samples.DispatcherQueueExtensions;

[ToolkitSample(id: nameof(MouseDebounceSample), "DispatcherQueueTimer Debounce Mouse", description: "A sample for showing how to use the DispatcherQueueTimer Debounce extension to smooth mouse input.")]
[ToolkitSampleNumericOption("Interval", 400, 300, 1000)]
public sealed partial class MouseDebounceSample : Page
{
    public DispatcherQueueTimer _debounceTimer = DispatcherQueue.GetForCurrentThread().CreateTimer();

    private int _count = 0;

    public MouseDebounceSample()
    {
        InitializeComponent();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        _debounceTimer.Debounce(() =>
            {
                ResultText.Text = $"You hit the button {++_count} times!";
            },
            interval: TimeSpan.FromMilliseconds(Interval),
            // By being on the leading edge, we ignore inputs past the first for the duration of the interval
            immediate: true);
    }
}
