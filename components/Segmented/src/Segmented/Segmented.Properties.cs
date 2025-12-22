// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace CommunityToolkit.WinUI.Controls;

public partial class Segmented
{
    /// <summary>
    /// The backing <see cref="DependencyProperty"/> for the <see cref="Orientation"/> property.
    /// </summary>
    public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(
        nameof(Orientation),
        typeof(Orientation),
        typeof(Segmented),
        new PropertyMetadata(Orientation.Horizontal, (d, e) => ((Segmented)d).OnOrientationChanged()));

    /// <summary>
    /// Gets or sets the orientation.
    /// </summary>
    public Orientation Orientation
    {
        get => (Orientation)GetValue(OrientationProperty);
        set => SetValue(OrientationProperty, value);
    }
}
