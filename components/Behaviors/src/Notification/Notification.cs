// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace CommunityToolkit.WinUI.Behaviors;

/// <summary>
/// The content of a notification to display in <see cref="StackedNotificationsBehavior"/>.
/// The <see cref="Message"/> and <see cref="Duration"/> values will
/// always be applied to the targeted <see cref="MUXC.InfoBar"/>.
/// The <see cref="Title"/>, <see cref="Severity"/>, <see cref="IsIconVisible"/>, <see cref="IconSource"/>, <see cref="Content"/>, <see cref="ContentTemplate"/> and <see cref="ActionButton"/> values
/// will be applied only if set, otherwise the parent <see cref="MUXC.InfoBar"/> values will be used, if available.
/// </summary>
public class Notification
{
    private NotificationOverrides _overrides;
    private MUXC.InfoBarSeverity? _severity;
    private bool _isIconVisible = true; // Default for InfoBar
    private MUXC.IconSource? _iconSource;
    private object? _content;
    private DataTemplate? _contentTemplate;
    private ButtonBase? _actionButton;

    /// <summary>
    /// Gets or sets the notification title.
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets the notification message.
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// Gets or sets the duration of the notification.
    /// Set to null for persistent notification (default).
    /// </summary>
    public TimeSpan? Duration { get; set; }

    /// <summary>
    /// Gets or sets the type of the <see cref="MUXC.InfoBar.Severity"/> to apply consistent status color, icon,
    /// and assistive technology settings dependent on the criticality of the notification.
    /// By default the parent <see cref="MUXC.InfoBar.Severity"/> property's value will be used.
    /// </summary>
    public MUXC.InfoBarSeverity? Severity
    {
        get => _severity;
        set
        {
            _severity = value;
            _overrides |= NotificationOverrides.Severity;
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the icon is visible or not.
    /// True if the icon is visible; otherwise, false. The default is true.
    /// </summary>
    public bool IsIconVisible
    {
        get => _isIconVisible;
        set
        {
            _isIconVisible = value;
            _overrides |= NotificationOverrides.IconVisible;
        }
    }

    /// <summary>
    /// Gets or sets a value for an <see cref="MUXC.IconSource"/> to use as the <see cref="MUXC.InfoBar.IconSource"/> of the <see cref="MUXC.InfoBar"/> for this notification.
    /// </summary>
    public MUXC.IconSource? IconSource
    {
        get => _iconSource;
        set
        {
            _iconSource = value;
            _overrides |= NotificationOverrides.IconSource;
        }
    }

    /// <summary>
    /// Gets or sets the XAML Content that is displayed below the title and message in
    ///  the InfoBar.
    /// </summary>
    public object? Content
    {
        get => _content;
        set
        {
            _content = value;
            _overrides |= NotificationOverrides.Content;
        }
    }

    /// <summary>
    /// Gets or sets the data template for the <see cref="Content"/>.
    /// </summary>
    public DataTemplate? ContentTemplate
    {
        get => _contentTemplate;
        set
        {
            _contentTemplate = value;
            _overrides |= NotificationOverrides.ContentTemplate;
        }
    }

    /// <summary>
    /// Gets or sets the action button of the InfoBar.
    /// </summary>
    public ButtonBase? ActionButton
    {
        get => _actionButton;
        set
        {
            _actionButton = value;
            _overrides |= NotificationOverrides.ActionButton;
        }
    }

    internal NotificationOverrides Overrides => _overrides;
}

/// <summary>
/// The overrides which should be set on the notification.
/// </summary>
[Flags]
internal enum NotificationOverrides
{
    None,
    Severity,
    IconVisible,
    IconSource,
    Content,
    ContentTemplate,
    ActionButton,
}
