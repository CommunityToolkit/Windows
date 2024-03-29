// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
#if WINAPPSDK
using Microsoft.UI.Xaml.Hosting;
#else
using Windows.UI.Xaml.Hosting;
#endif

namespace CommunityToolkit.WinUI.Animations;

/// <summary>
/// Attached properties to support implicitly triggered animations for <see cref="UIElement"/> instances.
/// </summary>
public static class Implicit
{
    /// <summary>
    /// The attached "ShowAnimations" property.
    /// </summary>
    public static readonly DependencyProperty ShowAnimationsProperty = DependencyProperty.RegisterAttached(
        "ShowAnimations",
        typeof(ImplicitAnimationSet),
        typeof(Implicit),
        new PropertyMetadata(null, OnShowAnimationsPropertyChanged));

    /// <summary>
    /// The attached "HideAnimations" property.
    /// </summary>
    public static readonly DependencyProperty HideAnimationsProperty = DependencyProperty.RegisterAttached(
        "HideAnimations",
        typeof(ImplicitAnimationSet),
        typeof(Implicit),
        new PropertyMetadata(null, OnHideAnimationsPropertyChanged));

    /// <summary>
    /// The attached "Animations" property.
    /// </summary>
    public static readonly DependencyProperty AnimationsProperty = DependencyProperty.RegisterAttached(
        "Animations",
        typeof(ImplicitAnimationSet),
        typeof(Implicit),
        new PropertyMetadata(null, OnAnimationsPropertyChanged));

    /// <summary>
    /// Gets the value of the <see cref="ShowAnimationsProperty"/> property.
    /// </summary>
    /// <param name="element">The <see cref="UIElement"/> to get the value for.</param>
    /// <returns>The retrieved <see cref="ImplicitAnimationSet"/> value.</returns>
    public static ImplicitAnimationSet GetShowAnimations(UIElement element)
    {
        var collection = (ImplicitAnimationSet)element.GetValue(ShowAnimationsProperty);

        if (collection is null)
        {
            element.SetValue(ShowAnimationsProperty, collection = new ImplicitAnimationSet());
        }

        return collection;
    }

    /// <summary>
    /// Sets the value of the <see cref="ShowAnimationsProperty"/> property.
    /// </summary>
    /// <param name="element">The <see cref="UIElement"/> to set the value for.</param>
    /// <param name="value">The <see cref="ImplicitAnimationSet"/> value to set.</param>
    public static void SetShowAnimations(UIElement element, ImplicitAnimationSet value)
    {
        element.SetValue(ShowAnimationsProperty, value);
    }

    /// <summary>
    /// Gets the value of the <see cref="HideAnimationsProperty"/> property.
    /// </summary>
    /// <param name="element">The <see cref="UIElement"/> to get the value for.</param>
    /// <returns>The retrieved <see cref="ImplicitAnimationSet"/> value.</returns>
    public static ImplicitAnimationSet GetHideAnimations(UIElement element)
    {
        var collection = (ImplicitAnimationSet)element.GetValue(HideAnimationsProperty);

        if (collection is null)
        {
            element.SetValue(HideAnimationsProperty, collection = new ImplicitAnimationSet());
        }

        return collection;
    }

    /// <summary>
    /// Sets the value of the <see cref="HideAnimationsProperty"/> property.
    /// </summary>
    /// <param name="element">The <see cref="UIElement"/> to set the value for.</param>
    /// <param name="value">The <see cref="ImplicitAnimationSet"/> value to set.</param>
    public static void SetHideAnimations(UIElement element, ImplicitAnimationSet value)
    {
        element.SetValue(HideAnimationsProperty, value);
    }

    /// <summary>
    /// Gets the value of the <see cref="AnimationsProperty"/> property.
    /// </summary>
    /// <param name="element">The <see cref="UIElement"/> to get the value for.</param>
    /// <returns>The retrieved <see cref="ImplicitAnimationSet"/> value.</returns>
    public static ImplicitAnimationSet GetAnimations(UIElement element)
    {
        var collection = (ImplicitAnimationSet)element.GetValue(AnimationsProperty);

        if (collection is null)
        {
            element.SetValue(AnimationsProperty, collection = new ImplicitAnimationSet());
        }

        return collection;
    }

    /// <summary>
    /// Sets the value of the <see cref="AnimationsProperty"/> property.
    /// </summary>
    /// <param name="element">The <see cref="UIElement"/> to set the value for.</param>
    /// <param name="value">The <see cref="AnimationSet"/> value to set.</param>
    public static void SetAnimations(UIElement element, ImplicitAnimationSet value)
    {
        element.SetValue(AnimationsProperty, value);
    }

