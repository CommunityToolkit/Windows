// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace CommunityToolkit.WinUI.Controls;

/// <summary>
/// Represents the container for an item in a <see cref="Carousel"/> control
/// </summary>
public partial class CarouselItem : SelectorItem
{
    private const string PointerOverState = "PointerOver";
    private const string PointerOverSelectedState = "PointerOverSelected";
    private const string PressedState = "Pressed";
    private const string PressedSelectedState = "PressedSelected";
    private const string SelectedState = "Selected";
    private const string NormalState = "Normal";

    /// <summary>
    /// Fired when the <see cref="CarouselItem"/> is selected.
    /// </summary>
    public event EventHandler? Selected;

    /// <summary>
    /// Initializes a new instance of the <see cref="CarouselItem"/> class.
    /// </summary>
    public CarouselItem()
    {
        DefaultStyleKey = typeof(CarouselItem);

        RegisterPropertyChangedCallback(IsSelectedProperty, OnIsSelectedChanged);
    }

    /// <inheritdoc/>
    protected override void OnPointerEntered(PointerRoutedEventArgs e)
    {
        base.OnPointerEntered(e);

        VisualStateManager.GoToState(this, IsSelected ? PointerOverSelectedState : PointerOverState, true);
    }

    /// <inheritdoc/>
    protected override void OnPointerExited(PointerRoutedEventArgs e)
    {
        base.OnPointerExited(e);

        VisualStateManager.GoToState(this, IsSelected ? SelectedState : NormalState, true);
    }

    /// <inheritdoc/>
    protected override void OnPointerPressed(PointerRoutedEventArgs e)
    {
        base.OnPointerPressed(e);

        VisualStateManager.GoToState(this, IsSelected ? PressedSelectedState : PressedState, true);
    }

    private void OnIsSelectedChanged(DependencyObject sender, DependencyProperty dp)
    {
        var item = (CarouselItem)sender;

        if (item.IsSelected)
        {
            Selected?.Invoke(this, EventArgs.Empty);
            VisualStateManager.GoToState(item, SelectedState, true);
        }
        else
        {
            VisualStateManager.GoToState(item, NormalState, true);
        }
    }
}
