// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace CommunityToolkit.WinUI.Controls;

/// <summary>
/// A struct representing a coordinate in UV adjusted space.
/// </summary>
[DebuggerDisplay("({U}u,{V}v)")]
public struct UVCoord: IEquatable<UVCoord>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UVCoord"/> struct.
    /// </summary>
    public UVCoord(Orientation orientation)
    {
        Orientation = orientation;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UVCoord"/> struct.
    /// </summary>
    public UVCoord(double x, double y, Orientation orientation)
    {
        X = x;
        Y = y;
        Orientation = orientation;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UVCoord"/> struct.
    /// </summary>
    public UVCoord(Point point, Orientation orientation) : this(point.X, point.Y, orientation)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UVCoord"/> struct.
    /// </summary>
    public UVCoord(Size size, Orientation orientation) : this(size.Width, size.Height, orientation)
    {
    }

    /// <summary>
    /// Gets or sets the X coordinate.
    /// </summary>
    public double X { readonly get; set; }

    /// <summary>
    /// Gets or sets the Y coordinate.
    /// </summary>
    public double Y { readonly get; set; }

    /// <summary>
    /// Gets or sets the orientation for translation between the XY and UV coordinate systems.
    /// </summary>
    public Orientation Orientation { get; set; }

    /// <summary>
    /// Gets or sets the U coordinate.
    /// </summary>
    public double U
    {
        readonly get => Orientation is Orientation.Horizontal ? X : Y;
        set
        {
            if (Orientation is Orientation.Horizontal)
            {
                X = value;
            }
            else
            {
                Y = value;
            }
        }
    }

    /// <summary>
    /// Gets or sets the V coordinate.
    /// </summary>
    public double V
    {
        readonly get => Orientation is Orientation.Vertical ? X : Y;
        set
        {
            if (Orientation is Orientation.Vertical)
            {
                X = value;
            }
            else
            {
                Y = value;
            }
        }
    }

    /// <summary>
    /// Implicitly casts a <see cref="UVCoord"/> to a <see cref="Point"/>.
    /// </summary>
    public static implicit operator Point(UVCoord uv) => new(uv.X, uv.Y);

    /// <summary>
    /// Implicitly casts a <see cref="UVCoord"/> to a <see cref="Size"/>.
    /// </summary>
    public static implicit operator Size(UVCoord uv) => new(uv.X, uv.Y);

    public static UVCoord operator +(UVCoord addend1, UVCoord addend2)
    {
        if (addend1.Orientation != addend2.Orientation)
        {
            throw new InvalidOperationException($"Cannot add {nameof(UVCoord)} with mismatched {nameof(Orientation)}.");
        }

        var xSum = addend1.X + addend2.X;
        var ySum = addend1.Y + addend2.Y;
        var orientation = addend1.Orientation;
        return new UVCoord(xSum, ySum, orientation);
    }

    public static bool operator ==(UVCoord coord1, UVCoord coord2)
    {
        return coord1.U == coord2.U && coord1.V == coord2.V;
    }

    public static bool operator !=(UVCoord measure1, UVCoord measure2)
    {
        return !(measure1 == measure2);
    }

    public override bool Equals(object? obj)
    {
        return obj is UVCoord other && Equals(other);
    }

    public bool Equals(UVCoord other)
    {
        return this == other;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(U, V);
    }
}
