// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.Tests;
using CommunityToolkit.Tooling.TestGen;
using CommunityToolkit.WinUI.Controls;

namespace PrimitivesTests;

[TestClass]
public partial class Test_WrapPanel_StretchChild : VisualUITestBase
{
    /// <summary>
    /// When a WrapPanel is inside a parent with infinite width, the last child cannot stretch to fill the remaining space.
    /// Instead, it should measure to its desired size.
    /// </summary>
    [TestCategory("WrapPanel")]
    [UIThreadTestMethod]
    public void VerticalWrapPanelInsideParentWithInfinityHeightTest(VerticalWrapPanelInsideParentWithInfinityHeight page)
    {
        var wrapPanel = page.FindDescendant<WrapPanel>();
        Assert.IsNotNull(wrapPanel, "Could not find WrapPanel.");
        Assert.IsFalse(wrapPanel.StretchChild is not StretchChild.Last, "WrapPanel StretchChild property not set to Last.");
        Assert.IsFalse(wrapPanel.Children.Count < 1, "No children to test.");

        foreach (var child in wrapPanel.Children.Cast<FrameworkElement>())
        {
            double expectedHeight = child.DesiredSize.Height;
            Assert.AreEqual(expectedHeight, child.ActualHeight, "Child height not as expected.");
        }
    }

    /// <summary>
    /// When a WrapPanel is inside a parent with limited height, the last child with Stretch alignment should fill the remaining space.
    /// </summary>
    [TestCategory("WrapPanel")]
    [UIThreadTestMethod]
    public void VerticalWrapPanelInsideParentWithLimitedHeightTest(VerticalWrapPanelInsideParentWithLimitedHeight page)
    {
        var wrapPanel = page.FindDescendant<WrapPanel>();
        Assert.IsNotNull(wrapPanel, "Could not find WrapPanel.");
        Assert.IsFalse(wrapPanel.StretchChild is not StretchChild.Last, "WrapPanel StretchChild property not set to Last.");
        Assert.IsFalse(wrapPanel.Children.Count < 1, "No children to test.");

        var precedingChildren = wrapPanel.Children.Cast<FrameworkElement>().Take(wrapPanel.Children.Count - 1);

        foreach (var child in precedingChildren)
        {
            double expectedHeight = child.DesiredSize.Height;
            Assert.AreEqual(expectedHeight, child.ActualHeight, "Preceding child height not as expected.");
        }

        var lastChild = wrapPanel.Children.Cast<FrameworkElement>().Last();
        double lastChildExpectedHeight = wrapPanel.ActualHeight - precedingChildren.Sum(child => child.ActualHeight);
        Assert.AreEqual(lastChildExpectedHeight, lastChild.ActualHeight, "Last child height not as expected.");
    }

    /// <summary>
    /// When a WrapPanel is inside a parent with infinite width, the last child cannot stretch to fill the remaining space.
    /// Instead, it should measure to its desired size.
    /// </summary>
    [TestCategory("WrapPanel")]
    [UIThreadTestMethod]
    public void HorizontalWrapPanelInsideParentWithInfinityWidthTest(HorizontalWrapPanelInsideParentWithInfinityWidth page)
    {
        var wrapPanel = page.FindDescendant<WrapPanel>();
        Assert.IsNotNull(wrapPanel, "Could not find WrapPanel.");
        Assert.IsFalse(wrapPanel.StretchChild is not StretchChild.Last, "WrapPanel StretchChild property not set to Last.");
        Assert.IsFalse(wrapPanel.Children.Count < 1, "No children to test.");

        foreach (var child in wrapPanel.Children.Cast<FrameworkElement>())
        {
            double expectedWidth = child.DesiredSize.Width;
            Assert.AreEqual(expectedWidth, child.ActualWidth, "Preceding child width not as expected.");
        }
    }

    /// <summary>
    /// When a WrapPanel is inside a parent with limited width, the last child with Stretch alignment should fill the remaining space.
    /// </summary>
    /// <param name="page"></param>
    [TestCategory("WrapPanel")]
    [UIThreadTestMethod]
    public void HorizontalWrapPanelInsideParentWithLimitedWidthTest(HorizontalWrapPanelInsideParentWithLimitedWidth page)
    {
        var wrapPanel = page.FindDescendant<WrapPanel>();
        Assert.IsNotNull(wrapPanel, "Could not find WrapPanel.");
        Assert.IsFalse(wrapPanel.StretchChild is not StretchChild.Last, "WrapPanel StretchChild property not set to Last.");
        Assert.IsFalse(wrapPanel.Children.Count < 1, "No children to test.");

        var precedingChildren = wrapPanel.Children.Cast<FrameworkElement>().Take(wrapPanel.Children.Count - 1);

        foreach (var child in precedingChildren)
        {
            double expectedWidth = child.DesiredSize.Width;
            Assert.AreEqual(expectedWidth, child.ActualWidth, "Child width not as expected.");
        }

        var lastChild = wrapPanel.Children.Cast<FrameworkElement>().Last();
        double lastChildExpectedWidth = wrapPanel.ActualWidth - precedingChildren.Sum(child => child.ActualWidth);
        Assert.AreEqual(lastChildExpectedWidth, lastChild.ActualWidth, "Last child width not as expected.");
    }

    /// <summary>
    /// When a WrapPanel's width is constrained such that the last child wraps to a new row, the last child with Stretch alignment should fill the entire width of the new row.
    /// </summary>
    /// <param name="page"></param>
    [TestCategory("WrapPanel")]
    [UIThreadTestMethod]
    public void WrapPanelStretchLastChildInNewRowTest(WrapPanelStretchLastChildInNewRow page)
    {
        var wrapPanel = page.FindDescendant<WrapPanel>();
        Assert.IsNotNull(wrapPanel, "Could not find WrapPanel.");
        Assert.IsFalse(wrapPanel.StretchChild is not StretchChild.Last, "WrapPanel StretchChild property not set to Last.");
        Assert.IsFalse(wrapPanel.Children.Count < 1, "No children to test.");

        var precedingChildren = wrapPanel.Children.Cast<FrameworkElement>().Take(wrapPanel.Children.Count - 1);

        foreach (var child in precedingChildren)
        {
            double expectedWidth = child.DesiredSize.Width;
            Assert.AreEqual(expectedWidth, child.ActualWidth, "Child width not as expected.");
        }

        var lastChild = wrapPanel.Children.Cast<FrameworkElement>().Last();
        double lastChildExpectedWidth = wrapPanel.ActualWidth;
        Assert.AreEqual(lastChildExpectedWidth, lastChild.ActualWidth, "Last child width not as expected.");
    }
}
