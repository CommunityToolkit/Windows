// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace CommunityToolkit.WinUI.Controls;

internal static partial class ControlHelpers
{
#if WINUI3
    internal static bool IsXamlRootAvailable => true;
#else
    internal static bool IsXamlRootAvailable { get; } = Windows.Foundation.Metadata.ApiInformation.IsPropertyPresent("Windows.UI.Xaml.UIElement", "XamlRoot");
#endif
}
