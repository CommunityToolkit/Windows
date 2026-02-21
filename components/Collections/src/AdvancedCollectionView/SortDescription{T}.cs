// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace CommunityToolkit.WinUI.Collections;

/// <summary>
/// A generic version of <see cref="SortDescription"/> which preserves the required metadata for reflection-based sorting.
/// </summary>
/// <typeparam name="T">The type to sort</typeparam>
public sealed class SortDescription<
#if NET8_0_OR_GREATER
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)]
#endif
    T> : SortDescription
{
    private readonly PropertyInfo _prop;

    /// <summary>
    /// Initializes a new instance of the <see cref="SortDescription{T}"/> class.
    /// </summary>
    /// <param name="propertyName">Name of property to sort on</param>
    /// <param name="direction">Direction of sort</param>
    /// <param name="comparer">Comparer to use. If null, will use default comparer</param>
#if NET8_0_OR_GREATER
    [UnconditionalSuppressMessage("Trimming", "IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code",
        Justification = "This class preserves metadata and ensures at runtime that the received type is compatible.")]
#endif
    public SortDescription(string propertyName, SortDirection direction, IComparer? comparer = null) : base(propertyName, direction, comparer)
    {
        _prop = typeof(T).GetProperty(propertyName) ?? throw new ArgumentException($"Could not find property {propertyName}");
    }

    internal override PropertyInfo? GetProperty(Type type) =>
        (_prop.DeclaringType is not null && _prop.DeclaringType.IsAssignableFrom(type)) ? _prop : throw new ArgumentException("This instance of SortDescription is not compatible with the desired type");
}
