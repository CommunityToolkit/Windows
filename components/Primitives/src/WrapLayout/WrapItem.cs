// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace CommunityToolkit.WinUI.Controls;

internal class WrapItem
{
    public WrapItem(int index)
    {
        Index = index;
    }

    public int Index { get; }

    public UvMeasure? Measure { get; internal set; }

    public UvMeasure? Position { get; internal set; }

    public UIElement? Element { get; internal set; }
}
