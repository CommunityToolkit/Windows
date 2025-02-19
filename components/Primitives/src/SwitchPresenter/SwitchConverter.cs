// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.WinUI.Controls;

namespace CommunityToolkit.WinUI.Converters;

/// <summary>
/// A helper <see cref="IValueConverter"/> which can automatically translate incoming data to a set of resulting values defined in XAML.
/// </summary>
/// <example>
/// &lt;converters:SwitchConverter x:Key=&quot;StatusToColorSwitchConverter&quot;
///                                TargetType=&quot;models:CheckStatus&quot;&gt;
///     &lt;controls:Case Value=&quot;Error&quot; Content=&quot;{ThemeResource SystemFillColorCriticalBrush}&quot;/&gt;
///     &lt;controls:Case Value=&quot;Warning&quot; Content=&quot;{ThemeResource SystemFillColorCautionBrush}&quot;/&gt;
///     &lt;controls:Case Value=&quot;Success&quot; Content=&quot;{ThemeResource SystemFillColorSuccessBrush}&quot;/&gt;
/// &lt;/converters:SwitchConverter&gt;
/// ...
/// &lt;TextBlock
///     FontWeight=&quot;SemiBold&quot;
///     Foreground=&quot;{x:Bind Status, Converter={StaticResource StatusToColorSwitchConverter}}&quot;
///     Text = &quot;{x:Bind Status}&quot; /&gt;
/// </example>
[ContentProperty(Name = nameof(SwitchCases))]
public sealed partial class SwitchConverter : DependencyObject, IValueConverter
{
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
        DependencyProperty.Register(nameof(SwitchCases), typeof(CaseCollection), typeof(SwitchConverter), new PropertyMetadata(null));

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
        DependencyProperty.Register(nameof(TargetType), typeof(Type), typeof(SwitchConverter), new PropertyMetadata(null));

    /// <summary>
    /// Initializes a new instance of the <see cref="SwitchPresenter"/> class.
    /// </summary>
    public SwitchConverter()
    {
        // Note: we need to initialize this here so that XAML can automatically add cases without needing this defined around it as the content.
        // We don't do this in the PropertyMetadata as then we create a static shared collection for all converters, which we don't want. We want it per instance.
        // See https://learn.microsoft.com/windows/uwp/xaml-platform/custom-dependency-properties#initializing-the-collection
        SwitchCases = new CaseCollection();
    }

    /// <inheritdoc/>
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        var result = SwitchCases.EvaluateCases(value, TargetType ?? targetType);

        return result?.Content!;
    }

    /// <inheritdoc/>
    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
