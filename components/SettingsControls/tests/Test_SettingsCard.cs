// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.Tooling.TestGen;
using CommunityToolkit.Tests;
using CommunityToolkit.WinUI.Controls;

namespace SettingsControlsTests;

[TestClass]
public partial class SettingsCardTestClass : VisualUITestBase
{
    [UIThreadTestMethod]
    public void EmptyNameTest(SettingsCard card)
    {
        // See https://github.com/CommunityToolkit/Windows/issues/310#issue-2066181868
        card.Name = string.Empty;
    }

    [UIThreadTestMethod]
    public void EmptyDescriptionTest(SettingsCard card)
    {
        // See https://github.com/CommunityToolkit/Windows/issues/310#issue-2066181868
        card.Description = string.Empty;
    }

    [UIThreadTestMethod]
    public void SimpleUIExamplePageTest(SettingsCardTestPage page)
    {
        var component = page.FindDescendant<SettingsCard>();
        Assert.IsNotNull(component);
    }
}
