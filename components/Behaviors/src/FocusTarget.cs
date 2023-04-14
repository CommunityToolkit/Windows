// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace CommunityToolkit.WinUI.Behaviors;

/// <summary>
/// A target for the <see cref="FocusBehavior"/>.
/// </summary>
public sealed partial class FocusTarget : DependencyObject
{
    /// <summary>
    /// The DP to store the <see cref="Control"/> property value.
    /// </summary>
    public static readonly DependencyProperty ControlProperty = DependencyProperty.Register(
        nameof(Control),
        typeof(Control),
        typeof(FocusTarget),
        new PropertyMetadata(null, OnControlChanged));

    /// <summary>
    /// Raised when <see cref="Control"/> property changed.
    /// It can change if we use x:Load on the control.
    /// </summary>
    public event EventHandler? ControlChanged;

    /// <summary>
    /// Gets or sets the control that will receive the focus.
    /// </summary>
    public Control Control
    {
        get => (Control)GetValue(ControlProperty);
        set => SetValue(ControlProperty, value);
    }

    private static void OnControlChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var target = (FocusTarget)d;
        target.ControlChanged?.Invoke(target, EventArgs.Empty);
    }
}
#pragma warning restore SA1402 // File may only contain a single type
