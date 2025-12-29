// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.


namespace CommunityToolkit.WinUI;

/// <summary>
/// Provides attached dependency properties for the <see cref="ListViewBase"/>
/// </summary>
public static partial class ListViewExtensions
{
    private static readonly Dictionary<IObservableVector<object>, ListViewBase> _trackedListViews = [];

    /// <summary>
    /// Attached <see cref="DependencyProperty"/> for binding a <see cref="Brush"/> as an alternate background color to a <see cref="ListViewBase"/>
    /// </summary>
    public static readonly DependencyProperty AlternateColorProperty =
        DependencyProperty.RegisterAttached("AlternateColor", typeof(Brush), typeof(ListViewExtensions),
            new PropertyMetadata(null, OnAlternateRowPropertyChanged));

    /// <summary>
    /// Attached <see cref="DependencyProperty"/> for binding a <see cref="Style"/> as an alternate style to a <see cref="ListViewBase"/>
    /// </summary>
    public static readonly DependencyProperty AlternateStyleProperty =
        DependencyProperty.RegisterAttached("AlternateStyle", typeof(Style), typeof(ListViewExtensions),
            new PropertyMetadata(null, OnAlternateRowPropertyChanged));

    /// <summary>
    /// Attached <see cref="DependencyProperty"/> for binding a <see cref="DataTemplate"/> as an alternate template to a <see cref="ListViewBase"/>
    /// </summary>
    public static readonly DependencyProperty AlternateItemTemplateProperty =
        DependencyProperty.RegisterAttached("AlternateItemTemplate", typeof(DataTemplate), typeof(ListViewExtensions),
            new PropertyMetadata(null, OnAlternateRowPropertyChanged));

    /// <summary>
    /// Gets the alternate <see cref="Brush"/> associated with the specified <see cref="ListViewBase"/>
    /// </summary>
    /// <param name="obj">The <see cref="ListViewBase"/> to get the associated <see cref="Brush"/> from</param>
    /// <returns>The <see cref="Brush"/> associated with the <see cref="ListViewBase"/></returns>
    public static Brush? GetAlternateColor(ListViewBase obj) => (Brush?)obj.GetValue(AlternateColorProperty);

    /// <summary>
    /// Sets the alternate <see cref="Brush"/> associated with the specified <see cref="DependencyObject"/>
    /// </summary>
    /// <param name="obj">The <see cref="ListViewBase"/> to associate the <see cref="Brush"/> with</param>
    /// <param name="value">The <see cref="Brush"/> for binding to the <see cref="ListViewBase"/></param>
    public static void SetAlternateColor(ListViewBase obj, Brush? value) => obj.SetValue(AlternateColorProperty, value);

    /// <summary>
    /// Gets the alternate <see cref="Style"/> associated with the specified <see cref="ListViewBase"/>
    /// </summary>
    /// <param name="obj">The <see cref="ListViewBase"/> to get the associated <see cref="Style"/> from</param>
    /// <returns>The <see cref="Style"/> associated with the <see cref="ListViewBase"/></returns>
    public static Style? GetAlternateStyle(ListViewBase obj) => (Style?)obj.GetValue(AlternateStyleProperty);

    /// <summary>
    /// Sets the alternate <see cref="Style"/> associated with the specified <see cref="DependencyObject"/>
    /// </summary>
    /// <param name="obj">The <see cref="ListViewBase"/> to associate the <see cref="Style"/> with</param>
    /// <param name="value">The <see cref="Style"/> for binding to the <see cref="ListViewBase"/></param>
    public static void SetAlternateStyle(ListViewBase obj, Style? value) => obj.SetValue(AlternateStyleProperty, value);

    /// <summary>
    /// Gets the <see cref="DataTemplate"/> associated with the specified <see cref="ListViewBase"/>
    /// </summary>
    /// <param name="obj">The <see cref="ListViewBase"/> to get the associated <see cref="DataTemplate"/> from</param>
    /// <returns>The <see cref="DataTemplate"/> associated with the <see cref="ListViewBase"/></returns>
    public static DataTemplate? GetAlternateItemTemplate(ListViewBase obj) => (DataTemplate?)obj.GetValue(AlternateItemTemplateProperty);

