// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#if WINAPPSDK
using Microsoft.UI.Xaml.Automation.Provider;
#else
using Windows.UI.Xaml.Automation.Provider;
#endif

namespace CommunityToolkit.WinUI.Controls;

/// <summary>
/// AutomationPeer for Segmented
/// </summary>
public class SegmentedAutomationPeer : SelectorAutomationPeer, ISelectionProvider
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SettingsCard"/> class.
    /// </summary>
    /// <param name="owner">SettingsCard</param>
    public SegmentedAutomationPeer(Segmented owner)
        : base(owner)
    {
    }



    /// <summary>
    /// Gets the control type for the element that is associated with the UI Automation peer.
    /// </summary>
    /// <returns>The control type.</returns>
    protected override AutomationControlType GetAutomationControlTypeCore()
    {
        return AutomationControlType.Tab;
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

    //protected override string GetNameCore()
    //{
    //    return base.GetNameCore();
    //}

  
    
}
