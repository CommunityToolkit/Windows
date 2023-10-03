// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.UI.Xaml.Controls.Primitives;

namespace CommunityToolkit.WinUI.Controls;

[ContentProperty(Name = nameof(Content))]
public partial class SegmentedItem : ListViewItem
{
    internal const string IconLeftState = "IconLeft";
    internal const string IconOnlyState = "IconOnly";
    internal const string ContentOnlyState = "ContentOnly";
    private WeakReference<Segmented> parentSegmented;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public SegmentedItem()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
        this.DefaultStyleKey = typeof(SegmentedItem);
    }

    internal Segmented ParentSegmented
    {
        get
        {
            this.parentSegmented.TryGetTarget(out var segmented);
#pragma warning disable CS8603 // Possible null reference return.
            return segmented;
#pragma warning restore CS8603 // Possible null reference return.
        }
        
        set => this.parentSegmented = new WeakReference<Segmented>(value);
    }

    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        OnIconChanged();
        ContentChanged();
        RegisterAutomation();
        RegisterPropertyChangedCallback(SelectorItem.IsSelectedProperty, OnIsSelectedChanged);
    }

    private void RegisterAutomation()
    {
        //if (Content is string headerString && headerString != string.Empty)
        //{
        //    if (!string.IsNullOrEmpty(headerString) && string.IsNullOrEmpty(AutomationProperties.GetName(this)))
        //    {
        //        AutomationProperties.SetName(this, headerString);
        //    }
        //}
    }

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

    /// <summary>
    /// Creates AutomationPeer
    /// </summary>
    /// <returns>An automation peer for <see cref = "SegmentedItem" />.</ returns >
    protected override AutomationPeer OnCreateAutomationPeer()
    {
        return new SegmentedItemAutomationPeer(this);
    }
    internal event EventHandler Selected;
    private void OnIsSelectedChanged(DependencyObject sender, DependencyProperty dp)
    {
        var item = (SegmentedItem)sender;

        if (item.IsSelected)
        {
            Selected?.Invoke(this, EventArgs.Empty);

            //VisualStateManager.GoToState(item, SelectedState, true);
        }
        else
        {
           // VisualStateManager.GoToState(item, NormalState, true);
        }
    }
}
