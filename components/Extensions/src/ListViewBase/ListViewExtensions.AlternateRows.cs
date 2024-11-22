// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.


namespace CommunityToolkit.WinUI;

/// <summary>
/// Provides attached dependency properties for the <see cref="ListViewBase"/>
/// </summary>
public static partial class ListViewExtensions
{
    private static Dictionary<IObservableVector<object>, ListViewBase> _itemsForList = new Dictionary<IObservableVector<object>, ListViewBase>();

    /// <summary>
    /// Attached <see cref="DependencyProperty"/> for binding a <see cref="Brush"/> as an alternate background color to a <see cref="ListViewBase"/>
    /// </summary>
    public static readonly DependencyProperty AlternateColorProperty = DependencyProperty.RegisterAttached("AlternateColor", typeof(Brush), typeof(ListViewExtensions), new PropertyMetadata(null, OnAlternateColorPropertyChanged));

    /// <summary>
    /// Attached <see cref="DependencyProperty"/> for binding a <see cref="DataTemplate"/> as an alternate template to a <see cref="ListViewBase"/>
    /// </summary>
    public static readonly DependencyProperty AlternateItemTemplateProperty = DependencyProperty.RegisterAttached("AlternateItemTemplate", typeof(DataTemplate), typeof(ListViewExtensions), new PropertyMetadata(null, OnAlternateItemTemplatePropertyChanged));

    /// <summary>
    /// Gets the alternate <see cref="Brush"/> associated with the specified <see cref="ListViewBase"/>
    /// </summary>
    /// <param name="obj">The <see cref="ListViewBase"/> to get the associated <see cref="Brush"/> from</param>
    /// <returns>The <see cref="Brush"/> associated with the <see cref="ListViewBase"/></returns>
    public static Brush GetAlternateColor(ListViewBase obj)
    {
        return (Brush)obj.GetValue(AlternateColorProperty);
    }

    /// <summary>
    /// Sets the alternate <see cref="Brush"/> associated with the specified <see cref="DependencyObject"/>
    /// </summary>
    /// <param name="obj">The <see cref="ListViewBase"/> to associate the <see cref="Brush"/> with</param>
    /// <param name="value">The <see cref="Brush"/> for binding to the <see cref="ListViewBase"/></param>
    public static void SetAlternateColor(ListViewBase obj, Brush value)
    {
        obj.SetValue(AlternateColorProperty, value);
    }

    /// <summary>
    /// Gets the <see cref="DataTemplate"/> associated with the specified <see cref="ListViewBase"/>
    /// </summary>
    /// <param name="obj">The <see cref="ListViewBase"/> to get the associated <see cref="DataTemplate"/> from</param>
    /// <returns>The <see cref="DataTemplate"/> associated with the <see cref="ListViewBase"/></returns>
    public static DataTemplate GetAlternateItemTemplate(ListViewBase obj)
    {
        return (DataTemplate)obj.GetValue(AlternateItemTemplateProperty);
    }

    /// <summary>
    /// Sets the <see cref="DataTemplate"/> associated with the specified <see cref="ListViewBase"/>
    /// </summary>
    /// <param name="obj">The <see cref="ListViewBase"/> to associate the <see cref="DataTemplate"/> with</param>
    /// <param name="value">The <see cref="DataTemplate"/> for binding to the <see cref="ListViewBase"/></param>
    public static void SetAlternateItemTemplate(ListViewBase obj, DataTemplate value)
    {
        obj.SetValue(AlternateItemTemplateProperty, value);
    }

    private static void OnAlternateColorPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
    {
        if (sender is ListViewBase listViewBase)
        {
            listViewBase.ContainerContentChanging -= ColorContainerContentChanging;
            listViewBase.Items.VectorChanged -= ColorItemsVectorChanged;
            listViewBase.Unloaded -= OnListViewBaseUnloaded;

            _itemsForList[listViewBase.Items] = listViewBase;
            if (AlternateColorProperty != null)
            {
                listViewBase.ContainerContentChanging += ColorContainerContentChanging;
                listViewBase.Items.VectorChanged += ColorItemsVectorChanged;
                listViewBase.Unloaded += OnListViewBaseUnloaded;
            }
        }
    }

