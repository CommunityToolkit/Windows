// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace CommunityToolkit.WinUI.Controls;

/// <summary>
/// An example templated control.
/// </summary>
public partial class Triggers_xBind: Control
{
    /// <summary>
    /// Creates a new instance of the <see cref="Triggers_xBind"/> class.
    /// </summary>
    public Triggers_xBind()
    {
        this.DefaultStyleKey = typeof(Triggers_xBind);

        // Allows directly using this control as the x:DataType in the template.
        this.DataContext = this;
    }

    /// <summary>
    /// The backing <see cref="DependencyProperty"/> for the <see cref="ItemPadding"/> property.
    /// </summary>
    public static readonly DependencyProperty ItemPaddingProperty = DependencyProperty.Register(
        nameof(ItemPadding),
        typeof(Thickness),
        typeof(Triggers_xBind),
        new PropertyMetadata(defaultValue: new Thickness(0)));

    /// <summary>
    /// The backing <see cref="DependencyProperty"/> for the <see cref="MyProperty"/> property.
    /// </summary>
    public static readonly DependencyProperty MyPropertyProperty = DependencyProperty.Register(
        nameof(MyProperty),
        typeof(string),
        typeof(Triggers_xBind),
        new PropertyMetadata(defaultValue: string.Empty, (d, e) => ((Triggers_xBind)d).OnMyPropertyChanged((string)e.OldValue, (string)e.NewValue)));

    /// <summary>
    /// Gets or sets an example string. A basic DependencyProperty example.
    /// </summary>
    public string MyProperty
    {
        get => (string)GetValue(MyPropertyProperty);
        set => SetValue(MyPropertyProperty, value);
    }

    /// <summary>
    /// Gets or sets a padding for an item. A basic DependencyProperty example.
    /// </summary>
    public Thickness ItemPadding
    {
        get => (Thickness)GetValue(ItemPaddingProperty);
        set => SetValue(ItemPaddingProperty, value);
    }

    protected virtual void OnMyPropertyChanged(string oldValue, string newValue)
    {
        // Do something with the changed value.
    }

    public void Element_PointerEntered(object sender, PointerRoutedEventArgs e)
    {
        if (sender is TextBlock text)
        {
            text.Opacity = 1;
        }
    }
}
