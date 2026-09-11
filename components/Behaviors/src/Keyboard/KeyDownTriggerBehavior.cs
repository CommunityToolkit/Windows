// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#if WINUI3
using Microsoft.UI.Input;
#endif
using Microsoft.Xaml.Interactivity;
using Windows.System;
using Windows.UI.Core;

namespace CommunityToolkit.WinUI.Behaviors;

/// <summary>
/// A behavior that listens to <see cref="UIElement.PreviewKeyDown"/> on the associated
/// <see cref="FrameworkElement"/> and executes its actions when the specified key and
/// optional modifier keys are pressed. Supports capturing handled events.
/// </summary>
[TypeConstraint(typeof(FrameworkElement))]
public class KeyDownTriggerBehavior : Trigger<FrameworkElement>
{
    private KeyEventHandler? _handler;

    /// <summary>
    /// Identifies the <see cref="Key"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty KeyProperty =
        DependencyProperty.Register(
            nameof(Key),
            typeof(VirtualKey),
            typeof(KeyDownTriggerBehavior),
            new PropertyMetadata(VirtualKey.None));

    /// <summary>
    /// Gets or sets the key that triggers the behavior.
    /// </summary>
    public VirtualKey Key
    {
        get => (VirtualKey)GetValue(KeyProperty);
        set => SetValue(KeyProperty, value);
    }

    /// <summary>
    /// Identifies the <see cref="Modifiers"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ModifiersProperty =
        DependencyProperty.Register(
            nameof(Modifiers),
            typeof(VirtualKeyModifiers),
            typeof(KeyDownTriggerBehavior),
            new PropertyMetadata(VirtualKeyModifiers.None));

    /// <summary>
    /// Gets or sets the modifier keys that must be pressed together with <see cref="Key"/>.
    /// </summary>
    public VirtualKeyModifiers Modifiers
    {
        get => (VirtualKeyModifiers)GetValue(ModifiersProperty);
        set => SetValue(ModifiersProperty, value);
    }

    /// <summary>
    /// Identifies the <see cref="ModifiersEnabled"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ModifiersEnabledProperty =
        DependencyProperty.Register(
            nameof(ModifiersEnabled),
            typeof(bool),
            typeof(KeyDownTriggerBehavior),
            new PropertyMetadata(false));

    /// <summary>
    /// Gets or sets a value indicating whether the behavior should evaluate
    /// the current modifier key state when matching the <see cref="Key"/>.
    ///
    /// When <see langword="false"/> (default), the behavior ignores the state of
    /// modifier keys and triggers solely based on the <see cref="Key"/> value,
    /// preserving the original behavior.
    ///
    /// When <see langword="true"/>, the behavior requires the modifier keys
    /// specified in <see cref="Modifiers"/> to match the current keyboard state
    /// before triggering.
    /// </summary>
    public bool ModifiersEnabled
    {
        get => (bool)GetValue(ModifiersEnabledProperty);
        set => SetValue(ModifiersEnabledProperty, value);
    }

    /// <summary>
    /// Identifies the <see cref="HandledEventsToo"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty HandledEventsTooProperty =
        DependencyProperty.Register(
            nameof(HandledEventsToo),
            typeof(bool),
            typeof(KeyDownTriggerBehavior),
            new PropertyMetadata(true));

    /// <summary>
    /// Gets or sets a value indicating whether the behavior should receive
    /// <see cref="UIElement.PreviewKeyDown"/> events even if they were already handled.
    /// </summary>
    public bool HandledEventsToo
    {
        get => (bool)GetValue(HandledEventsTooProperty);
        set => SetValue(HandledEventsTooProperty, value);
    }

    /// <inheritdoc/>
    protected override void OnAttached()
    {
        _handler = OnPreviewKeyDown;

        AssociatedObject.AddHandler(
            UIElement.PreviewKeyDownEvent,
            _handler,
            HandledEventsToo);
    }

    /// <inheritdoc/>
    protected override void OnDetaching()
    {
        if (_handler is not null)
        {
            AssociatedObject.RemoveHandler(
                UIElement.PreviewKeyDownEvent,
                _handler);

            _handler = null;
        }
    }

    /// <summary>
    /// Handles the <see cref="UIElement.PreviewKeyDown"/> event and executes the associated actions
    /// when the specified <see cref="Key"/> and <see cref="Modifiers"/> match.
    /// </summary>
    /// <param name="sender">The source <see cref="UIElement"/> instance.</param>
    /// <param name="keyRoutedEventArgs">The arguments for the event (unused).</param>
    private void OnPreviewKeyDown(object sender, KeyRoutedEventArgs keyRoutedEventArgs)
    {
        if (keyRoutedEventArgs.Key != Key)
        {
            return;
        }

        if (ModifiersEnabled && !CheckModifiers())
        {
            return;
        }

        keyRoutedEventArgs.Handled = true;
        Interaction.ExecuteActions(sender, Actions, keyRoutedEventArgs);
    }

    /// <summary>
    /// Checks whether all required modifier keys specified in <see cref="Modifiers"/>
    /// are currently pressed. Retrieves the physical key states once and evaluates
    /// them against the required modifier flags.
    /// </summary>
    /// <returns><see langword="true"/> if the current modifier state matches the requirements; otherwise, <see langword="false"/>.</returns>
    private bool CheckModifiers()
    {
#if WINUI3
        bool ctrl = InputKeyboardSource.GetKeyStateForCurrentThread(VirtualKey.Control).HasFlag(CoreVirtualKeyStates.Down);
        bool shift = InputKeyboardSource.GetKeyStateForCurrentThread(VirtualKey.Shift).HasFlag(CoreVirtualKeyStates.Down);
        bool alt = InputKeyboardSource.GetKeyStateForCurrentThread(VirtualKey.Menu).HasFlag(CoreVirtualKeyStates.Down);
#else
        // In WinUI 2 we cannot use InputKeyboardSource, so we fall back to the CoreWindow API, which is less reliable in certain scenarios
        // (e.g. when the app is not active or in multi-window scenarios), but it's the best we have.

        var coreWindow = Window.Current?.CoreWindow;
        if (coreWindow is null)
        {
            return false;
        };

        bool ctrl = coreWindow.GetKeyState(VirtualKey.Control).HasFlag(CoreVirtualKeyStates.Down);
        bool shift = coreWindow.GetKeyState(VirtualKey.Shift).HasFlag(CoreVirtualKeyStates.Down);
        bool alt = coreWindow.GetKeyState(VirtualKey.Menu).HasFlag(CoreVirtualKeyStates.Down);
#endif

        return Match(VirtualKeyModifiers.Control, ctrl)
            && Match(VirtualKeyModifiers.Shift, shift)
            && Match(VirtualKeyModifiers.Menu, alt);
    }

    /// <summary>
    /// Compares whether a specific modifier flag is required and whether the
    /// corresponding key is currently pressed.
    /// </summary>
    /// <param name="mod">The modifier flag to evaluate.</param>
    /// <param name="isDown">The current physical key state for that modifier.</param>
    /// <returns><see langword="true"/> if the requirement matches the key state; otherwise, <see langword="false"/>.</returns>
    private bool Match(VirtualKeyModifiers mod, bool isDown)
    {
        bool required = (Modifiers & mod) != 0;
        return required == isDown;
    }
}
