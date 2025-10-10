// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections;

namespace CommunityToolkit.WinUI.Collections;

/// <summary>
/// Sort description
/// </summary>
public class SortDescription
{
    /// <summary>
    /// Gets the name of property to sort on
    /// </summary>
    public string PropertyName { get; }

    /// <summary>
    /// Gets the direction of sort
    /// </summary>
    public SortDirection Direction { get; }

    /// <summary>
    /// Gets the comparer
    /// </summary>
    public IComparer Comparer { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SortDescription"/> class that describes
    /// a sort on the object itself
    /// </summary>
    /// <param name="direction">Direction of sort</param>
    /// <param name="comparer">Comparer to use. If null, will use default comparer</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Trimming", "IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code", Justification = "Not using property name")]
    public SortDescription(SortDirection direction, IComparer? comparer = null)
        : this(null!, direction, comparer!)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SortDescription"/> class.
    /// </summary>
    /// <param name="propertyName">Name of property to sort on</param>
    /// <param name="direction">Direction of sort</param>
    /// <param name="comparer">Comparer to use. If null, will use default comparer</param>
#if NET5_0_OR_GREATER
    [System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Item sorting with the property name uses reflection to get the property and is not trim-safe. Either use SortDescription<T> to preserve the required metadata, or use the other constructor without a property name.")]
#endif
    public SortDescription(string propertyName, SortDirection direction, IComparer? comparer = null)
    {
        PropertyName = propertyName;
        Direction = direction;
        Comparer = comparer ?? ObjectComparer.Instance;
    }

    private class ObjectComparer : IComparer
    {
        public static readonly IComparer Instance = new ObjectComparer();

        private ObjectComparer()
        {
        }

        public int Compare(object? x, object? y)
        {
            var cx = x as IComparable;
            var cy = y as IComparable;

            // ReSharper disable once PossibleUnintendedReferenceComparison
            return cx == cy ? 0 : cx == null ? -1 : cy == null ? +1 : cx.CompareTo(cy);
        }
    }
}
