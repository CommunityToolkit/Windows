// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Numerics;

#if WINUI3
using Microsoft.UI.Composition;
using Microsoft.UI.Xaml.Hosting;
#elif WINUI2
using Windows.UI.Composition;
using Windows.UI.Xaml.Hosting;
#endif

namespace CommunityToolkit.WinUI.Media;

/// <summary>
/// Attached properties to support attaching custom pipelines to UI elements.
/// </summary>
public static class UIElementExtensions
{
    /// <summary>
    /// Identifies the VisualFactory XAML attached property.
    /// </summary>
    public static readonly DependencyProperty VisualFactoryProperty = DependencyProperty.RegisterAttached(
        "VisualFactory",
        typeof(AttachedVisualFactoryBase),
        typeof(UIElementExtensions),
        new PropertyMetadata(null, OnVisualFactoryPropertyChanged));

    /// <summary>
    /// Gets the value of <see cref="VisualFactoryProperty"/>.
    /// </summary>
    /// <param name="element">The <see cref="UIElement"/> to get the value for.</param>
    /// <returns>The retrieved <see cref="AttachedVisualFactoryBase"/> item.</returns>
    public static AttachedVisualFactoryBase GetVisualFactory(UIElement element)
    {
        return (AttachedVisualFactoryBase)element.GetValue(VisualFactoryProperty);
    }

    /// <summary>
    /// Sets the value of <see cref="VisualFactoryProperty"/>.
    /// </summary>
    /// <param name="element">The <see cref="UIElement"/> to set the value for.</param>
    /// <param name="value">The <see cref="AttachedVisualFactoryBase"/> value to set.</param>
    public static void SetVisualFactory(UIElement element, AttachedVisualFactoryBase value)
    {
        element.SetValue(VisualFactoryProperty, value);
    }

    /// <summary>
    /// Callback to apply the visual for <see cref="VisualFactoryProperty"/>.
    /// </summary>
    /// <param name="d">The target object the property was changed for.</param>
    /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance for the current event.</param>
    private static async void OnVisualFactoryPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        UIElement element = (UIElement)d;
        Visual attachedVisual = await ((AttachedVisualFactoryBase)e.NewValue).GetAttachedVisualAsync(element);

        attachedVisual.RelativeSizeAdjustment = Vector2.One;

        ElementCompositionPreview.SetElementChildVisual(element, attachedVisual);
    }
}
