// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Windows.Input;

namespace CommunityToolkit.WinUI;

/// <summary>
/// Provides the Command attached dependency property for the <see cref="ListViewBase"/>
/// </summary>
public static partial class ListViewExtensions
{
    /// <summary>
    /// Attached <see cref="DependencyProperty"/> for binding an <see cref="ICommand"/> to handle ListViewBase Item interaction by means of <see cref="ListViewBase"/> ItemClick event. ListViewBase IsItemClickEnabled must be set to true.
    /// </summary>
    public static readonly DependencyProperty CommandProperty =
        DependencyProperty.RegisterAttached("Command", typeof(ICommand), typeof(ListViewExtensions),
            new PropertyMetadata(null, OnCommandPropertyChanged));

    /// <summary>
    /// Gets the <see cref="ICommand"/> associated with the specified <see cref="ListViewBase"/>
    /// </summary>
    /// <param name="obj">The <see cref="ListViewBase"/> to get the associated <see cref="ICommand"/> from</param>
    /// <returns>The <see cref="ICommand"/> associated with the <see cref="ListViewBase"/></returns>
    public static ICommand GetCommand(ListViewBase obj) => (ICommand)obj.GetValue(CommandProperty);

    /// <summary>
    /// Sets the <see cref="ICommand"/> associated with the specified <see cref="ListViewBase"/>
    /// </summary>
    /// <param name="obj">The <see cref="ListViewBase"/> to associate the <see cref="ICommand"/> with</param>
    /// <param name="value">The <see cref="ICommand"/> for binding to the <see cref="ListViewBase"/></param>
    public static void SetCommand(ListViewBase obj, ICommand value) => obj.SetValue(CommandProperty, value);

    private static void OnCommandPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
    {
        if (sender is not ListViewBase listViewBase)
            return;

        var oldCommand = args.OldValue as ICommand;
        if (oldCommand is not null)
        {
            listViewBase.ItemClick -= OnListViewBaseItemClick;
        }

        var newCommand = args.NewValue as ICommand;
        if (newCommand is not null)
        {
            listViewBase.ItemClick += OnListViewBaseItemClick;
        }
    }

    private static void OnListViewBaseItemClick(object sender, ItemClickEventArgs e)
    {
        if (sender is not ListViewBase listViewBase)
            return;

        var command = GetCommand(listViewBase);
        if (command is null)
            return;

        if (command.CanExecute(e.ClickedItem))
            command.Execute(e.ClickedItem);
    }
}
