// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#if !WINAPPSDK
using Windows.System;
using DQ = Windows.System.DispatcherQueue;
#else
using Microsoft.UI.Dispatching;
using DQ = Microsoft.UI.Dispatching.DispatcherQueue;
#endif

namespace CommunityToolkit.WinUI.Behaviors;

/// <summary>
/// A behavior to add the stacked notification support to <see cref="InfoBar"/>.
/// </summary>
public class StackedNotificationsBehavior : BehaviorBase<MUXC.InfoBar>
{
    private readonly LinkedList<Notification> _stackedNotifications;
    private readonly DispatcherQueueTimer _dismissTimer;
    private Notification? _currentNotification;
    private string? _initialTitle;
    private MUXC.InfoBarSeverity? _initialSeverity;
    private bool _initialIconVisible;
    private MUXC.IconSource? _initialIconSource;
    private object? _initialContent;
    private DataTemplate? _initialContentTemplate;
    private ButtonBase? _initialActionButton;

    /// <summary>
    /// Initializes a new instance of the <see cref="StackedNotificationsBehavior"/> class.
    /// </summary>
    public StackedNotificationsBehavior()
    {
        _stackedNotifications = new LinkedList<Notification>();

        // TODO: On WinUI 3 we can use the local DispatcherQueue, so we need to abstract better for UWP
        var dispatcherQueue = DQ.GetForCurrentThread();
        _dismissTimer = dispatcherQueue.CreateTimer();
        _dismissTimer.Tick += OnTimerTick;
    }

    //// Provided as a simple helper to migrate from older InAppNotification control.
    /// <summary>
    /// Show notification using text as the <see cref="MUXC.InfoBar.Message"/> of the notification.
    /// </summary>
    /// <param name="message"><see cref="MUXC.InfoBar.Message"/> string to display as the notification.</param>
    /// <param name="duration">Optional, displayed duration of the notification in ms (less or equal 0 means infinite duration).</param>
    /// <param name="title">Optional, <see cref="MUXC.InfoBar.Title"/> for the notification.</param>
    /// <returns>The constructed <see cref="Notification"/> added to the queue.</returns>
    public Notification Show(string message, int duration = 0, string? title = null)
    {
        Notification notification = new()
        {
            Title = title,
            Message = message,
            Duration = duration <= 0 ? null : TimeSpan.FromMilliseconds(duration)
        };

        return Show(notification);
    }

    //// Provided as a simple helper to migrate from older InAppNotification control.
    /// <summary>
    /// Show notification using object or UIElement as the <see cref="MUXC.InfoBar.Content"/> of the notification. Note, it is
    /// generally best to also specific a message and/or title.
    /// </summary>
    /// <param name="content">Content to display as the notification.</param>
    /// <param name="duration">Optional, displayed duration of the notification in ms (less or equal 0 means infinite duration).</param>
    /// <param name="title">Optional, <see cref="MUXC.InfoBar.Title"/> for the notification.</param>
    /// <param name="message">Optional, <see cref="MUXC.InfoBar.Message"/> string to display as the notification.</param>
    /// <returns>The constructed <see cref="Notification"/> added to the queue.</returns>
    public Notification Show(object content, int duration = 0, string? title = null, string? message = null)
    {
        Notification notification = new()
        {
            Title = title,
            Message = message,
            Content = content,
            Duration = duration <= 0 ? null : TimeSpan.FromMilliseconds(duration)
        };

        return Show(notification);
    }

    /// <summary>
    /// Show <paramref name="notification"/>.
    /// </summary>
    /// <param name="notification">The notification to display.</param>
    /// <returns>The <see cref="Notification"/> added to the queue.</returns>
    public Notification Show(Notification notification)
    {
        if (notification is null)
        {
            throw new ArgumentNullException(nameof(notification));
        }

        _stackedNotifications.AddLast(notification);
        ShowNext();

        return notification;
    }

    /// <summary>
    /// Remove the <paramref name="notification"/>.
    /// If the notification is displayed, it will be closed.
    /// If the notification is still in the queue, it will be removed.
    /// </summary>
    /// <param name="notification">The notification to remove.</param>
    public void Remove(Notification notification)
    {
        if (notification is null)
        {
            throw new ArgumentNullException(nameof(notification));
        }

        if (notification == _currentNotification)
        {
            // We close the notification. This will trigger the display of the next one.
            // See OnInfoBarClosed.
            AssociatedObject.IsOpen = false;
            return;
        }

        _stackedNotifications.Remove(notification);
    }

    /// <inheritdoc/>
    protected override bool Initialize()
    {
        AssociatedObject.Closed += OnInfoBarClosed;
        AssociatedObject.PointerEntered += OnPointerEntered;
        AssociatedObject.PointerExited += OnPointedExited;
        return true;
    }

