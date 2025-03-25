// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.Tests;
using CommunityToolkit.Tooling.TestGen;

namespace ExtensionsTests;

[TestClass]
public partial class BitmapIconExtensionTests : VisualUITestBase
{
    [TestCategory("BitmapIconExtension")]
    [UIThreadTestMethod]
    public void BitmapIconExtension_MarkupExtension_ProvideImage(BitmapIconExtensionTestPage page)
    {
        var button = page.FindDescendant("ColorButton") as Button;

        Assert.IsNotNull(button, $"Could not find the {nameof(Button)} control in tree.");

        var item = ((MenuFlyout)button.Flyout)?.Items?.FirstOrDefault() as MenuFlyoutItem;

        Assert.IsNotNull(button, $"Could not find the target {nameof(MenuFlyoutItem)} control.");

        var icon = item?.Icon as BitmapIcon;

        Assert.IsNotNull(icon, $"Could not find the {nameof(BitmapIcon)} element in button.");

        Assert.AreEqual(icon.UriSource, new Uri("ms-resource:///Files/Assets/StoreLogo.png"), "Expected ms-resource:///Files/Assets/StoreLogo.png uri.");
        Assert.AreEqual(icon.ShowAsMonochrome, false, "Expected icon not to be monochrome");
    }

    [TestCategory("BitmapIconExtension")]
    [UIThreadTestMethod]
    public void BitmapIconExtension_MarkupExtension_ProvideImageAsMonochrome(BitmapIconExtensionTestPage page)
    {
        var button = page.FindDescendant("MonochromeButton") as Button;

        Assert.IsNotNull(button, $"Could not find the {nameof(Button)} control in tree.");

        var item = ((MenuFlyout)button.Flyout)?.Items?.FirstOrDefault() as MenuFlyoutItem;

        Assert.IsNotNull(button, $"Could not find the target {nameof(MenuFlyoutItem)} control.");

        var icon = item?.Icon as BitmapIcon;

        Assert.IsNotNull(icon, $"Could not find the {nameof(BitmapIcon)} element in button.");

        Assert.AreEqual(icon.ShowAsMonochrome, true, "Expected icon to be monochrome");
    }
}
