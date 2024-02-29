---
title: EnumValuesExtensions
author: Sergio0694
description: A markup extension that returns a collection of values of a specific enum type.
keywords: XAML, markup, enum
dev_langs:
  - csharp
category: Extensions
subcategory: Markup
discussion-id: 0
issue-id: 0
icon: Assets/Extensions.png
---

The `EnumValuesExtensions` type implements a markup extension that returns a collection of values of a specific enum type. It can be useful to easily bind a collection of all possible values from a given enum type to a UI element such as a [`ComboBox`](https://learn.microsoft.com/windows/uwp/design/controls-and-patterns/combo-box) or some other items container or selector control.

> **Platform APIs:** `EnumValuesExtensions`

## Syntax

Assuming we had an `Animal` enum type and we wanted the user to pick one of the available names, here is the XAML syntax that allows us to create a `ComboBox` and display all `Animal` values, directly from XAML and with no code-behind:

```xaml
<ComboBox
    xmlns:ui="using:CommunityToolkit.WinUI"
    xmlns:enums="using:MyApplication.Enums"
    ItemsSource="{ui:EnumValues Type=enums:Animal}"
    SelectedIndex="0"/>
```

In this example we're just relying on the default `ComboBox` item template, that will display the name of each `Animal` value in a [`TextBlock`](https://learn.microsoft.com/uwp/api/windows.ui.xaml.controls.textblock) control. We could of course also define a custom item template if we wanted to show additional info for each individual `Animal` value, or if we wanted to further customize how each value is presented to the user.

## Examples

Binding to an enum property can be accomplished like so:

```xaml
<ComboBox
    xmlns:ui="using:CommunityToolkit.WinUI"
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
