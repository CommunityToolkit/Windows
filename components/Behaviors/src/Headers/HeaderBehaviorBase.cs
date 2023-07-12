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
    protected ScrollViewer? _scrollViewer;
    protected CompositionPropertySet? _scrollProperties;
    protected CompositionPropertySet? _animationProperties;
    protected Visual? _headerVisual;

    /// <summary>
    /// Gets or sets the target element for the Header behavior to be manipulated within the viewport.
    /// </summary>
    /// <remarks>
    /// Set this using the header of a ListView or GridView.
    /// </remarks>
    public UIElement HeaderElement
    {
        get { return (UIElement)GetValue(HeaderElementProperty); }
        set { SetValue(HeaderElementProperty, value); }
    }

    /// <summary>
    /// Defines the Dependency Property for the <see cref="HeaderElement"/> property.
    /// </summary>
    public static readonly DependencyProperty HeaderElementProperty = DependencyProperty.Register(
        nameof(HeaderElement), typeof(UIElement), typeof(HeaderBehaviorBase), new PropertyMetadata(null, PropertyChangedCallback));

    /// <summary>
    /// If any of the properties are changed then the animation is automatically started.
    /// </summary>
    /// <param name="d">The dependency object.</param>
    /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
    private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is HeaderBehaviorBase @base)
        {
            @base.AssignAnimation();
        }
    }

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
    /// Uses Composition API to get the UIElement and sets an ExpressionAnimation
    /// </summary>
    /// <returns><c>true</c> if the assignment was successful; otherwise, <c>false</c>.</returns>
    protected virtual bool AssignAnimation()
    {
        StopAnimation();

        if (AssociatedObject == null)
        {
            return false;
        }

        // TODO: What if we attach to the 'header element' and look up for the ScrollViewer?
        if (_scrollViewer == null)
        {
            _scrollViewer = AssociatedObject as ScrollViewer ?? AssociatedObject.FindDescendant<ScrollViewer>();
        }

        if (_scrollViewer == null)
        {
            return false;
        }

        var listView = AssociatedObject as ListViewBase ?? AssociatedObject.FindDescendant<ListViewBase>();

        // TODO: Is this required?
        if (listView != null && listView.ItemsPanelRoot != null)
        {
            Canvas.SetZIndex(listView.ItemsPanelRoot, -1);
        }

        if (_scrollProperties == null)
        {
            _scrollProperties = ElementCompositionPreview.GetScrollViewerManipulationPropertySet(_scrollViewer);
        }

        if (_scrollProperties == null)
        {
            return false;
        }

        // Implicit operation: Find the Header object of the control if it uses ListViewBase
        if (HeaderElement == null && listView != null)
        {
            HeaderElement = (listView.Header as UIElement)!;
        }

        var headerElement = HeaderElement as FrameworkElement;
        if (headerElement == null || headerElement.RenderSize.Height == 0)
        {
            return false;
        }

        if (_headerVisual == null)
        {
            _headerVisual = ElementCompositionPreview.GetElementVisual(headerElement);
        }

        if (_headerVisual == null)
        {
            return false;
        }

        headerElement.SizeChanged -= ScrollHeader_SizeChanged;
        headerElement.SizeChanged += ScrollHeader_SizeChanged;

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

        if (HeaderElement is FrameworkElement element)
        {
            element.SizeChanged -= ScrollHeader_SizeChanged;
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
            FrameworkElement header = (FrameworkElement)HeaderElement;

            var point = element.TransformToVisual(scroller).TransformPoint(new Point(0, 0));

            if (point.Y < header.ActualHeight)
            {
                scroller.ChangeView(0, scroller.VerticalOffset - (header.ActualHeight - point.Y), 1, false);
            }
        }
    }
}
