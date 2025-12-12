// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.Tests;
using CommunityToolkit.Tooling.TestGen;

namespace PrimitivesTests;

[TestClass]
public partial class DockPanelTests : VisualUITestBase
{
    [UIThreadTestMethod]
    public void DockPanelTest(DockPanelSample page)
    {
        var dockPanel = page.FindDescendant<CommunityToolkit.WinUI.Controls.DockPanel>();

        Assert.IsNotNull(dockPanel, "Couldn't find DockPanel");
    }
}
