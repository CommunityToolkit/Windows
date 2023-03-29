---
title: EnumValuesExtensions
author: Sergio0694
description: A markup extension that returns a collection of values of a specific enum type.
keywords: windows 10, uwp, uwp community toolkit, uwp toolkit, markup extension, XAML, markup, enum
dev_langs:
  - csharp
category: Extensions
subcategory: Markup
discussion-id: 0
issue-id: 0
---

# EnumValuesExtensions

The [`EnumValuesExtensions`](/dotnet/api/microsoft.toolkit.uwp.ui.EnumValuesExtensions) type implements a markup extension that returns a collection of values of a specific enum type. It can be useful to easily bind a collection of all possible values from a given enum type to a UI element such as a [`ComboBox`](/windows/uwp/design/controls-and-patterns/combo-box) or some other items container or selector control.

> **Platform APIs:** [`EnumValuesExtensions`](/dotnet/api/microsoft.toolkit.uwp.ui.EnumValuesExtensions)

## Syntax

Assuming we had an `Animal` enum type and we wanted the user to pick one of the available names, here is the XAML syntax that allows us to create a `ComboBox` and display all `Animal` values, directly from XAML and with no code-behind:

```xaml
<ComboBox
    xmlns:ui="using:Microsoft.Toolkit.Uwp.UI"
    xmlns:enums="using:MyApplication.Enums"
    ItemsSource="{ui:EnumValues Type=enums:Animal}"
    SelectedIndex="0"/>
```

In this example we're just relying on the default `ComboBox` item template, that will display the name of each `Animal` value in a [`TextBlock`](/uwp/api/windows.ui.xaml.controls.textblock) control. We could of course also define a custom item template if we wanted to show additional info for each individual `Animal` value, or if we wanted to further customize how each value is presented to the user.

## Examples

Binding to an enum property can be accomplished like so:

```xaml
<ComboBox
    xmlns:ui="using:Microsoft.Toolkit.Uwp.UI"
    xmlns:enums="using:MyApplication.Enums"
    ItemsSource="{ui:EnumValues Type=enums:Animal}"
    SelectedItem="{x:Bind SelectedAnimal, Mode=OneWay}" />
```

```csharp
private Animal selectedAnimal = Animal.Dog;

public Animal SelectedAnimal
{
    get => selectedAnimal;
    set
    {
        selectedAnimal = value;
        OnPropertyChanged(nameof(SelectedAnimal));
    }
}
```

You can find more examples in the [unit tests](https://github.com/windows-toolkit/WindowsCommunityToolkit/tree/rel/7.1.0/UnitTests).
