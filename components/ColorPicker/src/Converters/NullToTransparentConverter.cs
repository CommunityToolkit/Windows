// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace CommunityToolkit.WinUI.Controls;

/// <summary>
/// Value converter that converts null values to Transparent.
/// </summary>
public partial class NullToTransparentConverter : IValueConverter
{
    /// <inheritdoc/>
    public object Convert(object value, Type targetType, object parameter, string language) => value;
    
    /// <inheritdoc/>
    public object ConvertBack(object? value, Type targetType, object parameter, string language) => value ??
#if WINUI2
        Windows.UI.Colors.Transparent;
#else
        Microsoft.UI.Colors.Transparent;
#endif
}
