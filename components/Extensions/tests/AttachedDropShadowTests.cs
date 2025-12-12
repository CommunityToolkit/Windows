// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.Tooling.TestGen;
using CommunityToolkit.Tests;
using CommunityToolkit.WinUI;

namespace ExtensionsTests;

[TestClass]
public partial class AttachedDropShadowTests : VisualUITestBase
{
    // If you don't need access to UI objects directly or async code, use this pattern.
    [TestMethod]
    public void SimpleSynchronousExampleTest()
    {
        var assembly = typeof(Effects).Assembly;
        var type = assembly.GetType(typeof(Effects).FullName ?? string.Empty);

        Assert.IsNotNull(type, "Could not find Effects type.");
        Assert.AreEqual(typeof(Effects), type, "Type of Effects does not match expected type.");
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
        var component = new AttachedDropShadow();
        Assert.IsNotNull(component);
    }

    // The UIThreadTestMethod can also easily grab a XAML Page for us by passing its type as a parameter.
    // This lets us actually test a control as it would behave within an actual application.
    // The page will already be loaded by the time your test is called.
    [UIThreadTestMethod]
    public void AttachedDropShadow_AttachInXaml(AttachedDropShadowTestPage page)
    {
        // You can use the Toolkit Visual Tree helpers here to find the component by type or name:
        var border = page.FindDescendants().Where(e => e is Border).Cast<Border>().ToArray();

        Assert.AreEqual(2, border.Length);

        var target = border[0];

        Assert.AreEqual(target.Name, "ShadowTarget", "Could not get target shadow area.");

        var shadow = Effects.GetShadow(border[1]) as AttachedDropShadow;

        Assert.IsNotNull(shadow, "Could not get attached drop shadow.");

        Assert.AreEqual(target, shadow.CastTo);
        Assert.AreEqual(32, shadow.CornerRadius, 0.05);
    }
}
