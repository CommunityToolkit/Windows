// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using TreeView = Microsoft.UI.Xaml.Controls.TreeView;

namespace CommunityToolkit.WinUI.Controls;

/// <summary>
/// Represents a <see cref="TreeView"/> and has a header.
/// </summary>
public partial class HeaderedTreeView : TreeView
{
    /// <summary>
    /// Initializes a new instance of the <see cref="HeaderedTreeView"/> class.
    /// </summary>
    public HeaderedTreeView()
    {
        DefaultStyleKey = typeof(HeaderedTreeView);

        // Workaround for https://github.com/microsoft/microsoft-ui-xaml/issues/3502
        this.DefaultStyleResourceUri = new Uri("ms-appx:///CommunityToolkit.WinUI.Controls.HeaderedControls/Themes/Generic.xaml");
    }

    /// <summary>
    /// Identifies the <see cref="Footer"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty FooterProperty = DependencyProperty.Register(
        nameof(Footer),
        typeof(object),
        typeof(HeaderedTreeView),
        new PropertyMetadata(null, OnFooterChanged));

    /// <summary>
    /// Identifies the <see cref="FooterTemplate"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty FooterTemplateProperty = DependencyProperty.Register(
        nameof(FooterTemplate),
        typeof(DataTemplate),
        typeof(HeaderedTreeView),
        new PropertyMetadata(null));

    /// <summary>
    /// Gets or sets the data used for the Footer of each control.
    /// </summary>
    public object Footer
    {
        get { return GetValue(FooterProperty); }
        set { SetValue(FooterProperty, value); }
    }

    /// <summary>
    /// Gets or sets the template used to display the content of the control's Footer.
    /// </summary>
    public DataTemplate FooterTemplate
    {
        get { return (DataTemplate)GetValue(FooterTemplateProperty); }
        set { SetValue(FooterTemplateProperty, value); }
    }

    /// <summary>
    /// Identifies the <see cref="Header"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
        nameof(Header),
        typeof(object),
        typeof(HeaderedTreeView),
        new PropertyMetadata(null, OnHeaderChanged));

    /// <summary>
    /// Identifies the <see cref="HeaderTemplate"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register(
        nameof(HeaderTemplate),
        typeof(DataTemplate),
        typeof(HeaderedTreeView),
        new PropertyMetadata(null));

    /// <summary>
    /// Gets or sets the data used for the header of each control.
    /// </summary>
    public object Header
    {
        get { return GetValue(HeaderProperty); }
        set { SetValue(HeaderProperty, value); }
    }

    /// <summary>
    /// Gets or sets the template used to display the content of the control's header.
    /// </summary>
    public DataTemplate HeaderTemplate
    {
        get { return (DataTemplate)GetValue(HeaderTemplateProperty); }
        set { SetValue(HeaderTemplateProperty, value); }
    }

    /// <summary>
    /// Called when the <see cref="Footer"/> property changes.
    /// </summary>
    /// <param name="oldValue">The old value of the <see cref="Footer"/> property.</param>
    /// <param name="newValue">The new value of the <see cref="Footer"/> property.</param>
    protected virtual void OnFooterChanged(object oldValue, object newValue)
    {
    }

    private static void OnFooterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var control = (HeaderedTreeView)d;
        control.OnFooterChanged(e.OldValue, e.NewValue);
    }

    /// <summary>
    /// Called when the <see cref="Header"/> property changes.
    /// </summary>
    /// <param name="oldValue">The old value of the <see cref="Header"/> property.</param>
    /// <param name="newValue">The new value of the <see cref="Header"/> property.</param>
    protected virtual void OnHeaderChanged(object oldValue, object newValue)
    {
    }

    private static void OnHeaderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var control = (HeaderedTreeView)d;
        control.OnHeaderChanged(e.OldValue, e.NewValue);
    }
}