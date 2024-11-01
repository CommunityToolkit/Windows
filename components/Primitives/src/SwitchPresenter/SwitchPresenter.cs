// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace CommunityToolkit.WinUI.Controls;

/// <summary>
/// The <see cref="SwitchPresenter"/> is a <see cref="ContentPresenter"/> which can allow a developer to mimic a <c>switch</c> statement within XAML.
/// When provided a set of <see cref="Case"/>s and a <see cref="Value"/>, it will pick the matching <see cref="Case"/> with the corresponding <see cref="Case.Value"/>.
/// </summary>
[ContentProperty(Name = nameof(SwitchCases))]
public sealed partial class SwitchPresenter : ContentPresenter
{
    /// <summary>
    /// Gets the current <see cref="Case"/> which is being displayed.
    /// </summary>
    public Case? CurrentCase
    {
        get { return (Case)GetValue(CurrentCaseProperty); }
        private set { SetValue(CurrentCaseProperty, value); }
    }

    /// <summary>
    /// Identifies the <see cref="CurrentCase"/> property.
    /// </summary>
    public static readonly DependencyProperty CurrentCaseProperty =
        DependencyProperty.Register(nameof(CurrentCase), typeof(Case), typeof(SwitchPresenter), new PropertyMetadata(null));

    /// <summary>
    /// Gets or sets a value representing the collection of cases to evaluate.
    /// </summary>
    public CaseCollection SwitchCases
    {
        get { return (CaseCollection)GetValue(SwitchCasesProperty); }
        set { SetValue(SwitchCasesProperty, value); }
    }

    /// <summary>
    /// Identifies the <see cref="SwitchCases"/> property.
    /// </summary>
    public static readonly DependencyProperty SwitchCasesProperty =
        DependencyProperty.Register(nameof(SwitchCases), typeof(CaseCollection), typeof(SwitchPresenter), new PropertyMetadata(null, OnSwitchCasesPropertyChanged));

    /// <summary>
    /// Gets or sets a value indicating the value to compare all cases against. When this value is bound to and changes, the presenter will automatically evaluate cases and select the new appropriate content from the switch.
    /// </summary>
    public object Value
    {
        get { return (object)GetValue(ValueProperty); }
        set { SetValue(ValueProperty, value); }
    }

    /// <summary>
    /// Identifies the <see cref="Value"/> property.
    /// </summary>
    public static readonly DependencyProperty ValueProperty =
        DependencyProperty.Register(nameof(Value), typeof(object), typeof(SwitchPresenter), new PropertyMetadata(null, OnValuePropertyChanged));

    /// <summary>
    /// Gets or sets a value indicating which type to first cast and compare provided values against.
    /// </summary>
    public Type TargetType
    {
        get { return (Type)GetValue(TargetTypeProperty); }
        set { SetValue(TargetTypeProperty, value); }
    }

    /// <summary>
    /// Identifies the <see cref="TargetType"/> property.
    /// </summary>
    public static readonly DependencyProperty TargetTypeProperty =
        DependencyProperty.Register(nameof(TargetType), typeof(Type), typeof(SwitchPresenter), new PropertyMetadata(null));

    private static void OnValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        // When our Switch's expression changes, re-evaluate.
        if (d is SwitchPresenter xswitch)
        {
            xswitch.EvaluateCases();
        }
    }

    private static void OnSwitchCasesPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        // If our collection somehow changes, we should re-evaluate.
        if (d is SwitchPresenter xswitch)
        {
            xswitch.EvaluateCases();
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SwitchPresenter"/> class.
    /// </summary>
    public SwitchPresenter()
    {
        this.SwitchCases = new CaseCollection();

        Loaded += this.SwitchPresenter_Loaded;
    }

    private void SwitchPresenter_Loaded(object sender, RoutedEventArgs e)
    {
        // In case we're in a template, we may have loaded cases later.
        EvaluateCases();
    }

    /// <inheritdoc/>
    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        EvaluateCases();
    }

    private void EvaluateCases()
    {
        if (CurrentCase?.Value != null &&
            CurrentCase.Value.Equals(Value))
        {
            // If the current case we're on already matches our current value,
            // then we don't have any work to do.
            return;
        }

        var result = SwitchCases.EvaluateCases(Value, TargetType);

        // Only bother changing things around if we actually have a new case. (this should handle prior null case as well)
        if (result != CurrentCase)
        {
            // If we don't have any cases or default, setting these to null is what we want to be blank again.
            Content = result?.Content;
            CurrentCase = result;
        }
    }
}
