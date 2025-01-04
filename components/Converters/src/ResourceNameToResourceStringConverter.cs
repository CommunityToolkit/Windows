// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

// TODO: Need to better understand how this works/maps on Uno between Uno.UI/Uno.WinUI,
// This should be the equivalent code from Uno.WinUI from: https://github.com/unoplatform/Uno.WindowsCommunityToolkit/blob/uno/CommunityToolkit.WinUI.UI/Converters/ResourceNameToResourceStringConverter.cs
#if WINAPPSDK && !HAS_UNO
using Microsoft.Windows.ApplicationModel.Resources;
#else
using Windows.ApplicationModel.Resources;
#endif

namespace CommunityToolkit.WinUI.Converters;

/// <summary>
/// Value converter that look up for the source string in the App Resources strings and returns its value, if found.
/// </summary>
public sealed partial class ResourceNameToResourceStringConverter : IValueConverter
{
#if WINAPPSDK && !HAS_UNO
    private readonly ResourceManager _resourceManager = new ResourceManager();
#else
    private readonly ResourceLoader _resourceLoader = ResourceLoader.GetForViewIndependentUse();
#endif

    /// <summary>
    /// Take the source string as a resource name that will be looked up in the App Resources.
    /// If the resource exists, the value is returned, otherwise.
    /// </summary>
    /// <param name="value">The source string being passed to the target.</param>
    /// <param name="targetType">The type of the target property, as a type reference.</param>
    /// <param name="parameter">Optional parameter. Not used.</param>
    /// <param name="language">The language of the conversion.</param>
    /// <returns>The string corresponding to the resource name.</returns>
    public object? Convert(object value, Type targetType, object parameter, string language)
    {
        var stringValue = value?.ToString();
        if (stringValue is null)
        {
            return string.Empty;
        }

#if WINAPPSDK && !HAS_UNO
        return _resourceManager.MainResourceMap.TryGetValue(stringValue).ValueAsString;
#else
        return _resourceLoader.GetString(stringValue);
#endif
    }

    /// <summary>
    /// Not implemented.
    /// </summary>
    /// <param name="value">The source string being passed to the target.</param>
    /// <param name="targetType">The type of the target property, as a type reference.</param>
    /// <param name="parameter">Optional parameter. Not used.</param>
    /// <param name="language">The language of the conversion.</param>
    /// <returns>The value to be passed to the target dependency property.</returns>
    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