    /// <inheritdoc/>
    protected override bool Uninitialize()
    {
        AssociatedObject.Closed -= OnInfoBarClosed;
        AssociatedObject.PointerEntered -= OnPointerEntered;
        AssociatedObject.PointerExited -= OnPointedExited;
        return true;
    }

    private void OnInfoBarClosed(MUXC.InfoBar sender, MUXC.InfoBarClosedEventArgs args)
    {
        // We display the next notification.
        ShowNext();
    }

    private void ShowNext()
    {
        if (AssociatedObject.IsOpen)
        {
            // One notification is already displayed. We wait for it to close
            return;
        }

        StopTimer();
        AssociatedObject.IsOpen = false;
        RestoreOverridenProperties();

        if (_stackedNotifications.Count == 0)
        {
            _currentNotification = null;
            return;
        }

        var notificationToDisplay = _stackedNotifications!.First!.Value;
        _stackedNotifications.RemoveFirst();

        _currentNotification = notificationToDisplay;
        SetNotification(notificationToDisplay);
        AssociatedObject.IsOpen = true;

        StartTimer(notificationToDisplay.Duration);
    }

    private void SetNotification(Notification notification)
    {
        if (notification.Title != null)
        {
            _initialTitle = AssociatedObject.Title;
            AssociatedObject.Title = notification.Title;
        }
        
        AssociatedObject.Message = notification.Message ?? string.Empty;

        if (notification.Overrides.HasFlag(NotificationOverrides.Severity))
        {
            _initialSeverity = AssociatedObject.Severity;
            AssociatedObject.Severity = notification.Severity!.Value;
        }

        if (notification.Overrides.HasFlag(NotificationOverrides.IconVisible))
        {
            _initialIconVisible = AssociatedObject.IsIconVisible;
            AssociatedObject.IsIconVisible = notification.IsIconVisible;
        }

        if (notification.Overrides.HasFlag(NotificationOverrides.IconSource))
        {
            _initialIconSource = AssociatedObject.IconSource;
            AssociatedObject.IconSource = notification.IconSource;
        }

        if (notification.Overrides.HasFlag(NotificationOverrides.Content))
        {
            _initialContent = AssociatedObject.Content;
            AssociatedObject.Content = notification.Content!;
        }

        if (notification.Overrides.HasFlag(NotificationOverrides.ContentTemplate))
        {
            _initialContentTemplate = AssociatedObject.ContentTemplate;
            AssociatedObject.ContentTemplate = notification.ContentTemplate!;
        }

        if (notification.Overrides.HasFlag(NotificationOverrides.ActionButton))
        {
            _initialActionButton = AssociatedObject.ActionButton;
            AssociatedObject.ActionButton = notification.ActionButton!;
        }
    }

    private void RestoreOverridenProperties()
    {
        if (_currentNotification is null)
        {
            return;
        }

        if (_currentNotification.Title != null)
        {
            AssociatedObject.Title = _initialTitle;
        }

        if (_currentNotification.Overrides.HasFlag(NotificationOverrides.Severity))
        {
            AssociatedObject.Severity = _initialSeverity!.Value;
        }

        if (_currentNotification.Overrides.HasFlag(NotificationOverrides.IconVisible))
        {
            AssociatedObject.IsIconVisible = _initialIconVisible;
        }

        if (_currentNotification.Overrides.HasFlag(NotificationOverrides.IconSource))
        {
            AssociatedObject.IconSource = _initialIconSource;
        }

        if (_currentNotification.Overrides.HasFlag(NotificationOverrides.Content))
        {
            AssociatedObject.Content = _initialContent!;
        }

        if (_currentNotification.Overrides.HasFlag(NotificationOverrides.ContentTemplate))
        {
            AssociatedObject.ContentTemplate = _initialContentTemplate!;
        }

        if (_currentNotification.Overrides.HasFlag(NotificationOverrides.ActionButton))
        {
            AssociatedObject.ActionButton = _initialActionButton!;
        }
    }

    private void StartTimer(TimeSpan? duration)
    {
        if (duration is null)
        {
            return;
        }

        _dismissTimer.Interval = duration.Value;
        _dismissTimer.Start();
    }

    private void StopTimer() => _dismissTimer.Stop();

    private void OnTimerTick(DispatcherQueueTimer sender, object args) => AssociatedObject.IsOpen = false;

    private void OnPointedExited(object sender, PointerRoutedEventArgs e) => StartTimer(_currentNotification?.Duration);

    private void OnPointerEntered(object sender, PointerRoutedEventArgs e) => StopTimer();
}