    /// <summary>
    /// Sets the <see cref="DataTemplate"/> associated with the specified <see cref="ListViewBase"/>
    /// </summary>
    /// <param name="obj">The <see cref="ListViewBase"/> to associate the <see cref="DataTemplate"/> with</param>
    /// <param name="value">The <see cref="DataTemplate"/> for binding to the <see cref="ListViewBase"/></param>
    public static void SetAlternateItemTemplate(ListViewBase obj, DataTemplate? value) => obj.SetValue(AlternateItemTemplateProperty, value);

    private static void OnAlternateRowPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
    {
        if (sender is not ListViewBase listViewBase)
            return;

        // Cleanup existing subscriptions
        listViewBase.ContainerContentChanging -= OnContainerContentChanging;
        listViewBase.Items.VectorChanged -= OnItemsVectorChanged;
        listViewBase.Unloaded -= OnListViewBaseUnloaded_AltRow;

        _trackedListViews[listViewBase.Items] = listViewBase;

        // Resubscribe to events as necessary
        var altColor = GetAlternateColor(listViewBase);
        var altStyle = GetAlternateStyle(listViewBase);
        var altTemplate = GetAlternateItemTemplate(listViewBase);

        // If any of the properties are set, subscribe to the necessary events
        if ((altColor ?? altStyle ?? (object?)altTemplate) is not null)
        {
            listViewBase.ContainerContentChanging += OnContainerContentChanging;
            listViewBase.Items.VectorChanged += OnItemsVectorChanged;
            listViewBase.Unloaded += OnListViewBaseUnloaded_AltRow;
        }
    }

    private static void OnContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args) => UpdateItem(sender, args.ItemIndex);

    private static void OnItemsVectorChanged(IObservableVector<object> sender, IVectorChangedEventArgs args)
    {
        // If the index is at the end, no other items were affected
        // and there's no action to take
        if (args.Index == (sender.Count - 1))
            return;

        // This function is for updating indirectly affected items
        // Therefore we only need to handle items inserted and removed where every
        // item beneath would potentially change if they are even or odd.
        if (args.CollectionChange is not (CollectionChange.ItemInserted or CollectionChange.ItemRemoved))
            return;

        // Attempt to get the list view for the affected items
        _trackedListViews.TryGetValue(sender, out ListViewBase? listViewBase);
        if (listViewBase is null)
            return;

        int index = (int)args.Index;
        for (int i = index; i < sender.Count; i++)
            UpdateItem(listViewBase, i);
    }

    private static void UpdateItem(ListViewBase listViewBase, int itemIndex)
    {
        // Get the item as a control
        var control = listViewBase.ContainerFromIndex(itemIndex) as Control;
        control ??= listViewBase.Items[itemIndex] as Control;

        // If the item is not a control, there's nothing to be done
        if (control is null)
            return;

        // Get the item as a container. This may be null if the item is not in a container.
        // Also get all the alternate properties
        var container = control as SelectorItem;
        var altColor = GetAlternateColor(listViewBase);
        var altStyle = GetAlternateStyle(listViewBase);
        var altTemplate = GetAlternateItemTemplate(listViewBase);

        // Apply the alternate properties as necessary
        if (altStyle is not null)
        {
            control.Style = itemIndex % 2 == 0 ? altStyle : listViewBase.ItemContainerStyle;
        }

        if (altColor is not null)
        {
            SetRowBackground(listViewBase, control, itemIndex);
        }

        if (altTemplate is not null && container is not null)
        {
            container.ContentTemplate = itemIndex % 2 == 0 ? altTemplate : listViewBase.ItemTemplate;
        }
    }

    private static void SetRowBackground(ListViewBase sender, Control itemContainer, int itemIndex)
    {
        var brush = itemIndex % 2 == 0 ? GetAlternateColor(sender) : null;
        var rootBorder = itemContainer.FindDescendant<Border>();

        itemContainer.Background = brush;
        if (rootBorder is not null)
        {
            rootBorder.Background = brush;
        }
    }

    private static void OnListViewBaseUnloaded_AltRow(object sender, RoutedEventArgs e)
    {
        if (sender is not ListViewBase listViewBase)
            return;

        // Untrack the list view
        _trackedListViews.Remove(listViewBase.Items);

        // Unsubscribe from events
        listViewBase.ContainerContentChanging -= OnContainerContentChanging;
        listViewBase.Items.VectorChanged -= OnItemsVectorChanged;
        listViewBase.Unloaded -= OnListViewBaseUnloaded_AltRow;
    }
}