    private static void ColorContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
    {
        var itemContainer = args.ItemContainer as Control;
        SetItemContainerBackground(sender, itemContainer, args.ItemIndex);
    }

    private static void OnAlternateItemTemplatePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
    {
        if (sender is ListViewBase listViewBase)
        {
            listViewBase.ContainerContentChanging -= ItemTemplateContainerContentChanging;
            listViewBase.Unloaded -= OnListViewBaseUnloaded;

            if (AlternateItemTemplateProperty != null)
            {
                listViewBase.ContainerContentChanging += ItemTemplateContainerContentChanging;
                listViewBase.Unloaded += OnListViewBaseUnloaded;
            }
        }
    }

    private static void ItemTemplateContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
    {
        if (args.ItemIndex % 2 == 0)
        {
            args.ItemContainer.ContentTemplate = GetAlternateItemTemplate(sender);
        }
        else
        {
            args.ItemContainer.ContentTemplate = sender.ItemTemplate;
        }
    }

    private static void OnItemContainerStretchDirectionPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
    {
        if (sender is ListViewBase listViewBase)
        {
            listViewBase.ContainerContentChanging -= ItemContainerStretchDirectionChanging;
            listViewBase.Unloaded -= OnListViewBaseUnloaded;

            if (ItemContainerStretchDirectionProperty != null)
            {
                listViewBase.ContainerContentChanging += ItemContainerStretchDirectionChanging;
                listViewBase.Unloaded += OnListViewBaseUnloaded;
            }
        }
    }

    private static void ItemContainerStretchDirectionChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
    {
        var stretchDirection = GetItemContainerStretchDirection(sender);

        if (stretchDirection == ItemContainerStretchDirection.Vertical || stretchDirection == ItemContainerStretchDirection.Both)
        {
            args.ItemContainer.VerticalContentAlignment = VerticalAlignment.Stretch;
        }

        if (stretchDirection == ItemContainerStretchDirection.Horizontal || stretchDirection == ItemContainerStretchDirection.Both)
        {
            args.ItemContainer.HorizontalContentAlignment = HorizontalAlignment.Stretch;
        }
    }

    private static void OnListViewBaseUnloaded(object sender, RoutedEventArgs e)
    {
        if (sender is ListViewBase listViewBase)
        {
            _itemsForList.Remove(listViewBase.Items);

            listViewBase.ContainerContentChanging -= ItemContainerStretchDirectionChanging;
            listViewBase.ContainerContentChanging -= ItemTemplateContainerContentChanging;
            listViewBase.ContainerContentChanging -= ColorContainerContentChanging;
            listViewBase.Items.VectorChanged -= ColorItemsVectorChanged;
            listViewBase.Unloaded -= OnListViewBaseUnloaded;
        }
    }

    private static void ColorItemsVectorChanged(IObservableVector<object> sender, IVectorChangedEventArgs args)
    {
        // If the index is at the end we can ignore
        if (args.Index == (sender.Count - 1))
        {
            return;
        }

        // Only need to handle Inserted and Removed because we'll handle everything else in the
        // ColorContainerContentChanging method
        if ((args.CollectionChange == CollectionChange.ItemInserted) || (args.CollectionChange == CollectionChange.ItemRemoved))
        {
            _itemsForList.TryGetValue(sender, out ListViewBase? listViewBase);
            if (listViewBase == null)
            {
                return;
            }

            int index = (int)args.Index;
            for (int i = index; i < sender.Count; i++)
            {
                var itemContainer = listViewBase.ContainerFromIndex(i) as Control;
                if (itemContainer != null)
                {
                    SetItemContainerBackground(listViewBase, itemContainer, i);
                }
            }
        }
    }

    private static void SetItemContainerBackground(ListViewBase sender, Control itemContainer, int itemIndex)
    {
        if (itemIndex % 2 == 0)
        {
            itemContainer.Background = GetAlternateColor(sender);
            var rootBorder = itemContainer.FindDescendant<Border>();
            if (rootBorder != null)
            {
                rootBorder.Background = GetAlternateColor(sender);
            }
        }
        else
        {
            itemContainer.Background = null;
            var rootBorder = itemContainer.FindDescendant<Border>();
            if (rootBorder != null)
            {
                rootBorder.Background = null;
            }
        }
    }
}
