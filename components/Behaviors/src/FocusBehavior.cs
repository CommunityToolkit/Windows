// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#if WINDOWS_UWP || HAS_UNO && WINUI2
using Windows.System;
#elif WINDOWS_WINAPPSDK || HAS_UNO && WINUI3
using Microsoft.UI.Dispatching;
#endif

namespace CommunityToolkit.WinUI.Behaviors;

#pragma warning disable SA1402 // File may only contain a single type
/// <summary>
/// This behavior sets the focus on the first control of <see cref="Targets"/> which accepts it.
/// The focus will be set following the <see cref="Targets"/> order. The first control being ready
/// and accepting the focus will receive it.
/// The focus can be set to another control with a higher priority if it loads before <see cref="FocusEngagementTimeout"/>.
/// The focus can be set to another control if some controls will be loaded/unloaded later.
/// </summary>
[ContentProperty(Name = nameof(Targets))]
public sealed class FocusBehavior : BehaviorBase<UIElement>
{
    /// <summary>
    /// The DP to store the <see cref="Targets"/> property value.
    /// </summary>
    public static readonly DependencyProperty TargetsProperty = DependencyProperty.Register(
        nameof(Targets),
        typeof(FocusTargetList),
        typeof(FocusBehavior),
        new PropertyMetadata(null, OnTargetsPropertyChanged));

    /// <summary>
    /// The DP to store the <see cref="FocusEngagementTimeout"/> property value.
    /// </summary>
    public static readonly DependencyProperty FocusEngagementTimeoutProperty = DependencyProperty.Register(
        nameof(FocusEngagementTimeout),
        typeof(TimeSpan),
        typeof(FocusBehavior),
        new PropertyMetadata(TimeSpan.FromMilliseconds(100)));

    private DispatcherQueueTimer _timer;

    /// <summary>
    /// Initializes a new instance of the <see cref="FocusBehavior"/> class.
    /// </summary>
    public FocusBehavior()
    {
        _timer = DispatcherQueue.GetForCurrentThread().CreateTimer();
        _timer.Tick += OnEngagementTimerTick;

        Targets = new FocusTargetList();
    }

    /// <summary>
    /// Gets or sets the ordered list of controls which should receive the focus when the associated object is loaded.
    /// </summary>
    public FocusTargetList Targets
    {
        get => (FocusTargetList)GetValue(TargetsProperty);
        set => SetValue(TargetsProperty, value);
    }

    /// <summary>
    /// Gets or sets the timeout before the <see cref="FocusBehavior"/> stops trying to set the focus to a control with
    /// a higher priority.
    /// </summary>
    public TimeSpan FocusEngagementTimeout
    {
        get => (TimeSpan)GetValue(FocusEngagementTimeoutProperty);
        set => SetValue(FocusEngagementTimeoutProperty, value);
    }

    /// <inheritdoc/>
    protected override void OnAssociatedObjectLoaded()
    {
        foreach (var target in Targets)
        {
            target.ControlChanged += OnTargetControlChanged;
        }

        ApplyFocus();
    }

    /// <inheritdoc/>
    protected override bool Uninitialize()
    {
        foreach (var target in Targets)
        {
            target.ControlChanged -= OnTargetControlChanged;
        }

        Stop();
        return true;
    }

    private static void OnTargetsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var behavior = (FocusBehavior)d;

        if (e.OldValue is FocusTargetList oldTargets)
        {
            behavior.Stop(oldTargets);
        }

        behavior.ApplyFocus();
    }

    private void ApplyFocus()
    {
        if (Targets.Count == 0)
        {
            return;
        }

        var focusedControlIndex = -1;
        var hasListViewBaseControl = false;
        for (var i = 0; i < Targets.Count; i++)
        {
            var control = Targets[i].Control;
            if (control is null)
            {
                continue;
            }

            if (control.IsLoaded)
            {
                if (control.Focus(FocusState.Programmatic))
                {
                    focusedControlIndex = i;
                    break;
                }

                if (control is ListViewBase listViewBase)
                {
                    listViewBase.ContainerContentChanging -= OnContainerContentChanging;
                    listViewBase.ContainerContentChanging += OnContainerContentChanging;
                    hasListViewBaseControl = true;
                }
            }
            else
            {
                control.Loaded -= OnControlLoaded;
                control.Loaded += OnControlLoaded;
            }
        }

        if (focusedControlIndex == 0 || (!hasListViewBaseControl && Targets.All(t => t.Control?.IsLoaded == true)))
        {
            Stop();
        }
        else if (focusedControlIndex > 0)
        {
            // We have been able to set the focus on one control.
            // We start the timer to detect if we can focus another control with an higher priority.
            // This allows us to handle the case where the controls are not loaded in the order we expect.
            _timer.Interval = FocusEngagementTimeout;
            _timer.Start();
        }
    }

    private void OnEngagementTimerTick(object? sender, object e)
    {
        ApplyFocus();
        Stop();
    }

    private void OnControlLoaded(object? sender, RoutedEventArgs e) => ApplyFocus();

    private void OnTargetControlChanged(object? sender, EventArgs e) => ApplyFocus();

    private void OnContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
    {
        sender.ContainerContentChanging -= OnContainerContentChanging;
        ApplyFocus();
    }

    private void Stop(FocusTargetList? targets = null)
    {
        if (_timer.IsRunning)
        {
            _timer.Stop();
        }

        foreach (var target in targets ?? Targets)
        {
            if (target.Control is null)
            {
                continue;
            }

            target.Control.Loaded -= OnControlLoaded;

            if (target.Control is ListViewBase listViewBase)
            {
                listViewBase.ContainerContentChanging -= OnContainerContentChanging;
            }
        }
    }
}
