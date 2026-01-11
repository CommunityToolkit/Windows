// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace CommunityToolkit.WinUI.Controls;

/// <summary>
/// An example templated control.
/// </summary>
[TemplatePart(Name = nameof(PART_HelloWorld), Type = typeof(TextBlock))]
public partial class Carousel_ClassicBinding : Control
{
    /// <summary>
    /// Creates a new instance of the <see cref="Carousel_ClassicBinding"/> class.
    /// </summary>
    public Carousel_ClassicBinding()
    {
        this.DefaultStyleKey = typeof(Carousel_ClassicBinding);
    }

    /// <summary>
    /// The primary text block that displays "Hello world".
    /// </summary>
    protected TextBlock? PART_HelloWorld { get; private set; }

    /// <inheritdoc />
    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        // Detach all attached events when a new template is applied.
        if (PART_HelloWorld is not null)
        {
            PART_HelloWorld.PointerEntered -= Element_PointerEntered;
        }

        // Attach events when the template is applied and the control is loaded.
        PART_HelloWorld = GetTemplateChild(nameof(PART_HelloWorld)) as TextBlock;
        
        if (PART_HelloWorld is not null)
        {
            PART_HelloWorld.PointerEntered += Element_PointerEntered;
        }
    }

    /// <summary>
    /// The backing <see cref="DependencyProperty"/> for the <see cref="ItemPadding"/> property.
    /// </summary>
    public static readonly DependencyProperty ItemPaddingProperty = DependencyProperty.Register(
        nameof(ItemPadding),
        typeof(Thickness),
        typeof(Carousel_ClassicBinding),
        new PropertyMetadata(defaultValue: new Thickness(0)));

    /// <summary>
    /// The backing <see cref="DependencyProperty"/> for the <see cref="MyProperty"/> property.
    /// </summary>
    public static readonly DependencyProperty MyPropertyProperty = DependencyProperty.Register(
        nameof(MyProperty),
        typeof(string),
        typeof(Carousel_ClassicBinding),
        new PropertyMetadata(defaultValue: string.Empty, (d, e) => ((Carousel_ClassicBinding)d).OnMyPropertyChanged((string)e.OldValue, (string)e.NewValue)));

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
