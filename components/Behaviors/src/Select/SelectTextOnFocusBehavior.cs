// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace CommunityToolkit.WinUI.Behaviors;

/// <summary>
/// This behavior automatically selects the text of the associated <see cref="TextBox"/> when it receives the focus.
/// </summary>
public sealed class SelectTextOnFocusBehavior : BehaviorBase<TextBox>
{
    /// <inheritdoc/>
    protected override bool Initialize()
    {
        AssociatedObject.GotFocus += OnAssociatedObjectGotFocus;
        return base.Initialize();
    }

    /// <inheritdoc/>
    protected override bool Uninitialize()
    {
        AssociatedObject.GotFocus -= OnAssociatedObjectGotFocus;
        return base.Uninitialize();
    }

    private void OnAssociatedObjectGotFocus(object sender, RoutedEventArgs e) => AssociatedObject.SelectAll();
}
