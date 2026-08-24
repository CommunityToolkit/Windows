// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.Tooling.TestGen;
using CommunityToolkit.Tests;
using CommunityToolkit.WinUI.Controls;
using Microsoft.UI.Xaml.Controls;
using ColorPicker = CommunityToolkit.WinUI.Controls.ColorPicker;

namespace ColorPickerTests;

[TestClass]
public partial class PaletteSelectionTests : VisualUITestBase
{
    // First color in FluentColorPalette (#ffb900)
    private static readonly Windows.UI.Color PaletteColor = Windows.UI.Color.FromArgb(255, 255, 185, 0);

    // Second palette color for the round-trip check (#d13438)
    private static readonly Windows.UI.Color PaletteColor2 = Windows.UI.Color.FromArgb(255, 209, 52, 56);

    // White is not in the FluentColorPalette
    private static readonly Windows.UI.Color NonPaletteColor = Colors.White;

    /// <summary>
    /// Verifies that the palette GridView selection follows the Color property when it is changed
    /// programmatically (e.g. via the spectrum or sliders). Regression test for issue #833.
    /// </summary>
    [UIThreadTestMethod]
    public async Task PaletteGridView_FollowsColorProperty()
    {
        var colorPicker = new ColorPicker();
        await LoadTestContentAsync(colorPicker);
        await CompositionTargetHelper.ExecuteAfterCompositionRenderingAsync(() => { });

        // The PaletteItemGridView lives inside a SwitchPresenter Case that is only added to
        // the visual tree once the palette tab is selected. Navigate there first.
        var panelSelector = colorPicker.FindDescendant<Segmented>();
        Assert.IsNotNull(panelSelector, "Could not find ColorPanelSelector in visual tree.");

        var paletteItem = panelSelector.FindDescendant("PaletteItem") as SegmentedItem;
        Assert.IsNotNull(paletteItem, "Could not find PaletteItem in ColorPanelSelector.");

        panelSelector.SelectedItem = paletteItem;
        await CompositionTargetHelper.ExecuteAfterCompositionRenderingAsync(() => { });

        var paletteGridView = colorPicker.FindDescendant("PaletteItemGridView") as GridView;
        Assert.IsNotNull(paletteGridView, "Could not find PaletteItemGridView in visual tree.");

        // Set to a color that is in the default palette — selection should follow.
        colorPicker.Color = PaletteColor;
        await CompositionTargetHelper.ExecuteAfterCompositionRenderingAsync(() => { });

        Assert.AreEqual(PaletteColor, paletteGridView.SelectedItem,
            "PaletteItemGridView should select the matching palette color when Color is set.");

        // Set to a color not in the palette — selection should be cleared.
        colorPicker.Color = NonPaletteColor;
        await CompositionTargetHelper.ExecuteAfterCompositionRenderingAsync(() => { });

        Assert.IsNull(paletteGridView.SelectedItem,
            "PaletteItemGridView should have no selection when Color is not in the palette.");

        // Set to a different palette color — selection should update to match.
        colorPicker.Color = PaletteColor2;
        await CompositionTargetHelper.ExecuteAfterCompositionRenderingAsync(() => { });

        Assert.AreEqual(PaletteColor2, paletteGridView.SelectedItem,
            "PaletteItemGridView should update its selection when Color changes to another palette color.");

        await UnloadTestContentAsync(colorPicker);
    }
}
