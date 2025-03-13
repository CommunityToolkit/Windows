// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.Tooling.TestGen;
using CommunityToolkit.Tests;
using CommunityToolkit.WinUI.Controls;

namespace TabbedCommandBar.Tests;

[TestClass]
public partial class ExampleTabbedCommandBarTestClass : VisualUITestBase
{
    [UIThreadTestMethod]
    public void SimpleUIExamplePageTest(ExampleTabbedCommandBarTestPage page)
    {
        var component = page.FindDescendant<TabbedCommandBar>();
        Assert.IsNotNull(component);
    }
}
