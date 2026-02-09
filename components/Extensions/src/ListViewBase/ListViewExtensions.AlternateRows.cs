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
            new PropertyMetadata(null, OnAlternateColorPropertyChanged));

    /// <summary>
    /// Attached <see cref="DependencyProperty"/> for binding a <see cref="DataTemplate"/> as an alternate template to a <see cref="ListViewBase"/>
    /// </summary>
    public static readonly DependencyProperty AlternateItemTemplateProperty =
        DependencyProperty.RegisterAttached("AlternateItemTemplate", typeof(DataTemplate), typeof(ListViewExtensions),
            new PropertyMetadata(null, OnAlternateItemTemplatePropertyChanged));

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

    private static void OnAlternateColorPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
    {
        if (sender is not ListViewBase listViewBase)
            return;

        // Cleanup existing subscriptions
        listViewBase.ContainerContentChanging -= ColorContainerContentChanging;
        listViewBase.Items.VectorChanged -= ColorItemsVectorChanged;
        listViewBase.Unloaded -= OnListViewBaseUnloaded_AltRow;

        // Resubscribe to events as necessary
        if (GetAlternateColor(listViewBase) is not null)
        {
            listViewBase.ContainerContentChanging += ColorContainerContentChanging;
            listViewBase.Items.VectorChanged += ColorItemsVectorChanged;
            listViewBase.Unloaded += OnListViewBaseUnloaded_AltRow;

            _trackedListViews[listViewBase.Items] = listViewBase;
        }
        else
        {
            _trackedListViews.Remove(listViewBase.Items);
        }
    }

    private static void ColorContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
    {
        // Get the row's item container, or contents as a fallback
        Control? control = args.ItemContainer ?? args.Item as Control;

        // Update the row background if the item was found
        if (control is not null)
        {
            SetRowBackground(sender, control, args.ItemIndex);
        }
    }

    private static void OnAlternateItemTemplatePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
    {
        if (sender is not ListViewBase listViewBase)
            return;

        // Cleanup existing subscriptions
        listViewBase.ContainerContentChanging -= ItemTemplateContainerContentChanging;
        listViewBase.Unloaded -= OnListViewBaseUnloaded_AltRow;

        // Resubscribe to events as necessary
        if (GetAlternateItemTemplate(listViewBase) is not null)
        {
            listViewBase.ContainerContentChanging += ItemTemplateContainerContentChanging;
            listViewBase.Unloaded += OnListViewBaseUnloaded_AltRow;
        }
    }

    private static void ItemTemplateContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
    {
        var template = args.ItemIndex % 2 == 0 ? GetAlternateItemTemplate(sender) : sender.ItemTemplate;
        args.ItemContainer.ContentTemplate = template;
    }

    private static void ColorItemsVectorChanged(IObservableVector<object> sender, IVectorChangedEventArgs args)
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
        {
            // Get item container or element at index
            var itemContainer = listViewBase.ContainerFromIndex(i) as Control;
            itemContainer ??= listViewBase.Items[i] as Control;

            if (itemContainer is not null)
            {
                SetRowBackground(listViewBase, itemContainer, i);
            }
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
        listViewBase.ContainerContentChanging -= ItemTemplateContainerContentChanging;
        listViewBase.ContainerContentChanging -= ColorContainerContentChanging;
        listViewBase.Items.VectorChanged -= ColorItemsVectorChanged;
        listViewBase.Unloaded -= OnListViewBaseUnloaded_AltRow;
    }
}
