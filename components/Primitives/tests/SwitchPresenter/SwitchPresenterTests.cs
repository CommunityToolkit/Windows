// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.Tests;
using CommunityToolkit.Tooling.TestGen;
using CommunityToolkit.WinUI.Controls;
using CommunityToolkit.WinUI.Converters;

namespace PrimitivesTests;

[TestClass]
public partial class SwitchPresenterTests : VisualUITestBase
{
    [UIThreadTestMethod]
    public async Task SwitchPresenterLayoutTest(SwitchPresenterLayoutSample page)
    {
        var spresenter = page.FindDescendant<SwitchPresenter>();
        var combobox = page.FindDescendant<ComboBox>();

        Assert.IsNotNull(spresenter, "Couldn't find SwitchPresenter");
        Assert.IsNotNull(combobox, "Couldn't find ComboBox");

        var dcase = spresenter.SwitchCases.OfType<Case>().FirstOrDefault(@case => @case.IsDefault);

        Assert.IsNotNull(dcase, "Couldn't find default case");

        // Are we in our initial case?
        Assert.AreEqual("Select an option", spresenter.Value, "SwitchPresenter not expected starting value");
        Assert.AreEqual(dcase, spresenter.CurrentCase, "SwitchPresenter not at current default case");

        // Can we find our matching textbox?
        var tbox = spresenter.FindDescendant<TextBlock>();
        Assert.IsNotNull(tbox, "Couldn't find inner textbox");
        Assert.AreEqual("Please select a way to lookup your reservation above...", tbox.Text, "Unexpected content for SwitchPresenter default case");

        // Update combobox
        combobox.SelectedIndex = 1;

        // Wait for update
        await CompositionTargetHelper.ExecuteAfterCompositionRenderingAsync(() => { });

        // Are we in the new case?
        Assert.AreEqual("Confirmation Code", spresenter.Value);

        var ccase = spresenter.SwitchCases.OfType<Case>().FirstOrDefault(@case => @case.Value?.ToString() == "Confirmation Code");

        Assert.IsNotNull(ccase, "Couldn't find expected case");

        Assert.AreEqual(ccase, spresenter.CurrentCase, "SwitchPresenter didn't change cases");

        var txtbox = spresenter.FindDescendant<TextBox>();
        Assert.IsNotNull(txtbox, "Couldn't find new textbox");
        Assert.AreEqual("Confirmation code", txtbox.Header, "Textbox header not expected value");
    }

    [UIThreadTestMethod]
    public async Task SwitchConverterBrushTest(SwitchConverterBrushSample page)
    {
        var combobox = page.FindDescendant<ComboBox>();
        var textblock = page.FindDescendant("ResultBlock") as TextBlock;

        Assert.IsNotNull(combobox, "Couldn't find ComboBox");
        Assert.IsNotNull(textblock, "Couldn't find SwitchPresenter");

        // Are we in our initial case?
        Assert.AreEqual(0, combobox.SelectedIndex, "ComboBox not initialized");
        Assert.AreEqual("Success", combobox.SelectedItem, "Not expected starting value in ComboBox");

        // Check the TextBlock's brush
        Assert.AreEqual(((SolidColorBrush)page.Resources["SystemFillColorSuccessBrush"]).Color, ((SolidColorBrush)textblock.Foreground).Color, "TextBlock not in initial success state");

        // Update combobox
        combobox.SelectedIndex = 1;

        // Wait for update
        await CompositionTargetHelper.ExecuteAfterCompositionRenderingAsync(() => { });

        // Are we in the new case?
        Assert.AreEqual("Warning", combobox.SelectedItem, "ComboBox didn't change");

        // Check the TextBlock's brush
        Assert.AreEqual(((SolidColorBrush)page.Resources["SystemFillColorCautionBrush"]).Color, ((SolidColorBrush)textblock.Foreground).Color, "TextBlock not in new state");

        // Update combobox
        combobox.SelectedIndex = 2;

        // Wait for update
        await CompositionTargetHelper.ExecuteAfterCompositionRenderingAsync(() => { });

        // Are we in the new case?
        Assert.AreEqual("Error", combobox.SelectedItem, "ComboBox didn't change 2");

        // Check the TextBlock's brush
        Assert.AreEqual(((SolidColorBrush)page.Resources["SystemFillColorCriticalBrush"]).Color, ((SolidColorBrush)textblock.Foreground).Color, "TextBlock not in final state");
    }

    [UIThreadTestMethod]
    public void SwitchConverterDirectTest()
    {
        // Multiply by 10
        SwitchConverter sconverter = new()
        {
            SwitchCases = new CaseCollection {
                new Case()
                {
                    Content = 10,
                    Value = 1,
                },
                new Case()
                {
                    Content = 50,
                    Value = 5,
                    IsDefault = true
                },
                new Case()
                {
                    Content = 30,
                    Value = 3,
                },
                new Case()
                {
                    Content = 20,
                    Value = 2,
                },
            },
            TargetType = typeof(int)
        };

        Assert.IsNotNull(sconverter);
        Assert.AreEqual(4, sconverter.SwitchCases.Count, "Not 4 cases");

        var @default = sconverter.Convert(100, typeof(int), string.Empty, string.Empty);

        Assert.AreEqual(50, @default, "Unexpected default return");

        Assert.AreEqual(10, sconverter.Convert(1, typeof(int), string.Empty, string.Empty), "Unexpected result with 1");
        Assert.AreEqual(20, sconverter.Convert(2, typeof(int), string.Empty, string.Empty), "Unexpected result with 2");
        Assert.AreEqual(30, sconverter.Convert(3, typeof(int), string.Empty, string.Empty), "Unexpected result with 3");
        Assert.AreEqual(50, sconverter.Convert(5, typeof(int), string.Empty, string.Empty), "Unexpected result with 5");
    }
}
