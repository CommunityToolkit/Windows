// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace CommunityToolkit.WinUI.Converters;

/// <summary>
/// This class converts a collection size to visibility.
/// </summary>
public partial class CollectionVisibilityConverter : EmptyCollectionToObjectConverter
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CollectionVisibilityConverter"/> class.
    /// </summary>
    public CollectionVisibilityConverter()
    {
        NotEmptyValue = Visibility.Visible;
        EmptyValue = Visibility.Collapsed;
    }
}
