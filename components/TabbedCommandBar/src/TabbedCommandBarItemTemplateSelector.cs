// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace CommunityToolkit.WinUI.Controls;

/// <summary>
/// <see cref="DataTemplateSelector"/> used by <see cref="TabbedCommandBar"/> for determining the style of normal vs. contextual <see cref="TabbedCommandBarItem"/>s.
/// </summary>
public partial class TabbedCommandBarItemTemplateSelector : DataTemplateSelector
{
    /// <summary>
    /// Gets or sets the <see cref="Style"/> of a normal <see cref="TabbedCommandBarItem"/>.
    /// </summary>
    public DataTemplate? Normal { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="Style"/> of a contextual <see cref="TabbedCommandBarItem"/>.
    /// </summary>
    public DataTemplate? Contextual { get; set; }

    /// <inheritdoc/>
    protected override DataTemplate SelectTemplateCore(object item)
    {
#pragma warning disable CS8603 // Possible null reference return.
        return item is TabbedCommandBarItem t && t.IsContextual ? Contextual : Normal;
#pragma warning restore CS8603 // Possible null reference return.
    }

    /// <inheritdoc/>
    protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
    {
        return SelectTemplateCore(item);
    }
}
