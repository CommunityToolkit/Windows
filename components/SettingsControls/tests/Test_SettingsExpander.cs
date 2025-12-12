// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.Tooling.TestGen;
using CommunityToolkit.Tests;
using CommunityToolkit.WinUI.Controls;

namespace SettingsControlsTests;

[TestClass]
public partial class SettingsExpanderTestClass : VisualUITestBase
{
    [UIThreadTestMethod]
    public void SimpleUIExamplePageTest(SettingsExpanderTestPage page)
    {
        var component = page.FindDescendant<SettingsExpander>();
        Assert.IsNotNull(component);
    }
}
