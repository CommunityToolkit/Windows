// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace CommunityToolkit.WinUI.Controls;

[DebuggerDisplay("U = {U} V = {V}")]
internal struct UvMeasure
{
    internal double U { get; set; }

    internal double V { get; set; }

    public UvMeasure(Orientation orientation, Size size)
    {
        if (orientation is Orientation.Horizontal)
        {
            U = size.Width;
            V = size.Height;
        }
        else
        {
            U = size.Height;
            V = size.Width;
        }
    }

    public Point GetPoint(Orientation orientation)
    {
        return orientation is Orientation.Horizontal ? new Point(U, V) : new Point(V, U);
    }

    public Size GetSize(Orientation orientation)
    {
        return orientation is Orientation.Horizontal ? new Size(U, V) : new Size(V, U);
    }

    public static bool operator ==(UvMeasure measure1, UvMeasure measure2)
    {
        return measure1.U == measure2.U && measure1.V == measure2.V;
    }

    public static bool operator !=(UvMeasure measure1, UvMeasure measure2)
    {
        return !(measure1 == measure2);
    }

    public override bool Equals(object? obj)
    {
        return obj is UvMeasure measure && this == measure;
    }

    public bool Equals(UvMeasure value)
    {
        return this == value;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}
