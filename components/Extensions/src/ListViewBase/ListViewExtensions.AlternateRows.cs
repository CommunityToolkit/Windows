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

            _trackedListViews[listViewBase.Items] = listViewBase;
        }
        else
        {
            _trackedListViews.Remove(listViewBase.Items);
        }

            // Update all items to apply the new property
            UpdateItems(listViewBase);
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

        // Update all items from the affected index and below
        UpdateItems(listViewBase, (int)args.Index);
    }

    private static void UpdateItems(ListViewBase listViewBase, int startingIndex = 0)
    {
        for (int i = startingIndex; i < listViewBase.Items.Count; i++)
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
        var container = control as SelectorItem;

        // Get the base properties
        // The base color cannot be retrieved, and therefore cannot be unapplied.
        // NOTE: This is a huge design limitation, and one reason I believe
        // AlternateColor should be replaced entirely with AlternateStyle.
        var baseStyle = listViewBase.ItemContainerStyle;
        var baseTemplate = listViewBase.ItemTemplate;

        // Get all the alternate properties. 
        var altColor = GetAlternateColor(listViewBase);
        var altStyle = GetAlternateStyle(listViewBase);
        var altTemplate = GetAlternateItemTemplate(listViewBase);

        // Determine the realized properties based on the item index and
        // whether or not alternate properties are set.
        bool altRow = itemIndex % 2 == 0;
        var realizedColor = (altRow ? altColor : null) ?? null;
        var realizedStyle = (altRow ? altStyle : baseStyle) ?? baseStyle;
        var realizedTemplate = (altRow ? altTemplate : baseTemplate) ?? baseTemplate;

        // Apply the realized properties
        SetRowBackground(listViewBase, control, realizedColor);
        control.Style = realizedStyle;
        if (container is not null)
        {
            container.ContentTemplate = realizedTemplate;
        }
    }

    private static void SetRowBackground(ListViewBase sender, Control itemContainer, Brush? brush)
    {
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
