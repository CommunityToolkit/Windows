// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.Tests;
using CommunityToolkit.Tooling.TestGen;
using CommunityToolkit.WinUI;

#if WINAPPSDK
using DispatcherQueue = Microsoft.UI.Dispatching.DispatcherQueue;
using DispatcherQueuePriority = Microsoft.UI.Dispatching.DispatcherQueuePriority;
using DispatcherQueueTimer = Microsoft.UI.Dispatching.DispatcherQueueTimer;
#else
using Windows.Foundation.Metadata;
using DispatcherQueue = Windows.System.DispatcherQueue;
using DispatcherQueuePriority = Windows.System.DispatcherQueuePriority;
using DispatcherQueueTimer = Windows.System.DispatcherQueueTimer;
#endif

namespace ExtensionsComponent.Tests;

[TestClass]
public partial class DispatcherQueueTimerExtensionTests : VisualUITestBase
{
    [TestCategory("DispatcherQueueTimerExtensions")]
    [UIThreadTestMethod]
    public async Task DispatcherQueueTimer_Debounce_Trailing()
    {
        var debounceTimer = DispatcherQueue.GetForCurrentThread().CreateTimer();

        var triggeredCount = 0;
        string? triggeredValue = null;

        var value = "He";
        debounceTimer.Debounce(
            () =>
            {
                triggeredCount++;
                triggeredValue = value;
            },
            TimeSpan.FromMilliseconds(60));

        Assert.AreEqual(true, debounceTimer.IsRunning, "Expected time to be running.");
        Assert.AreEqual(0, triggeredCount, "Function shouldn't have run yet.");
        Assert.IsNull(triggeredValue, "Function shouldn't have run yet.");

        await Task.Delay(TimeSpan.FromMilliseconds(80));

        Assert.AreEqual(false, debounceTimer.IsRunning, "Expected to stop the timer.");
        Assert.AreEqual(value, triggeredValue, "Expected result to be set.");
        Assert.AreEqual(1, triggeredCount, "Expected to run once.");
    }

    [TestCategory("DispatcherQueueTimerExtensions")]
    [UIThreadTestMethod]
    public async Task DispatcherQueueTimer_Debounce_Trailing_Interrupt()
    {
        var debounceTimer = DispatcherQueue.GetForCurrentThread().CreateTimer();

        var triggeredCount = 0;
        string? triggeredValue = null;

        var value = "He";
        debounceTimer.Debounce(
            () =>
            {
                triggeredCount++;
                triggeredValue = value;
            },
            TimeSpan.FromMilliseconds(60));

        Assert.AreEqual(true, debounceTimer.IsRunning, "Expected time to be running.");
        Assert.AreEqual(0, triggeredCount, "Function shouldn't have run yet.");
        Assert.IsNull(triggeredValue, "Function shouldn't have run yet.");

        var value2 = "Hello";
        debounceTimer.Debounce(
            () =>
            {
                triggeredCount++;
                triggeredValue = value2;
            },
            TimeSpan.FromMilliseconds(60));

        Assert.AreEqual(true, debounceTimer.IsRunning, "Expected time to be running.");

        await Task.Delay(TimeSpan.FromMilliseconds(110));

        Assert.AreEqual(false, debounceTimer.IsRunning, "Expected to stop the timer.");
        Assert.AreEqual(value2, triggeredValue, "Expected to execute the last action.");
        Assert.AreEqual(1, triggeredCount, "Expected to postpone execution.");
    }

    [TestCategory("DispatcherQueueTimerExtensions")]
    [UIThreadTestMethod]
    public async Task DispatcherQueueTimer_Debounce_Immediate()
    {
        var debounceTimer = DispatcherQueue.GetForCurrentThread().CreateTimer();

        var triggeredCount = 0;
        string? triggeredValue = null;

        var value = "He";
        debounceTimer.Debounce(
            () =>
            {
                triggeredCount++;
                triggeredValue = value;
            },
            TimeSpan.FromMilliseconds(60), true);

        Assert.AreEqual(true, debounceTimer.IsRunning, "Expected time to be running.");
        Assert.AreEqual(1, triggeredCount, "Function should have run right away.");
        Assert.AreEqual(value, triggeredValue, "Should have expected immediate set of value");
    }

    [TestCategory("DispatcherQueueTimerExtensions")]
    [UIThreadTestMethod]
    public async Task DispatcherQueueTimer_Debounce_Immediate_Interrupt()
    {
        var debounceTimer = DispatcherQueue.GetForCurrentThread().CreateTimer();

        var triggeredCount = 0;
        string? triggeredValue = null;

        var value = "He";
        debounceTimer.Debounce(
            () =>
            {
                triggeredCount++;
                triggeredValue = value;
            },
            TimeSpan.FromMilliseconds(60), true);

        Assert.AreEqual(true, debounceTimer.IsRunning, "Expected time to be running.");
        Assert.AreEqual(1, triggeredCount, "Function should have run right away.");
        Assert.AreEqual(value, triggeredValue, "Should have expected immediate set of value");

        var value2 = "Hello";
        debounceTimer.Debounce(
            () =>
            {
                triggeredCount++;
                triggeredValue = value2;
            },
            TimeSpan.FromMilliseconds(60));

        Assert.AreEqual(true, debounceTimer.IsRunning, "Expected time to be running.");

        await Task.Delay(TimeSpan.FromMilliseconds(110));

        Assert.AreEqual(false, debounceTimer.IsRunning, "Expected to stop the timer.");
        Assert.AreEqual(value2, triggeredValue, "Expected to execute the last action.");
        Assert.AreEqual(2, triggeredCount, "Expected to postpone execution.");
    }
}
