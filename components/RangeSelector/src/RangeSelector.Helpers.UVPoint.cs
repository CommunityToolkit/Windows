// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace CommunityToolkit.WinUI.Controls;

/// <summary>
/// A helper struct that abstracts orientation-dependent coordinate handling.
/// U represents the primary axis (X for horizontal, Y for vertical).
/// V represents the secondary axis (Y for horizontal, X for vertical).
/// </summary>
internal readonly struct UVPoint
{
    /// <summary>
    /// Gets the orientation this point is configured for.
    /// </summary>
    public Orientation Orientation { get; }

    /// <summary>
    /// Gets the primary axis coordinate (X for horizontal, Y for vertical).
    /// </summary>
    public double U { get; }

    /// <summary>
    /// Gets the secondary axis coordinate (Y for horizontal, X for vertical).
    /// </summary>
    public double V { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="UVPoint"/> struct.
    /// </summary>
    /// <param name="orientation">The orientation context.</param>
    /// <param name="u">The primary axis value.</param>
    /// <param name="v">The secondary axis value.</param>
    public UVPoint(Orientation orientation, double u, double v = 0)
    {
        Orientation = orientation;
        U = u;
        V = v;
    }

    /// <summary>
    /// Gets the X coordinate (U for horizontal, V for vertical).
    /// </summary>
    public double X => Orientation == Orientation.Horizontal ? U : V;

    /// <summary>
    /// Gets the Y coordinate (V for horizontal, U for vertical).
    /// </summary>
    public double Y => Orientation == Orientation.Horizontal ? V : U;

    /// <summary>
    /// Creates a UVPoint from a Point with the specified orientation.
    /// </summary>
    /// <param name="point">The point in X/Y coordinates.</param>
    /// <param name="orientation">The orientation context.</param>
    /// <returns>A new UVPoint with converted coordinates.</returns>
    public static UVPoint FromPoint(Point point, Orientation orientation)
    {
        return orientation == Orientation.Horizontal
            ? new UVPoint(orientation, point.X, point.Y)
            : new UVPoint(orientation, point.Y, point.X);
    }

    /// <summary>
    /// Converts this UVPoint to a Point in X/Y coordinates.
    /// </summary>
    /// <returns>A Point with X/Y coordinates.</returns>
    public Point ToPoint() => new Point(X, Y);

    /// <summary>
    /// Creates a new UVPoint with the specified U value, preserving V and orientation.
    /// </summary>
    /// <param name="u">The new U value.</param>
    /// <returns>A new UVPoint with the updated U value.</returns>
    public UVPoint WithU(double u) => new UVPoint(Orientation, u, V);

    /// <summary>
    /// Creates a new UVPoint with the specified V value, preserving U and orientation.
    /// </summary>
    /// <param name="v">The new V value.</param>
    /// <returns>A new UVPoint with the updated V value.</returns>
    public UVPoint WithV(double v) => new UVPoint(Orientation, U, v);
}

/// <summary>
/// Extension methods for UVPoint operations on UI elements.
/// </summary>
internal static class UVPointExtensions
{
    /// <param name="element">The element to position.</param>
    extension(UIElement? element)
    {
        /// <summary>
        /// Sets the Canvas position of an element using UVPoint coordinates.
        /// For horizontal: sets Canvas.Left from U.
        /// For vertical: sets Canvas.Top from U.
        /// </summary>
        /// <param name="point">The UVPoint containing position data.</param>
        public void SetCanvasU(UVPoint point)
        {
            if (element == null) return;

            if (point.Orientation == Orientation.Horizontal)
            {
                Canvas.SetLeft(element, point.U);
            }
            else
            {
                Canvas.SetTop(element, point.U);
            }
        }

        /// <summary>
        /// Gets the Canvas position of an element as a U coordinate.
        /// For horizontal: returns Canvas.Left.
        /// For vertical: returns Canvas.Top.
        /// </summary>
        /// <param name="orientation">The orientation context.</param>
        /// <returns>The position along the primary axis.</returns>
        public double GetCanvasU(Orientation orientation)
        {
            if (element == null) return 0;

            return orientation == Orientation.Horizontal
                ? Canvas.GetLeft(element)
                : Canvas.GetTop(element);
        }

        /// <summary>
        /// Clears the Canvas position property for the primary axis.
        /// For horizontal: clears Canvas.Left.
        /// For vertical: clears Canvas.Top.
        /// </summary>
        /// <param name="orientation">The orientation context.</param>
        public void ClearCanvasU(Orientation orientation)
        {
            if (element == null) return;

            if (orientation == Orientation.Horizontal)
            {
                element.ClearValue(Canvas.LeftProperty);
            }
            else
            {
                element.ClearValue(Canvas.TopProperty);
            }
        }

        /// <summary>
        /// Clears the Canvas position property for the secondary axis.
        /// For horizontal: clears Canvas.Top.
        /// For vertical: clears Canvas.Left.
        /// </summary>
        /// <param name="orientation">The orientation context.</param>
        public void ClearCanvasV(Orientation orientation)
        {
            if (element == null) return;

            if (orientation == Orientation.Horizontal)
            {
                element.ClearValue(Canvas.TopProperty);
            }
            else
            {
                element.ClearValue(Canvas.LeftProperty);
            }
        }
    }

    /// <param name="element">The element to measure.</param>
    extension(FrameworkElement? element)
    {
        /// <summary>
        /// Gets the size along the primary axis (Width for horizontal, Height for vertical).
        /// </summary>
        /// <param name="orientation">The orientation context.</param>
        /// <returns>The size along the primary axis.</returns>
        public double GetSizeU(Orientation orientation)
        {
            if (element == null) return 0;

            return orientation == Orientation.Horizontal
                ? element.Width
                : element.Height;
        }

        /// <summary>
        /// Gets the actual size along the primary axis (ActualWidth for horizontal, ActualHeight for vertical).
        /// </summary>
        /// <param name="orientation">The orientation context.</param>
        /// <returns>The actual size along the primary axis.</returns>
        public double GetActualSizeU(Orientation orientation)
        {
            if (element == null) return 0;

            return orientation == Orientation.Horizontal
                ? element.ActualWidth
                : element.ActualHeight;
        }
    }

    /// <summary>
    /// Gets the desired size along the primary axis from DesiredSize.
    /// </summary>
    /// <param name="element">The element to measure.</param>
    /// <param name="orientation">The orientation context.</param>
    /// <returns>The desired size along the primary axis.</returns>
    public static double GetDesiredSizeU(this UIElement? element, Orientation orientation)
    {
        if (element == null) return 0;

        return orientation == Orientation.Horizontal
            ? element.DesiredSize.Width
            : element.DesiredSize.Height;
    }

    /// <summary>
    /// Sets the size along the primary axis (Width for horizontal, Height for vertical).
    /// </summary>
    /// <param name="element">The element to resize.</param>
    /// <param name="orientation">The orientation context.</param>
    /// <param name="size">The size value to set.</param>
    public static void SetSizeU(this FrameworkElement? element, Orientation orientation, double size)
    {
        if (element == null) return;

        if (orientation == Orientation.Horizontal)
        {
            element.Width = size;
        }
        else
        {
            element.Height = size;
        }
    }
}
