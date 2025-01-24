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
    internal const string IconOnlyState = "IconOnly";
    internal const string ContentOnlyState = "ContentOnly";

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
        OnIconChanged();
        ContentChanged();
    }

    /// <summary>
    /// Handles changes to the Content property.
    /// </summary>
    protected override void OnContentChanged(object oldContent, object newContent)
    {
        base.OnContentChanged(oldContent, newContent);
        ContentChanged();
    }

    private void ContentChanged()
    {
        if (Content != null)
        {
            VisualStateManager.GoToState(this, IconLeftState, true);
        }
        else
        {
            VisualStateManager.GoToState(this, IconOnlyState, true);
        }
    }

    /// <summary>
    /// Handles changes to the Icon property.
    /// </summary>
    protected virtual void OnIconPropertyChanged(IconElement oldValue, IconElement newValue)
    {
        OnIconChanged();
    }

    private void OnIconChanged()
    {
        if (Icon != null)
        {
            VisualStateManager.GoToState(this, IconLeftState, true);
        }
        else
        {
            VisualStateManager.GoToState(this, ContentOnlyState, true);
        }
    }
}
