// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#if WINUI3
using Microsoft.UI.Composition;
using Microsoft.UI.Xaml.Hosting;
using ListViewBase = Microsoft.UI.Xaml.Controls.ListViewBase;
#else
using Windows.UI.Composition;
using Windows.UI.Xaml.Hosting;
using ListViewBase = Windows.UI.Xaml.Controls.ListViewBase;
#endif

namespace CommunityToolkit.WinUI.Behaviors.Internal;

/// <summary>
/// Base class helper for header behaviors which manipulate an element within a viewport of a <see cref="ListViewBase"/> based control.
/// </summary>
public abstract class HeaderBehaviorBase : BehaviorBase<FrameworkElement>
{
    // From Doc: https://learn.microsoft.com/windows/windows-app-sdk/api/winrt/microsoft.ui.xaml.controls.canvas.zindex
    private const int CanvasZIndexMax = 1_000_000;

    /// <summary>
    /// The ScrollViewer associated with the ListViewBase control.
    /// </summary>
    protected ScrollViewer? _scrollViewer;

    /// <summary>
    /// The CompositionPropertySet associated with the ScrollViewer.
    /// </summary>
    protected CompositionPropertySet? _scrollProperties;

    /// <summary>
    /// The CompositionPropertySet associated with the animation.
    /// </summary>
    protected CompositionPropertySet? _animationProperties;

    /// <summary>
    /// The Visual associated with the header element.
    /// </summary>
    protected Visual? _headerVisual;

    /// <summary>
    /// Attaches the behavior to the associated object.
    /// </summary>
    /// <returns>
    ///   <c>true</c> if attaching succeeded; otherwise <c>false</c>.
    /// </returns>
    protected override bool Initialize()
    {
        var result = AssignAnimation();
        return result;
    }

    /// <summary>
    /// Detaches the behavior from the associated object.
    /// </summary>
    /// <returns>
    ///   <c>true</c> if detaching succeeded; otherwise <c>false</c>.
    /// </returns>
    protected override bool Uninitialize()
    {
        RemoveAnimation();
        return true;
    }

    /// <summary>
    /// Uses Composition API to get the UIElement and sets an ExpressionAnimation.
    /// </summary>
    /// <remarks>
    /// If this method returns true, you should have access to all protected fields with assigned components to use.
    /// </remarks>
    /// <returns><c>true</c> if the assignment was successful; otherwise, <c>false</c>.</returns>
    protected virtual bool AssignAnimation()
    {
        StopAnimation();

        // Double-check that we have an element associated with us (we should) and that it has size
        if (AssociatedObject == null || AssociatedObject.RenderSize.Height == 0)
        {
            return false;
        }

        if (_scrollViewer == null)
        {
            // TODO: We probably want checks which provide better guidance if we detect we're not attached correctly?
            _scrollViewer = AssociatedObject.FindAscendant<ScrollViewer>();
        }

        if (_scrollViewer == null)
        {
            return false;
        }

        var itemsControl = AssociatedObject.FindAscendant<ItemsControl>();

        if (itemsControl != null && itemsControl.ItemsPanelRoot != null)
        {
            // This appears to be important to force the items within the ScrollViewer of an ItemsControl behind our header element.
            Canvas.SetZIndex(itemsControl.ItemsPanelRoot, -1);
        }
        else
        {
            // If we're not part of a collection panel, then we're probably just in the ScrollViewer,
            // And we should ensure our 'header' element is on top of any other content within the ScrollViewer.
            Canvas.SetZIndex(AssociatedObject, CanvasZIndexMax);
        }

        if (_scrollProperties == null)
        {
            _scrollProperties = ElementCompositionPreview.GetScrollViewerManipulationPropertySet(_scrollViewer);
        }

        if (_scrollProperties == null)
        {
            return false;
        }

        if (_headerVisual == null)
        {
            _headerVisual = ElementCompositionPreview.GetElementVisual(AssociatedObject);
        }

        if (_headerVisual == null)
        {
            return false;
        }

        // TODO: Not sure if we need to provide an option to turn these events off, as FadeHeaderBehavior didn't use these two, unlike QuickReturn/Sticky did...
        AssociatedObject.SizeChanged -= ScrollHeader_SizeChanged;
        AssociatedObject.SizeChanged += ScrollHeader_SizeChanged;

        _scrollViewer.GotFocus -= ScrollViewer_GotFocus;
        _scrollViewer.GotFocus += ScrollViewer_GotFocus;

        var compositor = _scrollProperties.Compositor;

        if (_animationProperties == null)
        {
            _animationProperties = compositor.CreatePropertySet();
        }

        return true;
    }

    /// <summary>
    /// Stop the animation of the UIElement.
    /// </summary>
    protected abstract void StopAnimation();

    /// <summary>
    /// Remove the animation from the UIElement.
    /// </summary>
    protected virtual void RemoveAnimation()
    {
        if (_scrollViewer != null)
        {
            _scrollViewer.GotFocus -= ScrollViewer_GotFocus;
        }

        if (AssociatedObject != null)
        {
            AssociatedObject.SizeChanged -= ScrollHeader_SizeChanged;
        }

        StopAnimation();
    }

    private void ScrollHeader_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        AssignAnimation();
    }

    private void ScrollViewer_GotFocus(object sender, RoutedEventArgs e)
    {
        var scroller = (ScrollViewer)sender;

        object focusedElement;
        if (ApiInformationHelper.IsXamlRootAvailable && scroller.XamlRoot != null)
        {
            focusedElement = FocusManager.GetFocusedElement(scroller.XamlRoot)!;
        }
        else
        {
            focusedElement = FocusManager.GetFocusedElement()!;
        }

        // To prevent Popups (Flyouts...) from triggering the autoscroll, we check if the focused element has a valid parent.
        // Popups have no parents, whereas a normal Item would have the ListView as a parent.
        if (focusedElement is UIElement element && VisualTreeHelper.GetParent(element) != null)
        {
            var point = element.TransformToVisual(scroller).TransformPoint(new Point(0, 0));

            if (point.Y < AssociatedObject.ActualHeight)
            {
                scroller.ChangeView(0, scroller.VerticalOffset - (AssociatedObject.ActualHeight - point.Y), 1, false);
            }
        }
    }
}
