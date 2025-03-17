// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using CommunityToolkit.Tests;
using CommunityToolkit.WinUI.Controls;
using CommunityToolkit.Tooling.TestGen;

namespace HeaderedControlsTests;

[TestClass]
public partial class HeaderedContentControlTestClass : VisualUITestBase
{
    // The UIThreadTestMethod can also easily grab a XAML Page for us by passing its type as a parameter.
    // This lets us actually test a control as it would behave within an actual application.
    // The page will already be loaded by the time your test is called.
    [UIThreadTestMethod]
    public void SimpleUIExamplePageTest(HeaderedContentControlTestPage page)
    {
        var component = page.FindDescendant<HeaderedContentControl>();
        Assert.IsNotNull(component);
    }
}
