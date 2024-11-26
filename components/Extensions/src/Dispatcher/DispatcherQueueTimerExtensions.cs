// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Concurrent;
using System.Runtime.CompilerServices;


#if WINAPPSDK
using DispatcherQueueTimer = Microsoft.UI.Dispatching.DispatcherQueueTimer;
#else
using DispatcherQueueTimer = Windows.System.DispatcherQueueTimer;
#endif

namespace CommunityToolkit.WinUI;

/// <summary>
/// Set of extension methods for using <see cref="DispatcherQueueTimer"/>.
/// </summary>
public static class DispatcherQueueTimerExtensions
{
    /// <inheritdoc cref="System.Runtime.CompilerServices.ConditionalWeakTable{TKey,TValue}" />
    private static ConditionalWeakTable<DispatcherQueueTimer, Action> _debounceInstances = new();

    /// <summary>
    /// <para>Used to debounce (rate-limit) an event.  The action will be postponed and executed after the interval has elapsed.  At the end of the interval, the function will be called with the arguments that were passed most recently to the debounced function. Useful for smoothing keyboard input, for instance.</para>
    /// <para>Use this method to control the timer instead of calling Start/Interval/Stop manually.</para>
    /// <para>A scheduled debounce can still be stopped by calling the stop method on the timer instance.</para>
    /// <para>Each timer can only have one debounced function limited at a time.</para>
    /// </summary>
    /// <param name="timer">Timer instance, only one debounced function can be used per timer.</param>
    /// <param name="action">Action to execute at the end of the interval.</param>
    /// <param name="interval">Interval to wait before executing the action.</param>
    /// <param name="immediate">Determines if the action execute on the leading edge instead of trailing edge of the interval. Subsequent input will be ignored into the interval has completed. Useful for ignore extraneous extra input like multiple mouse clicks.</param>
    /// <example>
    /// <code>
    /// private DispatcherQueueTimer _typeTimer = DispatcherQueue.GetForCurrentThread().CreateTimer();
    ///
    /// _typeTimer.Debounce(async () =>
    ///     {
    ///         // Only executes code put here after 0.3 seconds have elapsed since last call to Debounce.
    ///     }, TimeSpan.FromSeconds(0.3));
    /// </code>
    /// </example>
    public static void Debounce(this DispatcherQueueTimer timer, Action action, TimeSpan interval, bool immediate = false)
    {
        // Check and stop any existing timer
        var timeout = timer.IsRunning;
        if (timeout)
        {
            timer.Stop();
        }

        // Reset timer parameters
        timer.Tick -= Timer_Tick;
        timer.Interval = interval;

        // Ensure we haven't been misconfigured and won't execute more times than we expect.
        timer.IsRepeating = false;

        if (immediate)
        {
            // If we have a _debounceInstance queued, then we were running in trailing mode,
            // so if we now have the immediate flag, we should ignore this timer, and run immediately.
            if (_debounceInstances.TryGetValue(timer, out var _))
            {
                timeout = false;

                _debounceInstances.Remove(timer);
            }

            // If we're in immediate mode then we only execute if the timer wasn't running beforehand
            if (!timeout)
            {
                action.Invoke();
            }
        }
        else
        {
            // If we're not in immediate mode, then we'll execute when the current timer expires.
            timer.Tick += Timer_Tick;

            // Store/Update function
            _debounceInstances.AddOrUpdate(timer, action);
        }

        // Start the timer to keep track of the last call here.
        timer.Start();
    }

    private static void Timer_Tick(object sender, object e)
    {
        // This event is only registered/run if we weren't in immediate mode above
        if (sender is DispatcherQueueTimer timer)
        {
            timer.Tick -= Timer_Tick;
            timer.Stop();

            if (_debounceInstances.TryGetValue(timer, out Action? action))
            {
                _debounceInstances.Remove(timer);
                action?.Invoke();
            }
        }
    }
}
