// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.Tests;
using CommunityToolkit.Tooling.TestGen;

namespace ExtensionsTests;

[TestClass]
public partial class FrameworkElementExtensionsTests : VisualUITestBase
{
    [TestCategory("FrameworkElementExtension")]
    [UIThreadTestMethod]
    public void FrameworkElementExtension_RelativeAncestor_InDataTemplate(FrameworkElementRelativeAncestorDataTemplateTestPage page)
    {
        var list = page.FindDescendant<ListView>();

        Assert.IsNotNull(list, "Couldn't find listview");

        int count = 0;
        foreach (var item in list.FindDescendants().OfType<TextBlock>())
        {
            count++;
            Assert.AreEqual("Hello", item.Text, "Text didn't match binding of ancestor tag property");
        }

        Assert.AreEqual(3, count, "Didn't find three textblocks");
    }

    [TestCategory("FrameworkElementExtension")]
    [UIThreadTestMethod]
    public async Task FrameworkElementExtension_RelativeAncestor_FreeParentBaseline(FrameworkElementRelativeAncestorDataTemplateTestPage page)
    {
        var text = page.FindDescendant<TextBox>();

        Assert.IsNotNull(text, "Couldn't find TextBox");

        // Grab a hold of a weak reference for TextBox so we can detect when it unloads.
        WeakReference textRef = new(text);
        text = null;

        var parent = page.FindDescendant<Grid>();

        Assert.IsNotNull(parent, "Couldn't find parent Grid");

        // Remove all the children from the grid to simulate it unloading.
        VisualTreeHelper.DisconnectChildrenRecursive(parent);
        parent.Children.Clear();
        parent = null;

        // Wait for the Visual Tree to perform removals and clean-up
        await CompositionTargetHelper.ExecuteAfterCompositionRenderingAsync(() => { });

        // Wait for the .NET Garbage Collector to clean up references
        GC.Collect();
        GC.WaitForPendingFinalizers();

        Assert.IsFalse(textRef.IsAlive, "TextBox is still alive...");
    }

    [Ignore]
    [TestCategory("FrameworkElementExtension")]
    ////[UIThreadTestMethod] - TODO: https://github.com/CommunityToolkit/Tooling-Windows-Submodule/issues/121
    public async Task FrameworkElementExtension_RelativeAncestor_FreeParent(FrameworkElementRelativeAncestorDataTemplateTestPage page)
    {
        var list = page.FindDescendant<ListView>();

        Assert.IsNotNull(list, "Couldn't find listview");

        // Grab a hold of a weak reference for ListView so we can detect when it unloads.
        WeakReference listRef = new(list);
        list = null;

        // Remove all the children from the grid to simulate it unloading.
        VisualTreeHelper.DisconnectChildrenRecursive(page);
        page.Content = null;

        // Wait for the Visual Tree to perform removals and clean-up
        await CompositionTargetHelper.ExecuteAfterCompositionRenderingAsync(() => { });

        // Wait for the .NET Garbage Collector to clean up references
        GC.Collect();
        GC.WaitForPendingFinalizers();

        Assert.IsFalse(listRef.IsAlive, "ListView is still alive...");
    }

    [Ignore]
    [TestCategory("FrameworkElementExtension")]
    ////[UIThreadTestMethod] - TODO: https://github.com/CommunityToolkit/Tooling-Windows-Submodule/issues/121
    public async Task FrameworkElementExtension_RelativeAncestor_FreePageNavigation()
    {
        TaskCompletionSource<bool?> taskCompletionSource = new();
        var frame = new Frame();
        frame.Navigated += OnNavigated;

        await LoadTestContentAsync(frame);

        // Navigate to the new page.
        frame.Navigate(typeof(FrameworkElementRelativeAncestorDataTemplateTestPage), null, new SuppressNavigationTransitionInfo());

        async void OnNavigated(object sender, NavigationEventArgs e)
        {
            frame.Navigated -= OnNavigated;

            // Wait for first Render pass
            await CompositionTargetHelper.ExecuteAfterCompositionRenderingAsync(() => { });

            taskCompletionSource.SetResult(true);
        }

        // Wait for frame to navigate/load
        var result = await taskCompletionSource.Task;
        Assert.IsTrue(result, "Navigation didn't complete");

        // Find the ListView we want to track

        var list = frame.FindDescendant<ListView>();

        Assert.IsNotNull(list, "Couldn't find listview");

        // Grab a hold of a weak reference for ListView so we can detect when it unloads.
        WeakReference listRef = new(list);
        list = null;

        TaskCompletionSource<bool?> taskCompletionSource2 = new();
        frame.Navigated += OnNavigated2;

        async void OnNavigated2(object sender, NavigationEventArgs e)
        {
            frame.Navigated -= OnNavigated2;

            // Wait for first Render pass
            await CompositionTargetHelper.ExecuteAfterCompositionRenderingAsync(() => { });

            taskCompletionSource2.SetResult(true);
        }

        // Navigate to any other page to unload our other one
        frame.Navigate(typeof(BitmapIconExtensionTestPage), null, new SuppressNavigationTransitionInfo());

        // Wait for navigation to complete
        result = await taskCompletionSource2.Task;
        Assert.IsTrue(result, "Navigation didn't complete 2");

        // Wait for the Visual Tree to perform removals and clean-up
        await CompositionTargetHelper.ExecuteAfterCompositionRenderingAsync(() => { });

        // Wait for the .NET Garbage Collector to clean up references
        GC.Collect();
        GC.WaitForPendingFinalizers();

        Assert.IsFalse(listRef.IsAlive, "ListView is still alive...");
    }
}
