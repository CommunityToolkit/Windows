// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using CommunityToolkit.Tests;
using CommunityToolkit.WinUI.Controls;
using CommunityToolkit.Tooling.TestGen;

namespace HeaderedControls.Tests;

[TestClass]
public partial class HeaderedItemsControlTestClass : VisualUITestBase
{
    [UIThreadTestMethod]
    public void SimpleUIExamplePageTest(HeaderedItemsControlTestPage page)
    {
        var component = page.FindDescendant<HeaderedItemsControl>();
        Assert.IsNotNull(component);
    }
}
