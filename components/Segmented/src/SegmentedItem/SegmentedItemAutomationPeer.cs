// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#if WINAPPSDK
using Microsoft.UI.Xaml.Automation.Provider;
#else
using Windows.UI.Xaml.Automation.Provider;
#endif

namespace CommunityToolkit.WinUI.Controls;


/// <summary>
/// AutomationPeer for Segmented
/// </summary>
public class SegmentedItemAutomationPeer : FrameworkElementAutomationPeer, ISelectionItemProvider
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SettingsCard"/> class.
    /// </summary>
    /// <param name="owner">SettingsCard</param>
    public SegmentedItemAutomationPeer(SegmentedItem owner)
        : base(owner)
    {
    }

    /// <summary>Gets a value indicating whether an item is selected.</summary>
    /// <returns>True if the element is selected; otherwise, false.</returns>
    public bool IsSelected => this.OwnerSegmentedItem.IsSelected;

    /// <summary>Gets the UI Automation provider that implements ISelectionProvider and acts as the container for the calling object.</summary>
    /// <returns>The UI Automation provider.</returns>
    public IRawElementProviderSimple SelectionContainer
    {
        get
        {
            Segmented parent = this.OwnerSegmentedItem.ParentSegmented;
            if (parent == null)
            {
                return null!;
            }

            AutomationPeer peer = FromElement(parent);
#pragma warning disable CS8603 // Possible null reference return.
            return peer != null ? this.ProviderFromPeer(peer) : null;
#pragma warning restore CS8603 // Possible null reference return.
        }
    }

    private SegmentedItem OwnerSegmentedItem
    {
#pragma warning disable CS8603 // Possible null reference return.
        get { return this.Owner as SegmentedItem; }
#pragma warning restore CS8603 // Possible null reference return.
    }

    /// <summary>Adds the current element to the collection of selected items.</summary>
    public void AddToSelection()
    {
        //SegmentedItem owner = this.OwnerSegmentedItem;
        //Segmented parent = owner.ParentSegmented;
        //parent?.SetSelectedItem(owner);
    }

    /// <summary>Removes the current element from the collection of selected items.</summary>
    public void RemoveFromSelection()
    {
        // Cannot remove the selection of a Carousel control.
    }

    /// <summary>Clears any existing selection and then selects the current element.</summary>
    public void Select()
    {
        //SegmentedItem owner = this.OwnerSegmentedItem;
        //Segmented parent = owner.ParentSegmented;
        //parent?.SetSelectedItem(owner);
    }

    /// <summary>
    /// Gets the control type for the element that is associated with the UI Automation peer.
    /// </summary>
    /// <returns>The control type.</returns>
    protected override AutomationControlType GetAutomationControlTypeCore()
    {
        return AutomationControlType.TabItem;
    }

    /// <summary>
    /// Called by GetClassName that gets a human readable name that, in addition to AutomationControlType,
    /// differentiates the control represented by this AutomationPeer.
    /// </summary>
    /// <returns>The string that contains the name.</returns>
    protected override string GetClassNameCore()
    {
        return Owner.GetType().Name;
    }

    /// <summary>
    /// Called by GetName.
    /// </summary>
    /// <returns>
    /// Returns the first of these that is not null or empty:
    /// - Value returned by the base implementation
    /// - Name of the owning CarouselItem
    /// - Carousel class name
    /// </returns>
    protected override string GetNameCore()
    {
        string name = AutomationProperties.GetName(this.OwnerSegmentedItem);
        if (!string.IsNullOrEmpty(name))
        {
            return name;
        }

        name = this.OwnerSegmentedItem.Name;
        if (!string.IsNullOrEmpty(name))
        {
            return name;
        }

        var textBlock = this.OwnerSegmentedItem.FindDescendant<TextBlock>();
        if (textBlock != null)
        {
            return textBlock.Text;
        }

        return base.GetNameCore();
    }

    /// <summary>
    /// Gets the control pattern that is associated with the specified Windows.UI.Xaml.Automation.Peers.PatternInterface.
    /// </summary>
    /// <param name="patternInterface">A value from the Windows.UI.Xaml.Automation.Peers.PatternInterface enumeration.</param>
    /// <returns>The object that supports the specified pattern, or null if unsupported.</returns>
    protected override object GetPatternCore(PatternInterface patternInterface)
    {
        switch (patternInterface)
        {
            case PatternInterface.SelectionItem:
                return this;
        }

        return base.GetPatternCore(patternInterface);
    }

    /// <summary>
    /// Returns the size of the set where the element that is associated with the automation peer is located.
    /// </summary>
    /// <returns>
    /// The size of the set.
    /// </returns>
    protected override int GetSizeOfSetCore()
    {
        int sizeOfSet = base.GetSizeOfSetCore();

        if (sizeOfSet != -1)
        {
            return sizeOfSet;
        }

        SegmentedItem owner = this.OwnerSegmentedItem;
        Segmented parent = owner.ParentSegmented;
        sizeOfSet = parent.Items.Count;

        return sizeOfSet;
    }

    /// <summary>
    /// Returns the ordinal position in the set for the element that is associated with the automation peer.
    /// </summary>
    /// <returns>
    /// The ordinal position in the set.
    /// </returns>
    protected override int GetPositionInSetCore()
    {
        int positionInSet = base.GetPositionInSetCore();

        if (positionInSet != -1)
        {
            return positionInSet;
        }

        SegmentedItem owner = this.OwnerSegmentedItem;
        Segmented parent = owner.ParentSegmented;
        positionInSet = parent.IndexFromContainer(owner) + 1;

        return positionInSet;
    }
}
