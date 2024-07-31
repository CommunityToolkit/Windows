// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace CommunityToolkit.WinUI;

/// <summary>
/// A markup extension that returns a collection of values of a specific <see langword="enum"/>
/// </summary>
[MarkupExtensionReturnType(ReturnType = typeof(Array))]
#if NET8_0_OR_GREATER
[System.Diagnostics.CodeAnalysis.RequiresDynamicCode("It might not be possible to create an array of a user-defined enum type at runtime.")]
#endif
public sealed partial class EnumValuesExtension : MarkupExtension
{
    /// <summary>
    /// Gets or sets the <see cref="global::System.Type"/> of the target <see langword="enum"/>
    /// </summary>
    public Type? Type { get; set; }

    /// <inheritdoc/>
    protected override object ProvideValue()
    {
        // TODO: We should probably make a throw helper and throw here if type is null?
        return Enum.GetValues(Type!);
    }
}
