// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Windows.Foundation.Metadata;

namespace CommunityToolkit.WinUI.Behaviors;

internal class ApiInformationHelper
{
#if WINUI2
    // 1903 - 18362
    public static bool IsXamlRootAvailable { get; } = ApiInformation.IsPropertyPresent("Windows.UI.Xaml.UIElement", "XamlRoot");
#elif WINUI3
    public static bool IsXamlRootAvailable => true;
#endif
}
