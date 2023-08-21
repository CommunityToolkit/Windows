// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Xaml.Interactivity;

namespace CommunityToolkit.WinUI.Behaviors;

public sealed partial class NavigateToUriBehavior : DependencyObject, IAction
{
    /// <summary>
    /// Gets or sets the linked <see cref="NavigateUri"/> instance to invoke.
    /// </summary>
    public Uri NavigateUri
    {
        get => (Uri)GetValue(NavigateUriProperty);
        set => SetValue(NavigateUriProperty, value);
    }

    /// <summary>
    /// Identifies the <seealso cref="NavigateUri"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty NavigateUriProperty = DependencyProperty.Register(
        nameof(NavigateUri),
        typeof(Uri),
        typeof(NavigateToUriBehavior),
        new PropertyMetadata(null));

    /// <inheritdoc/>
    public object Execute(object sender, object parameter)
    {
        if (NavigateUri != null)
        {
            _ = Windows.System.Launcher.LaunchUriAsync(NavigateUri);
        }
        else
        {
            throw new ArgumentNullException(nameof(NavigateUri));
        }

        return true;
    }
}
