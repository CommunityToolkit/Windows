// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.Tests;
using CommunityToolkit.Tests.Internal;
using CommunityToolkit.Tooling.TestGen;

namespace BehaviorsExperiment.Tests;

[TestClass]
public partial class ExampleBehaviorsTestClass : VisualUITestBase
{
    // The UIThreadTestMethod can also easily grab a XAML Page for us by passing its type as a parameter.
    // This lets us actually test a control as it would behave within an actual application.
    // The page will already be loaded by the time your test is called.
    [UIThreadTestMethod]
    public void SimpleUIExamplePageTest(ExampleBehaviorsTestPage page)
    {
        // You can use the Toolkit Visual Tree helpers here to find the component by type or name:
        /*var component = page.FindDescendant<Behaviors_ClassicBinding>();

        Assert.IsNotNull(component);

        var componentByName = page.FindDescendant("BehaviorsControl");

        Assert.IsNotNull(componentByName);*/
    }

    // You can still do async work with a UIThreadTestMethod as well.
    [UIThreadTestMethod]
    public async Task SimpleAsyncUIExamplePageTest(ExampleBehaviorsTestPage page)
    {
        // This helper can be used to wait for a rendering pass to complete.
        await CompositionTargetHelper.ExecuteAfterCompositionRenderingAsync(() => { });

        /*var component = page.FindDescendant<Behaviors_ClassicBinding>();

        Assert.IsNotNull(component);*/
    }
}
