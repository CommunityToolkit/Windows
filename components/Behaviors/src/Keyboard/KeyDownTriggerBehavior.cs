// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Xaml.Interactivity;
using Windows.System;

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

        if (!CheckModifiers())
        {
            return;
        }

        keyRoutedEventArgs.Handled = true;
        Interaction.ExecuteActions(sender, Actions, keyRoutedEventArgs);
    }

    /// <summary>
    /// Checks whether all required modifier keys specified in <see cref="Modifiers"/>
    /// are currently pressed.
    /// </summary>
    /// <returns><see langword="true"/> if the modifier state matches; otherwise, <see langword="false"/>.</returns>

    private bool CheckModifiers() =>
        Match(VirtualKeyModifiers.Control, VirtualKey.Control) &&
        Match(VirtualKeyModifiers.Shift, VirtualKey.Shift) &&
        Match(VirtualKeyModifiers.Menu, VirtualKey.Menu);

    /// <summary>
    /// Determines whether a specific modifier key is required and whether it is currently pressed.
    /// </summary>
    /// <param name="mod">The modifier flag to test.</param>
    /// <param name="key">The physical key corresponding to the modifier.</param>
    /// <returns><see langword="true"/> if the modifier requirement matches the current key state; otherwise, <see langword="false"/>.</returns>
    private bool Match(VirtualKeyModifiers mod, VirtualKey key)
    {
        bool required = (Modifiers & mod) != 0;
        bool pressed = IsDown(key);
        return required == pressed;
    }

    /// <summary>
    /// Checks whether the specified key is currently in the <see cref="CoreVirtualKeyStates.Down"/> state.
    /// </summary>
    /// <param name="key">The key to test.</param>
    /// <returns><see langword="true"/> if the key is pressed; otherwise, <see langword="false"/>.</returns>
    private static bool IsDown(VirtualKey key)
    {
        var state = InputKeyboardSource.GetKeyStateForCurrentThread(key);
        return state.HasFlag(CoreVirtualKeyStates.Down);
    }
}
