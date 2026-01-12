// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace CommunityToolkit.WinUI.Controls;

/// <summary>
/// An example templated control.
/// </summary>
public partial class Carousel : ItemsControl
{
    /// <summary>
    /// Occurs when the selected item changes.
    /// </summary>
    public event SelectionChangedEventHandler? SelectionChanged;

    /// <summary>
    /// Creates a new instance of the <see cref="Carousel"/> class.
    /// </summary>
    public Carousel()
    {
        this.DefaultStyleKey = typeof(Carousel);
    }

    /// <summary>
    /// Returns the container used for each item
    /// </summary>
    /// <returns>Returns always a CarouselItem</returns>
    protected override DependencyObject GetContainerForItemOverride()
        => new CarouselItem();
}
