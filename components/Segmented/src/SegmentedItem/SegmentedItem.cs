// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace CommunityToolkit.WinUI.Controls;

/// <summary>
/// Represents an item in a <see cref="Segmented"/> control.
/// </summary>
[ContentProperty(Name = nameof(Content))]
public partial class SegmentedItem : ListViewItem
{
    internal const string IconLeftState = "IconLeft";
    internal const string IconTopState = "IconTop";
    internal const string IconOnlyState = "IconOnly";
    internal const string ContentOnlyState = "ContentOnly";

    internal const string HorizontalState = "Horizontal";
    internal const string VerticalState = "Vertical";

    private bool _isVertical = false;

    /// <summary>
    /// Creates a new instance of <see cref="SegmentedItem"/>.
    /// </summary>
    public SegmentedItem()
    {
        this.DefaultStyleKey = typeof(SegmentedItem);
    }

    /// <inheritdoc/>
    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        UpdateVisualStates();
    }

    /// <summary>
    /// Handles changes to the Content property.
    /// </summary>
    protected override void OnContentChanged(object oldContent, object newContent)
    {
        base.OnContentChanged(oldContent, newContent);
        UpdateVisualStates();
    }

    /// <summary>
    /// Handles changes to the Icon property.
    /// </summary>
    protected virtual void OnIconPropertyChanged(IconElement oldValue, IconElement newValue) => UpdateVisualStates();

    internal void UpdateOrientation(Orientation orientation)
    {
        _isVertical = orientation is Orientation.Vertical;
        UpdateVisualStates();
    }

    private void UpdateVisualStates()
    {
        string contentState = (Icon is null, Content is null) switch
        {
            (false, false) => _isVertical ? IconTopState : IconLeftState,
            (false, true) => IconOnlyState,
            (true, false) => ContentOnlyState,
            (true, true) => ContentOnlyState, // Invalid state. Treat as content only
        };

        // Update visual states
        VisualStateManager.GoToState(this, contentState, true);
        VisualStateManager.GoToState(this, _isVertical ? VerticalState : HorizontalState, true);
    }
}
