// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using CommunityToolkit.Tests;
using CommunityToolkit.WinUI.Controls;
using CommunityToolkit.Tooling.TestGen;

namespace HeaderedControlsTests;

[TestClass]
public partial class HeaderedTreeViewTestClass : VisualUITestBase
{
    [UIThreadTestMethod]
    public void SimpleUIExamplePageTest(HeaderedTreeViewTestPage page)
    {
        var component = page.FindDescendant<HeaderedTreeView>();
        Assert.IsNotNull(component);
    }
}
