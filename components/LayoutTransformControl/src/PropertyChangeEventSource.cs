// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace CommunityToolkit.WinUI.Controls;

/// <summary>
/// Allows raise an event when the value of a dependency property changes when a view model is otherwise not necessary.
/// </summary>
/// <typeparam name="TPropertyType">Type of the DependencyProperty</typeparam>
internal partial class PropertyChangeEventSource<TPropertyType>
{
    private readonly DependencyObject _source;
    private readonly DependencyProperty _property;
    private readonly long _token;

    /// <summary>
    /// Occurs when the value changes.
    /// </summary>
    public event EventHandler<TPropertyType> ValueChanged;

    /// <summary>
    /// Initializes a new instance of the <see cref="PropertyChangeEventSource{TPropertyType}"/> class.
    /// </summary>
    /// <param name="source">The source.</param>
    /// <param name="property">Monitor this dependency property for changes.</param>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public PropertyChangeEventSource(
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        DependencyObject source,
        DependencyProperty property)
    {
        _source = source;
        _property = property;
        _token = source.RegisterPropertyChangedCallback(property, (_, _) =>
        {
            ValueChanged?.Invoke(this, (TPropertyType)source.GetValue(property));
        });
    }

    public void Unregister()
    {
        _source.UnregisterPropertyChangedCallback(_property, _token);
    }
}
