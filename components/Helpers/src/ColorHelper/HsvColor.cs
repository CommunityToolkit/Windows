// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace CommunityToolkit.WinUI;

/// <summary>
/// Defines a color in Hue/Saturation/Value (HSV) space with alpha.
/// </summary>
public struct HsvColor
{
    private double _hue;
    private double _saturation;
    private double _value;
    private double _alpha;
    
    /// <summary>
    /// Gets or sets the hue.
    /// </summary>
    /// <remarks>
    /// This value is clamped between 0 and 360.
    /// </remarks>
    public double H
    {
        readonly get => _hue;
        set => _hue = Math.Clamp(value, 0, 360);
    }
    
    /// <summary>
    /// Gets or sets the saturation.
    /// </summary>
    /// <remarks>
    /// This value is clamped between 0 and 1.
    /// </remarks>
    public double S
    {
        readonly get => _saturation;
        set => _saturation = Math.Clamp(value, 0, 1);
    }
    
    /// <summary>
    /// Gets or sets the color value.
    /// </summary>
    /// <remarks>
    /// This value is clamped between 0 and 1.
    /// </remarks>
    public double V
    {
        readonly get => _value;
        set => _value = Math.Clamp(value, 0, 1);
    }
    
    /// <summary>
    /// Gets or sets the alpha/opacity.
    /// </summary>
    /// <remarks>
    /// This value is clamped between 0 and 1.
    /// </remarks>
    public double A
    {
        readonly get => _alpha;
        set => _alpha = Math.Clamp(value, 0, 1);
    }
}
