---
title: NullableBoolExtension
author: michael-hawker
description: The NullableBoolExtension type allows developers to specify default values in XAML for nullable bool dependency properties.
keywords: windows 10, uwp, uwp community toolkit, uwp toolkit, nullable bool, dependency property, markup extension, XAML, markup 
dev_langs:
  - csharp
category: Extensions
subcategory: Media
discussion-id: 0
issue-id: 0
---

# NullableBoolExtension

The [`NullableBoolExtension`](/dotnet/api/microsoft.toolkit.uwp.ui.nullableboolextension) type provides the ability to set nullable `bool` dependency properties in XAML markup. These types of properties can normally be bound to, but can't be explicitly set to a specific value. This extension provides that capability.

> **Platform APIs:** [`NullableBoolExtension`](/dotnet/api/microsoft.toolkit.uwp.ui.nullableboolextension)

Here is an example of how this extension could be used when binding to a `DependencyProperty`:

```xaml
<Page.Resources
    xmlns:ui="using:Microsoft.Toolkit.Uwp.UI"
    xmlns:helpers="using:MyApp.Helpers">
    <helpers:ObjectWithNullableBoolProperty
        x:Key="OurObject"
        NullableBool="{ui:NullableBool Value=True}"/>
</Page.Resources>
```

With the following backend object to use as binding source:

```csharp
namespace MyApp.Helpers
{
    public class ObjectWithNullableBoolProperty : DependencyObject
    {
        // Using a DependencyProperty as the backing store for NullableBool. 
        // This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NullableBoolProperty = DependencyProperty.Register(
            nameof(NullableBool),
            typeof(bool?),
            typeof(ObjectWithNullableBoolProperty),
            new PropertyMetadata(null));

        public bool? NullableBool
        {
            get => (bool?)GetValue(NullableBoolProperty);
            set => SetValue(NullableBoolProperty, value);
        }
    }
}
```

## Examples

You can find more examples in the [unit tests](https://github.com/windows-toolkit/WindowsCommunityToolkit/tree/rel/7.1.0/UnitTests).
