using CommunityToolkit.Tests;
using CommunityToolkit.Tooling.TestGen;
using CommunityToolkit.WinUI.Controls;

namespace PrimitivesExperiment.Tests;

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
}