    /// <summary>
    /// Callback to keep the attached parent in sync for animations linked to the <see cref="ShowAnimationsProperty"/> property.
    /// </summary>
    /// <param name="d">The target object the property was changed for.</param>
    /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance for the current event.</param>
    private static void OnShowAnimationsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        // See https://github.com/CommunityToolkit/Windows/issues/319
        #if HAS_UNO
        #pragma warning disable CS0162
            return;
        #endif

        static void OnAnimationsChanged(object sender, EventArgs e)
        {
            var collection = (ImplicitAnimationSet)sender;

            if (collection.ParentReference!.TryGetTarget(out UIElement? element))
            {
                ElementCompositionPreview.SetImplicitShowAnimation(element, collection.GetCompositionAnimationGroup(element));
            }
        }

        if (e.OldValue is ImplicitAnimationSet oldCollection)
        {
            oldCollection.AnimationsChanged -= OnAnimationsChanged!;
        }

        if (d is UIElement element)
        {
            if (e.NewValue is ImplicitAnimationSet collection)
            {
                collection.ParentReference = new(element);
                collection.AnimationsChanged -= OnAnimationsChanged!;
                collection.AnimationsChanged += OnAnimationsChanged!;

                ElementCompositionPreview.SetIsTranslationEnabled(element, true);
                ElementCompositionPreview.SetImplicitShowAnimation(element, collection.GetCompositionAnimationGroup(element));
            }
            else
            {
                ElementCompositionPreview.SetImplicitShowAnimation(element, null);
            }
        }

        #if HAS_UNO
        #pragma warning restore CS0162
        #endif
    }

    /// <summary>
    /// Callback to keep the attached parent in sync for animations linked to the <see cref="HideAnimationsProperty"/> property.
    /// </summary>
    /// <param name="d">The target object the property was changed for.</param>
    /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance for the current event.</param>
    private static void OnHideAnimationsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        // See https://github.com/CommunityToolkit/Windows/issues/319
        #if HAS_UNO
        #pragma warning disable CS0162
            return;
        #endif

        static void OnAnimationsChanged(object sender, EventArgs e)
        {
            var collection = (ImplicitAnimationSet)sender;

            if (collection.ParentReference!.TryGetTarget(out UIElement? element))
            {
                ElementCompositionPreview.SetImplicitHideAnimation(element, collection.GetCompositionAnimationGroup(element));
            }
        }

        if (e.OldValue is ImplicitAnimationSet oldCollection)
        {
            oldCollection.AnimationsChanged -= OnAnimationsChanged!;
        }

        if (d is UIElement element)
        {
            if (e.NewValue is ImplicitAnimationSet collection)
            {
                collection.ParentReference = new(element);
                collection.AnimationsChanged -= OnAnimationsChanged!;
                collection.AnimationsChanged += OnAnimationsChanged!;

                ElementCompositionPreview.SetIsTranslationEnabled(element, true);
                ElementCompositionPreview.SetImplicitHideAnimation(element, collection.GetCompositionAnimationGroup(element));
            }
            else
            {
                ElementCompositionPreview.SetImplicitHideAnimation(element, null);
            }
        }

        #if HAS_UNO
        #pragma warning restore CS0162
        #endif
    }

    /// <summary>
    /// Callback to keep the attached parent in sync for animations linked to the <see cref="AnimationsProperty"/> property.
    /// </summary>
    /// <param name="d">The target object the property was changed for.</param>
    /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance for the current event.</param>
    private static void OnAnimationsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        // See https://github.com/CommunityToolkit/Windows/issues/319
        #if HAS_UNO
        #pragma warning disable CS0162
            return;
        #endif

        static void OnAnimationsChanged(object sender, EventArgs e)
        {
            var collection = (ImplicitAnimationSet)sender;

            if (collection.ParentReference!.TryGetTarget(out UIElement? element))
            {
                ElementCompositionPreview.GetElementVisual(element).ImplicitAnimations = collection.GetImplicitAnimationCollection(element);
            }
        }

        if (e.OldValue is ImplicitAnimationSet oldCollection)
        {
            oldCollection.AnimationsChanged -= OnAnimationsChanged!;
        }

        if (d is UIElement element)
        {
            if (e.NewValue is ImplicitAnimationSet collection)
            {
                collection.ParentReference = new(element);
                collection.AnimationsChanged -= OnAnimationsChanged!;
                collection.AnimationsChanged += OnAnimationsChanged!;

                ElementCompositionPreview.SetIsTranslationEnabled(element, true);
                ElementCompositionPreview.GetElementVisual(element).ImplicitAnimations = collection.GetImplicitAnimationCollection(element);
            }
            else
            {
                ElementCompositionPreview.GetElementVisual(element).ImplicitAnimations = null;
            }
        }

        #if HAS_UNO
        #pragma warning restore CS0162
        #endif
    }
}
