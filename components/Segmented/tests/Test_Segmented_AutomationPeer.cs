// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#if !HAS_UNO
using CommunityToolkit.Tests;
using CommunityToolkit.Tooling.TestGen;
using CommunityToolkit.WinUI.Controls;

#if WINAPPSDK
using Microsoft.UI.Xaml.Automation.Provider;
#else
using Windows.UI.Xaml.Automation.Provider;
#endif

namespace SegmentedTests;

[TestClass]
[TestCategory("Test_Segmented_AutomationPeer")]
public partial class Test_Segmented_AutomationPeer : VisualUITestBase
{
    [UIThreadTestMethod]
    public Task StringItemsExposeSelection() => VerifySelectionAsync(false);

    [UIThreadTestMethod]
    public Task ExplicitContainersExposeSelection() => VerifySelectionAsync(true);

    [UIThreadTestMethod]
    public Task StringItemSelectionRespectsIsEnabled() => VerifySelectAsync(false);

    [UIThreadTestMethod]
    public Task ExplicitContainerSelectionRespectsIsEnabled() => VerifySelectAsync(true);

    [UIThreadTestMethod]
    public Task StringItemPeersTrackCollectionChanges() => VerifyCollectionChangesAsync(false);

    [UIThreadTestMethod]
    public Task ExplicitContainerPeersTrackCollectionChanges() => VerifyCollectionChangesAsync(true);

    private async Task VerifySelectionAsync(bool explicitContainers)
    {
        var (control, items) = CreateControl(explicitContainers);
        await LoadTestContentAsync(control);

        var peer = GetPeer(control);
        Assert.AreEqual("View mode", peer.GetName());
        Assert.AreEqual(nameof(Segmented), peer.GetClassName());
        Assert.AreEqual(AutomationControlType.List, peer.GetAutomationControlType());

        var selection = GetSelectionProvider(peer);
        Assert.IsFalse(selection.CanSelectMultiple);

        var itemProviders = GetItemProviders(peer, "List", "Grid", "Details");
        for (var selectedIndex = 0; selectedIndex <= 1; selectedIndex++)
        {
            control.SelectedIndex = selectedIndex;

            Assert.AreSame(items[selectedIndex], control.SelectedItem);
            AssertSelection(selection, itemProviders, selectedIndex);
        }
    }

    private async Task VerifySelectAsync(bool explicitContainers)
    {
        var (control, items) = CreateControl(explicitContainers);
        await LoadTestContentAsync(control);
        control.SelectedIndex = 0;

        var peer = GetPeer(control);
        var selection = GetSelectionProvider(peer);
        var itemProviders = GetItemProviders(peer, "List", "Grid", "Details");

        itemProviders[1].Select();

        Assert.AreEqual(1, control.SelectedIndex);
        Assert.AreSame(items[1], control.SelectedItem);
        AssertSelection(selection, itemProviders, 1);

        control.IsEnabled = false;
        Assert.ThrowsException<ElementNotEnabledException>(() => itemProviders[0].Select());

        Assert.AreEqual(1, control.SelectedIndex);
        Assert.AreSame(items[1], control.SelectedItem);
        AssertSelection(selection, itemProviders, 1);
    }

    private async Task VerifyCollectionChangesAsync(bool explicitContainers)
    {
        var (control, items) = CreateControl(explicitContainers);
        await LoadTestContentAsync(control);
        control.SelectedIndex = 1;

        var peer = GetPeer(control);
        var selection = GetSelectionProvider(peer);
        var originalProviders = GetItemProviders(peer, "List", "Grid", "Details");
        var selectedItem = items[1];

        items.Move(2, 0);
        await CompositionTargetHelper.ExecuteAfterCompositionRenderingAsync(() => { });

        Assert.AreEqual(2, control.SelectedIndex);
        Assert.AreSame(selectedItem, control.SelectedItem);
        AssertSelection(selection, GetItemProviders(peer, "Details", "List", "Grid"), 2);
        AssertSelection(selection, originalProviders, 1);

        items.Remove(selectedItem);
        await CompositionTargetHelper.ExecuteAfterCompositionRenderingAsync(() => { });

        // A retained peer must still refer to List, not the item now at its old index.
        originalProviders[0].Select();

        Assert.AreEqual(1, control.SelectedIndex);
        Assert.AreSame(items[1], control.SelectedItem);
        AssertSelection(selection, GetItemProviders(peer, "Details", "List"), 1);
        AssertSelection(selection, originalProviders, 0);
    }

    private static (Segmented Control, ObservableCollection<object> Items) CreateControl(bool explicitContainers)
    {
        var items = new ObservableCollection<object>();
        foreach (var name in new[] { "List", "Grid", "Details" })
        {
            if (explicitContainers)
            {
                var item = new SegmentedItem { Content = name };
                AutomationProperties.SetName(item, name);
                items.Add(item);
            }
            else
            {
                items.Add(name);
            }
        }

        var control = new Segmented
        {
            Name = "ViewModeControl",
            ItemsSource = items,
            SelectionMode = ListViewSelectionMode.Single,
        };
        AutomationProperties.SetName(control, "View mode");
        return (control, items);
    }

    private static AutomationPeer GetPeer(Segmented control)
    {
        var peer = FrameworkElementAutomationPeer.CreatePeerForElement(control);
        Assert.IsNotNull(peer);
        return peer;
    }

    private static ISelectionProvider GetSelectionProvider(AutomationPeer peer)
    {
        var selection = peer.GetPattern(PatternInterface.Selection) as ISelectionProvider;
        Assert.IsNotNull(selection, "The root must expose the native Selection provider.");
        return selection;
    }

    private static ISelectionItemProvider[] GetItemProviders(AutomationPeer peer, params string[] expectedNames)
    {
        var children = peer.GetChildren();
        Assert.IsNotNull(children);
        Assert.AreEqual(expectedNames.Length, children.Count);

        var providers = new ISelectionItemProvider[children.Count];
        for (var index = 0; index < children.Count; index++)
        {
            Assert.AreEqual(expectedNames[index], children[index].GetName());
            var provider = children[index].GetPattern(PatternInterface.SelectionItem) as ISelectionItemProvider;
            Assert.IsNotNull(provider, $"{expectedNames[index]} must expose the native SelectionItem provider.");
            Assert.IsNotNull(provider.SelectionContainer);
            providers[index] = provider;
        }

        return providers;
    }

    private static void AssertSelection(ISelectionProvider selection, ISelectionItemProvider[] items, int selectedIndex)
    {
        var selectedItems = selection.GetSelection();
        Assert.IsNotNull(selectedItems);
        Assert.AreEqual(1, selectedItems.Length);

        for (var index = 0; index < items.Length; index++)
        {
            Assert.AreEqual(index == selectedIndex, items[index].IsSelected, $"Selection identity at item {index}.");
        }
    }
}
#endif
