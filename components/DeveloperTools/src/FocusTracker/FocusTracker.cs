// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace CommunityToolkit.WinUI.DeveloperTools;

//// <summary>
/// FocusTracker can be used to display information about the current focused XAML element.
/// </summary>
[TemplatePart(Name = PartControlName, Type = typeof(TextBlock))]
[TemplatePart(Name = PartControlType, Type = typeof(TextBlock))]
[TemplatePart(Name = PartControlAutomationName, Type = typeof(TextBlock))]
[TemplatePart(Name = PartControlFirstParentWithName, Type = typeof(TextBlock))]
public partial class FocusTracker : Control
{
    internal const string PartControlName = "PART_ControlName";
    internal const string PartControlType = "PART_ControlType";
    internal const string PartControlAutomationName = "PART_ControlAutomationName";
    internal const string PartControlFirstParentWithName = "PART_ControlFirstParentWithName";

    /// <summary>
    /// Defines the <see cref="IsActive"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty IsActiveProperty = DependencyProperty.Register(nameof(IsActive), typeof(bool), typeof(FocusTracker), new PropertyMetadata(false, OnIsActiveChanged));

    private static void OnIsActiveChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is FocusTracker focusTracker)
        {
            if (e.NewValue != null && (bool)e.NewValue)
            {
                focusTracker.Start();
            }
            else
            {
                focusTracker.Stop();
            }
        }
    }

    private TextBlock? controlName;
    private TextBlock? controlType;
    private TextBlock? controlAutomationName;
    private TextBlock? controlFirstParentWithName;

    /// <summary>
    /// Gets or sets a value indicating whether the tracker is running or not.
    /// </summary>
    public bool IsActive
    {
        get { return (bool)GetValue(IsActiveProperty); }
        set { SetValue(IsActiveProperty, value); }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="FocusTracker"/> class.
    /// </summary>
    public FocusTracker()
    {
        DefaultStyleKey = typeof(FocusTracker);
    }

    /// <summary>
    /// Update the visual state of the control when its template is changed.
    /// </summary>
    protected override void OnApplyTemplate()
    {
        controlName = GetTemplateChild(PartControlName) as TextBlock;
        controlType = GetTemplateChild(PartControlType) as TextBlock;
        controlAutomationName = GetTemplateChild(PartControlAutomationName) as TextBlock;
        controlFirstParentWithName = GetTemplateChild(PartControlFirstParentWithName) as TextBlock;
    }

    private void Start()
    {
        // Get currently focused control once when we start
        if (Windows.Foundation.Metadata.ApiInformation.IsPropertyPresent("Windows.UI.Xaml.UIElement", "XamlRoot") && XamlRoot != null)
        {
            if (FocusManager.GetFocusedElement(XamlRoot) is FrameworkElement element)
            {
                FocusOnControl(element);
            }
        }
        else
        {
            if (FocusManager.GetFocusedElement() is FrameworkElement element)
            {
                FocusOnControl(element);
            }
        }

        // Then use FocusManager event from 1809 to listen to updates
        FocusManager.GotFocus += FocusManager_GotFocus!;
    }

    private void Stop()
    {
        FocusManager.GotFocus -= FocusManager_GotFocus!;
        ClearContent();
    }

    private void FocusManager_GotFocus(object sender, FocusManagerGotFocusEventArgs e)
    {
        if (e.NewFocusedElement is FrameworkElement element)
        {
            FocusOnControl(element);
        }
    }

    private void ClearContent()
    {
        if (controlName != null)
        {
            controlName.Text = string.Empty;
        }

        if (controlType != null)
        {
            controlType.Text = string.Empty;
        }

        if (controlAutomationName != null)
        {
            controlAutomationName.Text = string.Empty;
        }

        if (controlFirstParentWithName != null)
        {
            controlFirstParentWithName.Text = string.Empty;
        }
    }

    private void FocusOnControl(FrameworkElement focusedControl)
    {
        if (focusedControl == null)
        {
            ClearContent();
            return;
        }

        if (controlName != null)
        {
            controlName.Text = focusedControl.Name;
        }

        if (controlType != null)
        {
            controlType.Text = focusedControl.GetType().Name;
        }

        if (controlAutomationName != null)
        {
            controlAutomationName.Text = AutomationProperties.GetName(focusedControl);
        }

        if (controlFirstParentWithName != null)
        {
            var parentWithName = FindVisualAscendantWithName(focusedControl);
            controlFirstParentWithName.Text = parentWithName?.Name ?? string.Empty;
        }
    }

    private FrameworkElement? FindVisualAscendantWithName(FrameworkElement element)
    {
        var parent = VisualTreeHelper.GetParent(element) as FrameworkElement;

        if (parent == null)
        {
            return null;
        }

        if (!string.IsNullOrEmpty(parent.Name))
        {
            return parent;
        }

        return FindVisualAscendantWithName(parent);
    }
}
