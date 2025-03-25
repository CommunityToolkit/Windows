using CommunityToolkit.Tests;
using CommunityToolkit.Tooling.TestGen;

namespace PrimitivesTests;

[TestClass]
public partial class DockPanelTests : VisualUITestBase
{
    [UIThreadTestMethod]
    public void DockPanelTest(DockPanelSample page)
    {
        var dockPanel = page.FindDescendant<CommunityToolkit.WinUI.Controls.DockPanel>();

        Assert.IsNotNull(dockPanel, "Couldn't find DockPanel");
    }
}
