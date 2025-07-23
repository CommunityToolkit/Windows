// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.Tests;
using CommunityToolkit.WinUI.Controls;

namespace PrimitivesTests;

[TestClass]
public class Test_WrapPanel_Infinity : VisualUITestBase
{
    [TestCategory("WrapPanel")]
    [TestMethod]
    public async Task Test_WrapPanel_InfinityWidth_WithStretchChild_Last()
    {
        await App.DispatcherQueue.EnqueueAsync(async () =>
        {
            var treeRoot = XamlReader.Load(@"<Page
    xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
    xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
    xmlns:controls=""using:CommunityToolkit.WinUI.Controls"">
    <Grid>
        <Grid.ColumnDefinitions>
            <ColumnDefinition Width=""Auto"" />
        </Grid.ColumnDefinitions>
        <controls:WrapPanel
            x:Name=""WrapPanel""
            Grid.Column=""0""
            StretchChild=""Last"">
            <Border />
            <Border />
            <Border />
        </controls:WrapPanel>
    </Grid>
</Page>") as FrameworkElement;

            Assert.IsNotNull(treeRoot, "Could not load XAML tree.");

            // Initialize Visual Tree
            await LoadTestContentAsync(treeRoot);

            var wrapPanel = treeRoot.FindChild("WrapPanel") as WrapPanel;
            Assert.IsNotNull(wrapPanel, "Could not find WrapPanel in tree.");
            Assert.IsTrue(wrapPanel.IsLoaded, "WrapPanel is not loaded.");
        });
    }
}
