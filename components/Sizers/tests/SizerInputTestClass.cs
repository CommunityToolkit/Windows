// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.WinUI.Controls;
using CommunityToolkit.Tooling.TestGen;
using CommunityToolkit.Tests;
using CommunityToolkit.WinUI.Controls.Automation.Peers;
using CommunityToolkit.Tests.Input;

namespace SizersTests;

[TestClass]
public partial class SizerInputTestClass : VisualUITestBase
{
    [UIThreadTestMethod]
    public void PropertySizer_TestTouchDrag(PropertySizerTestInitialBinding testControl)
    {
        var propertySizer = testControl.FindDescendant<PropertySizer>();

        Assert.IsNotNull(propertySizer, "Could not find PropertySizer control.");
        Assert.IsNotNull(App.ContentRoot, "Could not find ContentRoot.");
        // Set in XAML Page LINK: PropertySizerTestInitialBinding.xaml#L14
        Assert.AreEqual(300, propertySizer.Binding, "Property Sizer not at expected initial value.");

        var location = App.ContentRoot.CoordinatesToCenter(propertySizer);

        App.CurrentWindow.InjectInput();
    }

    [UIThreadTestMethod]
    public async Task InputInjection_TestClickButton(TouchInjectionTest testControl)
    {
        var button = testControl.FindDescendant<Button>();

        Assert.IsNotNull(button, "Could not find button control.");
        Assert.IsFalse(testControl.WasButtonClicked, "Initial state unexpected. Button shouldn't be clicked yet.");

        // Get location to button.
        var location = App.ContentRoot!.CoordinatesToCenter(button); // TODO: Write a `CoordinatesToCenter` helper?

        InputSimulator sim = App.CurrentWindow.InjectInput();

        sim.StartTouch();
        // Offset location slightly to ensure we're inside the button.
        var pointerId = sim.TouchDown(new Point(location.X, location.Y));
        await Task.Delay(50);
        sim.TouchUp(pointerId);

        // Ensure UI event is processed by our button
        await CompositionTargetHelper.ExecuteAfterCompositionRenderingAsync(() => { });

        // Optional delay for us to be able to see button content change before test shuts down.
        await Task.Delay(250);

        Assert.IsTrue(testControl.WasButtonClicked, "Button wasn't clicked.");

        sim.StopTouch();
    }
}
