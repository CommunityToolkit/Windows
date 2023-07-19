// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace CommunityToolkit.WinUI.Controls;

/// <summary>
/// This is the base control to create consistent settings experiences, inline with the Windows 11 design language.
/// A SettingsCard can also be hosted within a SettingsExpander.
/// </summary>

[TemplatePart(Name = ActionIconPresenterHolder, Type = typeof(Viewbox))]
[TemplatePart(Name = HeaderPresenter, Type = typeof(ContentPresenter))]
[TemplatePart(Name = DescriptionPresenter, Type = typeof(ContentPresenter))]
[TemplatePart(Name = HeaderIconPresenterHolder, Type = typeof(Viewbox))]
public partial class SettingsCard : ButtonBase
{
    internal const string NormalState = "Normal";
    internal const string PointerOverState = "PointerOver";
    internal const string PressedState = "Pressed";
    internal const string DisabledState = "Disabled";

    internal const string ActionIconPresenterHolder = "PART_ActionIconPresenterHolder";
    internal const string HeaderPresenter = "PART_HeaderPresenter";
    internal const string DescriptionPresenter = "PART_DescriptionPresenter";
    internal const string HeaderIconPresenterHolder = "PART_HeaderIconPresenterHolder";
    /// <summary>
    /// Creates a new instance of the <see cref="SettingsCard"/> class.
    /// </summary>
    public SettingsCard()
    {
        this.DefaultStyleKey = typeof(SettingsCard);
    }

