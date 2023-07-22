// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace CommunityToolkit.WinUI.Controls;

public partial class SegmentedItem : ListViewItem
{
    /// <summary>
    /// The backing <see cref="DependencyProperty"/> for the <see cref="Icon"/> property.
    /// </summary>
    public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
        nameof(Icon),
        typeof(IconElement),
        typeof(SegmentedItem),
        new PropertyMetadata(defaultValue: null, (d, e) => ((SegmentedItem)d).OnIconPropertyChanged((IconElement)e.OldValue, (IconElement)e.NewValue)));

    /// <summary>
    /// Gets or sets the icon.
    /// </summary>
    public IconElement Icon
    {
        get => (IconElement)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }
}
