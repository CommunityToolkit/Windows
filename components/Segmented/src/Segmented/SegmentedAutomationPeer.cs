// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.WinUI.Controls;

namespace CommunityToolkit.WinUI.Automation.Peers;

/// <summary>
/// Defines an automation peer for the <see cref="Segmented"/> control.
/// </summary>
public partial class SegmentedAutomationPeer : ListViewBaseAutomationPeer
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SegmentedAutomationPeer"/> class.
    /// </summary>
    /// <param name="owner">The <see cref="Segmented"/> associated with this automation peer.</param>
    public SegmentedAutomationPeer(Segmented owner)
        : base(owner)
    {
    }

    /// <inheritdoc/>
    protected override string GetClassNameCore() => nameof(Segmented);

    /// <inheritdoc/>
    protected override ItemAutomationPeer OnCreateItemAutomationPeer(object item)
        => new ListViewItemDataAutomationPeer(item, this);
}
