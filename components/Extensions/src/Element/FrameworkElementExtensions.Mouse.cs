// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#if WINAPPSDK
using Microsoft.UI.Input;
using Cursor = Microsoft.UI.Input.InputCursor;
using CursorShape = Microsoft.UI.Input.InputSystemCursorShape;
#else
using Cursor = Windows.UI.Core.CoreCursor;
using CursorShape = Windows.UI.Core.CoreCursorType;
#endif

namespace CommunityToolkit.WinUI;

// TODO: Note: Windows App SDK doesn't support this (need to use Protected Cursor), but we still use this extension for Sizer controls.
// For now rather than not porting, we'll just exclude on Windows App SDK platforms. Fenced other blocks below and support both equivelent types, but don't have a general way to set cursor on window yet in PointerEntered/Exited. If in end, FrameworkElement gets non-protected Cursor property like WPF, then this extension also isn't needed.
// See https://github.com/microsoft/microsoft-ui-xaml/issues/4834
#if !WINAPPSDK

/// <inheritdoc cref="FrameworkElementExtensions"/>
public static partial class FrameworkElementExtensions
{
    private static readonly object _cursorLock = new object();
    private static readonly Cursor _defaultCursor =
#if WINAPPSDK
        InputSystemCursor.Create(CursorShape.Arrow);
#else
        new Cursor(CursorShape.Arrow, 1);
#endif
    private static readonly Dictionary<CursorShape, Cursor> _cursors =
        new() { { CursorShape.Arrow, _defaultCursor } };

    /// <summary>
    /// Dependency property for specifying the target <see cref="InputSystemCursorShape"/> to be shown
    /// over the target <see cref="FrameworkElement"/>.
    /// </summary>
    public static readonly DependencyProperty CursorProperty =
        DependencyProperty.RegisterAttached("Cursor", typeof(CursorShape), typeof(FrameworkElementExtensions), new PropertyMetadata(CursorShape.Arrow, CursorChanged));

    /// <summary>
    /// Set the target <see cref="InputSystemCursorShape"/>.
    /// </summary>
    /// <param name="element">Object where the selector cursor type should be shown.</param>
    /// <param name="value">Target cursor type value.</param>
    public static void SetCursor(FrameworkElement element, CursorShape value)
    {
        element.SetValue(CursorProperty, value);
    }

    /// <summary>
    /// Get the current <see cref="InputSystemCursorShape"/>.
    /// </summary>
    /// <param name="element">Object where the selector cursor type should be shown.</param>
    /// <returns>Cursor type set on target element.</returns>
    public static CursorShape GetCursor(FrameworkElement element)
    {
        return (CursorShape)element.GetValue(CursorProperty);
    }

    private static void CursorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var element = d as FrameworkElement;
        if (element == null)
        {
            throw new NullReferenceException(nameof(element));
        }

        var value = (CursorShape)e.NewValue;

        // lock ensures InputCursor creation and event handlers attachment/detachment is atomic
        lock (_cursorLock)
        {
            if (!_cursors.ContainsKey(value))
            {
                _cursors[value] =
#if WINAPPSDK
                    InputSystemCursor.Create(value);
#else
                    new Cursor(value, 1);
#endif
            }

            // make sure event handlers are not attached twice to element
            element.PointerEntered -= Element_PointerEntered;
            element.PointerEntered += Element_PointerEntered;
            element.PointerExited -= Element_PointerExited;
            element.PointerExited += Element_PointerExited;
            element.Unloaded -= ElementOnUnloaded;
            element.Unloaded += ElementOnUnloaded;
        }
    }

    private static void Element_PointerEntered(object sender, PointerRoutedEventArgs e)
    {
        if (Window.Current == null)
        {
            return;
        }

        CursorShape cursor = GetCursor((FrameworkElement)sender);

#if !WINAPPSDK
        Window.Current.CoreWindow.PointerCursor = _cursors[cursor];
#endif
    }

    private static void Element_PointerExited(object sender, PointerRoutedEventArgs e)
    {
        if (Window.Current == null)
        {
            return;
        }

        // when exiting change the cursor to the target Mouse.Cursor value of the new element
        Cursor cursor;
        if (sender != e.OriginalSource && e.OriginalSource is FrameworkElement newElement)
        {
            cursor = _cursors[GetCursor(newElement)];
        }
        else
        {
            cursor = _defaultCursor;
        }

#if !WINAPPSDK
        Window.Current.CoreWindow.PointerCursor = cursor;
#endif
    }

    private static void ElementOnUnloaded(object sender, RoutedEventArgs routedEventArgs)
    {
        if (Window.Current == null)
        {
            return;
        }

        // when the element is programatically unloaded, reset the cursor back to default
        // this is necessary when click triggers immediate change in layout and PointerExited is not called
#if !WINAPPSDK
        Window.Current.CoreWindow.PointerCursor = _defaultCursor;
#endif
    }
}

#endif