    /// <inheritdoc />
    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        IsEnabledChanged -= OnIsEnabledChanged;
        OnActionIconChanged();
        OnHeaderChanged();
        OnHeaderIconChanged();
        OnDescriptionChanged();
        OnIsClickEnabledChanged();
        VisualStateManager.GoToState(this, IsEnabled ? NormalState : DisabledState, true);
        RegisterAutomation();
        IsEnabledChanged += OnIsEnabledChanged;
    }

    private void RegisterAutomation()
    {
        if (Header is string headerString && headerString != string.Empty)
        {
            AutomationProperties.SetName(this, headerString);
            // We don't want to override an AutomationProperties.Name that is manually set, or if the Content basetype is of type ButtonBase (the ButtonBase.Content will be used then)
            if (Content is UIElement element && string.IsNullOrEmpty(AutomationProperties.GetName(element)) && element.GetType().BaseType != typeof(ButtonBase) && element.GetType() != typeof(TextBlock))
            {
                AutomationProperties.SetName(element, headerString);
            }
        }
    }

    private void EnableButtonInteraction()
    {
        DisableButtonInteraction();

        IsTabStop = true;
        PointerEntered += Control_PointerEntered;
        PointerExited += Control_PointerExited;
        PointerCaptureLost += Control_PointerCaptureLost;
        PointerCanceled += Control_PointerCanceled;
        PreviewKeyDown += Control_PreviewKeyDown;
        PreviewKeyUp += Control_PreviewKeyUp;
    }

    private void DisableButtonInteraction()
    {
        IsTabStop = false;
        PointerEntered -= Control_PointerEntered;
        PointerExited -= Control_PointerExited;
        PointerCaptureLost -= Control_PointerCaptureLost;
        PointerCanceled -= Control_PointerCanceled;
        PreviewKeyDown -= Control_PreviewKeyDown;
        PreviewKeyUp -= Control_PreviewKeyUp;
    }

    private void Control_PreviewKeyUp(object sender, KeyRoutedEventArgs e)
    {
        if (e.Key == Windows.System.VirtualKey.Enter || e.Key == Windows.System.VirtualKey.Space || e.Key == Windows.System.VirtualKey.GamepadA)
        {
            VisualStateManager.GoToState(this, NormalState, true);
        }
    }

    private void Control_PreviewKeyDown(object sender, KeyRoutedEventArgs e)
    {
        if (e.Key == Windows.System.VirtualKey.Enter || e.Key == Windows.System.VirtualKey.Space || e.Key == Windows.System.VirtualKey.GamepadA)
        {
            // Check if the active focus is on the card itself - only then we show the pressed state.
            if (GetFocusedElement() is SettingsCard)
            {
                VisualStateManager.GoToState(this, PressedState, true);
            }
        }
    }

    public void Control_PointerEntered(object sender, PointerRoutedEventArgs e)
    {
        base.OnPointerEntered(e);
        VisualStateManager.GoToState(this, PointerOverState, true);
    }
    
    public void Control_PointerExited(object sender, PointerRoutedEventArgs e)
    {
        base.OnPointerExited(e);
        VisualStateManager.GoToState(this, NormalState, true);
    }
    
    private void Control_PointerCaptureLost(object sender, PointerRoutedEventArgs e)
    {
        base.OnPointerCaptureLost(e);
        VisualStateManager.GoToState(this, NormalState, true);
    }

    private void Control_PointerCanceled(object sender, PointerRoutedEventArgs e)
    {
        base.OnPointerCanceled(e);
        VisualStateManager.GoToState(this, NormalState, true);
    }

    protected override void OnPointerPressed(PointerRoutedEventArgs e)
    {
        //  e.Handled = true;
        if (IsClickEnabled)
        {
            base.OnPointerPressed(e);
            VisualStateManager.GoToState(this, PressedState, true);
        }
    }
    
    protected override void OnPointerReleased(PointerRoutedEventArgs e)
    {
        if (IsClickEnabled)
        {
            base.OnPointerReleased(e);
            VisualStateManager.GoToState(this, NormalState, true);
        }
    }

    /// <summary>
    /// Creates AutomationPeer
    /// </summary>
    /// <returns>An automation peer for <see cref="SettingsCard"/>.</returns>
    protected override AutomationPeer OnCreateAutomationPeer()
    {
        return new SettingsCardAutomationPeer(this);
    }

    private void OnIsClickEnabledChanged()
    {
        OnActionIconChanged();
        if (IsClickEnabled)
        {
            EnableButtonInteraction();
        }
        else
        {
            DisableButtonInteraction();
        }
    }

    private void OnIsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        VisualStateManager.GoToState(this, IsEnabled ? NormalState : DisabledState, true);
    }

    private void OnActionIconChanged()
    {
        if (GetTemplateChild(ActionIconPresenterHolder) is FrameworkElement actionIconPresenter)
        {
            if (IsClickEnabled && IsActionIconVisible)
            {
                actionIconPresenter.Visibility = Visibility.Visible;
            }
            else
            {
                actionIconPresenter.Visibility =Visibility.Collapsed;
            }
        }
    }

    private void OnHeaderIconChanged()
    {
        if (GetTemplateChild(HeaderIconPresenterHolder) is FrameworkElement headerIconPresenter)
        {
            headerIconPresenter.Visibility = HeaderIcon != null
                ? Visibility.Visible
                : Visibility.Collapsed;
        }
    }

    private void OnDescriptionChanged()
    {
        if (GetTemplateChild(DescriptionPresenter) is FrameworkElement descriptionPresenter)
        {
            descriptionPresenter.Visibility = Description != null
                ? Visibility.Visible
                : Visibility.Collapsed;
        }
    }

    private void OnHeaderChanged()
    {
        if (GetTemplateChild(HeaderPresenter) is FrameworkElement headerPresenter)
        {
            headerPresenter.Visibility = Header != null
                ? Visibility.Visible
                : Visibility.Collapsed;
        }
    }

    private FrameworkElement? GetFocusedElement()
    {
        if (ControlHelpers.IsXamlRootAvailable && XamlRoot != null)
        {
            return FocusManager.GetFocusedElement(XamlRoot) as FrameworkElement;
        }
        else
        {
            return FocusManager.GetFocusedElement() as FrameworkElement;
        }
    }
}
