// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace CommunityToolkit.WinUI.Controls;

/// <summary>
/// Defines constants that specify how items are sized to fill the available space in a <see cref="StaggeredLayout"/>.
/// </summary>
public enum StaggeredLayoutItemsStretch
{
    /// <summary>
    /// The items' width is determined by the <see cref="StaggeredLayout.DesiredColumnWidth"/>.
    /// </summary>
    None,

    /// <summary>
    /// The items' width is determined by the parent's width. The minimum width is determined by the <see cref="StaggeredLayout.DesiredColumnWidth"/>.
    /// </summary>
    Fill
}
