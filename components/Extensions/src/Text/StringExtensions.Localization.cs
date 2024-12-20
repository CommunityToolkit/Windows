// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

// TODO: Need to better understand how this works/maps on Uno between Uno.UI/Uno.WinUI
// Differences to understand with https://github.com/unoplatform/Uno.WindowsCommunityToolkit/blob/uno/CommunityToolkit.WinUI/Extensions/StringExtensions.cs
#if WINAPPSDK && !HAS_UNO
using Microsoft.Windows.ApplicationModel.Resources;
#else
using Windows.ApplicationModel.Resources;
using Windows.UI;
#endif

namespace CommunityToolkit.WinUI;

/// <summary>
/// UWP specific helpers for working with strings.
/// </summary>
public static partial class StringExtensions
{
#if WINAPPSDK && !HAS_UNO
    private static readonly ResourceManager? ResourceManager;
#else
    private static readonly ResourceLoader? IndependentLoader;
#endif

    static StringExtensions()
    {
        try
        {
#if WINAPPSDK && !HAS_UNO
            ResourceManager = new ResourceManager();
#else
            IndependentLoader = ResourceLoader.GetForViewIndependentUse();
#endif
        }
        catch
        {
        }
    }

    //// Note: WindowsAppSDK doesn't have a UIContext, so there's no similar notion of a View specific resource? Thus, no GetViewLocalized currently... TODO: We may need to understand this better moving forward.
#if !WINAPPSDK
    /// <summary>
    /// Retrieves the provided resource for the current view context.
    /// </summary>
    /// <param name="resourceKey">Resource key to retrieve.</param>
    /// <param name="uiContext"><see cref="UIContext"/> to be used to get the <paramref name="resourceKey"/> from.
    /// You can retrieve this from a UIElement.UIContext, XamlRoot.UIContext (XamlIslands), or Window.UIContext.</param>
    /// <returns>string value for given resource or empty string if not found.</returns>
    ////[SupportedOSPlatform("Windows10.0.18362.0")]
    public static string? GetViewLocalized(this string resourceKey, UIContext? uiContext = null)
    {
        if (uiContext != null)
        {
            var resourceLoader = ResourceLoader.GetForUIContext(uiContext);
            return resourceLoader.GetString(resourceKey);
        }
        else
        {
            return ResourceLoader.GetForCurrentView().GetString(resourceKey);
        }
    }
#endif

#if WINAPPSDK && !HAS_UNO
    //// Note: WindowsAppSDK doesn't seem to have the notion of a UIContext, so that's why we have a different signature here.

    /// <summary>
    /// Retrieves the provided resource for the given key for use independent of the UI thread.
    /// </summary>
    /// <param name="resourceKey">Resource key to retrieve.</param>
    /// <returns>string value for given resource or empty string if not found.</returns>
    public static string? GetLocalized(this string resourceKey)
    {
        var result = ResourceManager?.MainResourceMap.TryGetValue(resourceKey)?.ValueAsString;

        if (string.IsNullOrEmpty(result))
        {
            var r = ResourceManager?.MainResourceMap.TryGetSubtree("Resources")?.TryGetValue(resourceKey);
            if (r != null)
            {
                result = r.ValueAsString;
            }
        }

        return result;
    }
#else
    /// <summary>
    /// Retrieves the provided resource for the given key for use independent of the UI thread.
    /// </summary>
    /// <param name="resourceKey">Resource key to retrieve.</param>
    /// <param name="uiContext"><see cref="UIContext"/> to be used to get the <paramref name="resourceKey"/> from.
    /// You can retrieve this from a UIElement.UIContext, XamlRoot.UIContext (XamlIslands), or Window.UIContext.</param>
    /// <returns>string value for given resource or empty string if not found.</returns>
    public static string? GetLocalized(this string resourceKey, UIContext? uiContext = null)
    {
        if (uiContext != null)
        {
            var resourceLoader = ResourceLoader.GetForUIContext(uiContext);
            return resourceLoader?.GetString(resourceKey);
        }
        else
        {
            return IndependentLoader?.GetString(resourceKey);
        }
    }
#endif

    /// <summary>
    /// Retrieves the provided resource for the given key for use independent of the UI thread. First looks up resource at the application level, before falling back to provided resourcePath. This allows for easily overridable resources within a library.
    /// </summary>
    /// <param name="resourceKey">Resource key to retrieve.</param>
    /// <param name="resourcePath">Resource path to fall back to in case resourceKey not found in app resources.</param>
    /// <returns>string value for given resource or empty string if not found.</returns>
    public static string? GetLocalized(this string resourceKey, string resourcePath)
    {
        // Try and retrieve resource at app level first.
#if WINAPPSDK && !HAS_UNO
        var result = ResourceManager?.MainResourceMap.TryGetValue(resourceKey)?.ValueAsString;

        if (string.IsNullOrEmpty(result))
        {
            var r = ResourceManager?.MainResourceMap.TryGetSubtree("Resources")?.TryGetValue(resourceKey);
            if (r != null)
            {
                result = r.ValueAsString;
            }

            if (string.IsNullOrEmpty(result))
            {
                r = ResourceManager?.MainResourceMap.TryGetSubtree(resourcePath)?.TryGetValue(resourceKey);
                if (r != null)
                {
                    result = r.ValueAsString;
                }
            }
        }
#else
        var result = IndependentLoader?.GetString(resourceKey);

        if (string.IsNullOrEmpty(result))
        {
#if HAS_UNO
            result = new ResourceLoader(resourcePath).GetString(resourceKey);
#else
            result = ResourceLoader.GetForViewIndependentUse(resourcePath).GetString(resourceKey);
#endif
        }
#endif

            return result;
    }
}
