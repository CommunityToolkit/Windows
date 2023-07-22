// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.WinUI.Controls;
using CommunityToolkit.Tooling.TestGen;
using CommunityToolkit.Tests;

namespace ShimmerExperiment.Tests;

[TestClass]
public partial class ExampleShimmerTestClass : VisualUITestBase
{
    // If you don't need access to UI objects directly or async code, use this pattern.
    [TestMethod]
    public void SimpleSynchronousExampleTest()
    {
        var assembly = typeof(Shimmer).Assembly;
        var type = assembly.GetType(typeof(Shimmer).FullName ?? string.Empty);

        Assert.IsNotNull(type, "Could not find Shimmer type.");
        Assert.AreEqual(typeof(Shimmer), type, "Type of Shimmer does not match expected type.");
    }

    // If you don't need access to UI objects directly, use this pattern.
    [TestMethod]
    public async Task SimpleAsyncExampleTest()
    {
        await Task.Delay(250);

        Assert.IsTrue(true);
    }

    // Example that shows how to check for exception throwing.
    [TestMethod]
    public void SimpleExceptionCheckTest()
    {
        // If you need to check exceptions occur for invalid inputs, etc...
        // Use Assert.ThrowsException to limit the scope to where you expect the error to occur.
        // Otherwise, using the ExpectedException attribute could swallow or
        // catch other issues in setup code.
        Assert.ThrowsException<NotImplementedException>(() => throw new NotImplementedException());
    }

    // The UIThreadTestMethod automatically dispatches to the UI for us to work with UI objects.
    [UIThreadTestMethod]
    public void SimpleUIAttributeExampleTest()
    {
        var component = new Shimmer();
        Assert.IsNotNull(component);
    }

    // The UIThreadTestMethod can also easily grab a XAML Page for us by passing its type as a parameter.
    // This lets us actually test a control as it would behave within an actual application.
    // The page will already be loaded by the time your test is called.
    [UIThreadTestMethod]
    public void SimpleUIExamplePageTest(ExampleShimmerTestPage page)
    {
        // You can use the Toolkit Visual Tree helpers here to find the component by type or name:
        var component = page.FindDescendant<Shimmer>();

        Assert.IsNotNull(component);

        var componentByName = page.FindDescendant("ShimmerControl");

        Assert.IsNotNull(componentByName);
    }

    // You can still do async work with a UIThreadTestMethod as well.
    [UIThreadTestMethod]
    public async Task SimpleAsyncUIExamplePageTest(ExampleShimmerTestPage page)
    {
        // This helper can be used to wait for a rendering pass to complete.
        await CompositionTargetHelper.ExecuteAfterCompositionRenderingAsync(() => { });

        var component = page.FindDescendant<Shimmer>();

        Assert.IsNotNull(component);
    }

    //// ----------------------------- ADVANCED TEST SCENARIOS -----------------------------

    // If you need to use DataRow, you can use this pattern with the UI dispatch still.
    // Otherwise, checkout the UIThreadTestMethod attribute above.
    // See https://github.com/CommunityToolkit/Labs-Windows/issues/186
    [TestMethod]
    public async Task ComplexAsyncUIExampleTest()
    {
        await EnqueueAsync(() =>
        {
            var component = new Shimmer();
            Assert.IsNotNull(component);
        });
    }

    // If you want to load other content not within a XAML page using the UIThreadTestMethod above.
    // Then you can do that using the Load/UnloadTestContentAsync methods.
    [TestMethod]
    public async Task ComplexAsyncLoadUIExampleTest()
    {
        await EnqueueAsync(async () =>
        {
            var component = new Shimmer();
            Assert.IsNotNull(component);
            Assert.IsFalse(component.IsLoaded);

            await LoadTestContentAsync(component);

            Assert.IsTrue(component.IsLoaded);

            await UnloadTestContentAsync(component);

            Assert.IsFalse(component.IsLoaded);
        });
    }

    // You can still use the UIThreadTestMethod to remove the extra layer for the dispatcher as well:
    [UIThreadTestMethod]
    public async Task ComplexAsyncLoadUIExampleWithoutDispatcherTest()
    {
        var component = new Shimmer();
        Assert.IsNotNull(component);
        Assert.IsFalse(component.IsLoaded);

        await LoadTestContentAsync(component);

        Assert.IsTrue(component.IsLoaded);

        await UnloadTestContentAsync(component);

        Assert.IsFalse(component.IsLoaded);
    }
}
