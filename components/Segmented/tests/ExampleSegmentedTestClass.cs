// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.WinUI.Controls;
using CommunityToolkit.Tooling.TestGen;
using CommunityToolkit.Tests;

namespace SegmentedTests;

[TestClass]
public partial class ExampleSegmentedTestClass : VisualUITestBase
{
    [UIThreadTestMethod]
    public void SimpleUIExamplePageTest(ExampleSegmentedTestPage page)
    {
        var component = page.FindDescendant<Segmented>();
        Assert.IsNotNull(component);
    }
}
