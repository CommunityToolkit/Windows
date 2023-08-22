// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Xaml.Interactivity;

namespace CommunityToolkit.WinUI.Behaviors;

/// <summary>
/// NavigateToUriAction represents an action that allows navigate to a specified URL defined in XAML, similiar to a Hyperlink and HyperlinkButton. No action will be invoked if the Uri cannot be navigated to.
/// </summary>
public sealed partial class NavigateToUriAction : DependencyObject, IAction
{
    /// <summary>
    /// Gets or sets the Uniform Resource Identifier (URI) to navigate to when the object is clicked.
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
        typeof(NavigateToUriAction),
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
