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

[ToolkitSample(id: nameof(KeyboardDebounceSample), "DispatcherQueueTimer Debounce Keyboard", description: "A sample for showing how to use the DispatcherQueueTimer Debounce extension to smooth keyboard input.")]
[ToolkitSampleNumericOption("Interval", 120, 60, 240)]
public sealed partial class KeyboardDebounceSample : Page
{
    public DispatcherQueueTimer _debounceTimer = DispatcherQueue.GetForCurrentThread().CreateTimer();

    public KeyboardDebounceSample()
    {
        InitializeComponent();
    }

    private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (sender is TextBox textBox)
        {
            _debounceTimer.Debounce(() =>
                {
                    ResultText.Text = textBox.Text;
                },
                //// i.e. if another keyboard press comes in within 120ms of the last, we'll wait before we fire off the request
                interval: TimeSpan.FromMilliseconds(Interval),
                //// If we're blanking out or the first character type, we'll start filtering immediately instead to appear more responsive.
                //// We want to switch back to trailing as the user types more so that we still capture all the input.
                immediate: textBox.Text.Length <= 1);
        }
    }
}
