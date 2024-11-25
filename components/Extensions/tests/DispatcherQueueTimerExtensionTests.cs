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
        Assert.AreEqual(0, triggeredCount, "Function shouldn't have run yet.");
        Assert.IsNull(triggeredValue, "Function shouldn't have run yet.");

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

        await Task.Delay(TimeSpan.FromMilliseconds(80));

        Assert.AreEqual(false, debounceTimer.IsRunning, "Expected to stop the timer.");
    }

    /// <summary>
    /// Tests the immediate mode of the Debounce function ignoring subsequent inputs that come after the first within the specified time window.
    /// For instance, this could be useful to ignore extra multiple clicks on a button, but immediately start processing upon the first click.
    /// </summary>
    /// <returns></returns>
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
            TimeSpan.FromMilliseconds(60), true); // Ensure we're interrupting with immediate again

        Assert.AreEqual(true, debounceTimer.IsRunning, "Expected time to be running.");
        Assert.AreEqual(1, triggeredCount, "2nd request coming within first period should have been ignored.");
        Assert.AreEqual(value, triggeredValue, "Value shouldn't have changed from 2nd request within time bound.");

        await Task.Delay(TimeSpan.FromMilliseconds(110));

        Assert.AreEqual(false, debounceTimer.IsRunning, "Expected to stop the timer.");
        Assert.AreEqual(value, triggeredValue, "Expected to execute only the first action.");
        Assert.AreEqual(1, triggeredCount, "Expected 2nd request to be ignored.");
    }

    /// <summary>
    /// Tests the scenario where we flip from wanting trailing to leading edge invocaton.
    /// For instance, this could be for a case where a user has cleared the textbox, so you
    /// want to immediately return new results vs. waiting for further input.
    /// </summary>
    /// <returns></returns>
    [TestCategory("DispatcherQueueTimerExtensions")]
    [UIThreadTestMethod]
    public async Task DispatcherQueueTimer_Debounce_Trailing_Switch_Leading_Interrupt()
    {
        var debounceTimer = DispatcherQueue.GetForCurrentThread().CreateTimer();

        var triggeredCount = 0;
        string? triggeredValue = null;

        var value = "Hello";
        debounceTimer.Debounce(
            () =>
            {
                triggeredCount++;
                triggeredValue = value;
            },
            TimeSpan.FromMilliseconds(100), false); // Start off waiting

        // Intentional pause to mimic reality
        await Task.Delay(TimeSpan.FromMilliseconds(30));

        Assert.AreEqual(true, debounceTimer.IsRunning, "Expected time to be running.");
        Assert.AreEqual(0, triggeredCount, "Function shouldn't have run yet.");
        Assert.IsNull(triggeredValue, "Function shouldn't have run yet.");

        // Now interrupt with a scenario we want processed immediately, i.e. user started typing something new
        var value2 = "He";
        debounceTimer.Debounce(
            () =>
            {
                triggeredCount++;
                triggeredValue = value2;
            },
            TimeSpan.FromMilliseconds(100), true);

        Assert.AreEqual(true, debounceTimer.IsRunning, "Expected timer should still be running.");
        Assert.AreEqual(1, triggeredCount, "Function should now have run immediately.");
        Assert.AreEqual(value2, triggeredValue, "Function should have set value to 'He'");

        // Wait to where all should be done
        await Task.Delay(TimeSpan.FromMilliseconds(120));

        Assert.AreEqual(false, debounceTimer.IsRunning, "Expected to stop the timer.");
        Assert.AreEqual(value2, triggeredValue, "Expected value to remain the same.");
        Assert.AreEqual(1, triggeredCount, "Expected to interrupt execution and ignore initial queued exectution.");
    }

    /// <summary>
    /// Tests where we start with immediately processing a delay, but then want to switch to processing after.
    /// For instance, maybe we want to ensure we start processing the first letter of a search query to filter initial results.
    /// But then later want to delay and wait to execute until all the query string is available.
    /// </summary>
    /// <returns></returns>
    [TestCategory("DispatcherQueueTimerExtensions")]
    [UIThreadTestMethod]
    public async Task DispatcherQueueTimer_Debounce_Leading_Switch_Trailing_Interrupt_Twice()
    {
        var debounceTimer = DispatcherQueue.GetForCurrentThread().CreateTimer();

        var triggeredCount = 0;
        string? triggeredValue = null;

        var value = "H";
        debounceTimer.Debounce(
            () =>
            {
                triggeredCount++;
                triggeredValue = value;
            },
            TimeSpan.FromMilliseconds(100), true); // Start off right away

        Assert.AreEqual(true, debounceTimer.IsRunning, "Expected time to be running.");
        Assert.AreEqual(1, triggeredCount, "Function should have run right away.");
        Assert.AreEqual(value, triggeredValue, "Function should have set value immediately.");

        // Pragmatic pause
        await Task.Delay(TimeSpan.FromMilliseconds(30));

        // Now interrupt with more data two times.
        var value2 = "Hel";
        debounceTimer.Debounce(
            () =>
            {
                triggeredCount++;
                triggeredValue = value2;
            },
            TimeSpan.FromMilliseconds(100), false); // We want to ensure we catch the latest data now

        Assert.AreEqual(true, debounceTimer.IsRunning, "Expected timer to still to be running.");
        Assert.AreEqual(1, triggeredCount, "Function should now haven't run again yet.");
        Assert.AreEqual(value, triggeredValue, "Function should still be the initial value");

        // Pragmatic pause again
        await Task.Delay(TimeSpan.FromMilliseconds(30));

        var value3 = "Hello";
        debounceTimer.Debounce(
            () =>
            {
                triggeredCount++;
                triggeredValue = value3;
            },
            TimeSpan.FromMilliseconds(100), false); // We want to ensure we catch the latest data now

        Assert.AreEqual(true, debounceTimer.IsRunning, "Expected timer to still to be running.");
        Assert.AreEqual(1, triggeredCount, "Function should still now haven't run again yet.");
        Assert.AreEqual(value, triggeredValue, "Function should still be the initial value x2");

        // Wait to where the timer should have fired and is done
        await Task.Delay(TimeSpan.FromMilliseconds(120));

        Assert.AreEqual(false, debounceTimer.IsRunning, "Expected timer to stopped at trailing edge to execute latest result.");
        Assert.AreEqual(value3, triggeredValue, "Expected value to now be the last value provided.");
        Assert.AreEqual(2, triggeredCount, "Expected to interrupt execution of 2nd request.");
    }
}
