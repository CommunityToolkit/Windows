---
title: Converters
author: niels9001
description: Commonly used converters that allow the data to be modified as it passes through the binding engine.
keywords: Converters, Control, Layout
dev_langs:
  - csharp
category: Xaml
subcategory: None
discussion-id: 0
issue-id: 0
icon: Assets/Converters.png
---

| Converter | Purpose |
| --- | --- |
| [BoolNegationConverter](/dotnet/api/microsoft.toolkit.uwp.ui.converters.boolnegationconverter?view=uwp-toolkit-dotnet) | Converts a boolean to the inverse value (True to False and vice versa) |
| [BoolToObjectConverter](/dotnet/api/microsoft.toolkit.uwp.ui.converters.booltoobjectconverter?view=uwp-toolkit-dotnet) | Converts a boolean value into an object. The converted value is selected between the values of TrueValue and FalseValue properties |
| [BoolToVisibilityConverter](/dotnet/api/microsoft.toolkit.uwp.ui.converters.booltovisibilityconverter?view=uwp-toolkit-dotnet) | Converts a boolean value into a Visibility enumeration |
| [CollectionVisibilityConverter](/dotnet/api/microsoft.toolkit.uwp.ui.converters.collectionvisibilityconverter?view=uwp-toolkit-dotnet) | Converts a collection into a Visibility enumeration (Collapsed if the given collection is empty or null) |
| [DoubleToObjectConverter](/dotnet/api/microsoft.toolkit.uwp.ui.converters.doubletoobjectconverter?view=uwp-toolkit-dotnet) | Converts a double value into an object based on a value to be greater than, less than, or in-between. |
| [DoubleToVisibilityConverter](/dotnet/api/microsoft.toolkit.uwp.ui.converters.doubletovisibilityconverter?view=uwp-toolkit-dotnet) | Converts a double value into a Visibility enumeration based on a value to be greater than, less than, or in-between. |
| [EmptyCollectionToObjectConverter](/dotnet/api/microsoft.toolkit.uwp.ui.converters.emptycollectiontoobjectconverter?view=uwp-toolkit-dotnet) | Converts a collection into an object. The converted value is selected between the values of EmptyValue and NotEmptyValue properties |
| [EmptyObjectToObjectConverter](/dotnet/api/microsoft.toolkit.uwp.ui.converters.emptyobjecttoobjectconverter?view=uwp-toolkit-dotnet) | Converts a check on a null value into an object. The converted value is selected between the values of EmptyValue and NonEmptyValue properties |
| [EmptyStringToObjectConverter](/dotnet/api/microsoft.toolkit.uwp.ui.converters.emptystringtoobjectconverter?view=uwp-toolkit-dotnet) | Converts a string into an object. The converted value is selected between the values of EmptyValue and NotEmptyValue properties |
| [FormatStringConverter](/dotnet/api/microsoft.toolkit.uwp.ui.converters.formatstringconverter?view=uwp-toolkit-dotnet) | Converts an [IFormattable](/dotnet/api/system.string.format?view=netframework-4.7) value into a string. The ConverterParameter provides the string format |
| [ResourceNameToResourceStringConverter](/dotnet/api/microsoft.toolkit.uwp.ui.converters.resourcenametoresourcestringconverter?view=uwp-toolkit-dotnet) | Converter to look up the source string in the App Resources strings and returns its value, if found |
| [StringFormatConverter](/dotnet/api/microsoft.toolkit.uwp.ui.converters.stringformatconverter?view=uwp-toolkit-dotnet) | Converts a source object to the formatted string version using [string.Format](/dotnet/api/system.string.format?view=netframework-4.7). The ConverterParameter provides the string format |
| [StringVisibilityConverter](/dotnet/api/microsoft.toolkit.uwp.ui.converters.stringvisibilityconverter?view=uwp-toolkit-dotnet) | Converts a string value into a Visibility enumeration (if the value is null or empty returns a collapsed value) |
| [ToolbarFormatActiveConverter](/dotnet/api/microsoft.toolkit.uwp.ui.converters.toolbarformatactiveconverter?view=uwp-toolkit-dotnet) | Compares if Formats are equal and returns bool |
| [VisibilityToBoolConverter](/dotnet/api/microsoft.toolkit.uwp.ui.converters.visibilitytoboolconverter?view=uwp-toolkit-dotnet) | This class converts a Visibility enumeration to a boolean value |

## BoolToObjectConverter Examples

`BoolToObjectConverter` can be used to generalize the behavior of `BoolToVisibilityConverter` by allowing to pass the two values it can return.
You can use it to switch Visibility by declaring it :

```xaml
<Page ...
     xmlns:converters="using:Microsoft.Toolkit.Uwp.UI.Converters"/>

<Page.Resources>
    <converters:BoolToObjectConverter x:Key="BoolToVisibilityConverter" TrueValue="Visible" FalseValue="Collapsed"/>
</Page.Resources>
```

and using it like that :

```xaml
<Image Visibility="{x:Bind Path=MyBoolValue, Converter={StaticResource BoolToVisibilityConverter}}" />
```

It can also be used to switch between two values of brush.

Note : you can use a resource for the brush or pass the color string and have it converted to a brush automatically.

```xaml
<Page.Resources>
    <converters:BoolToObjectConverter x:Key="BoolToBrushConverter" TrueValue="Green" FalseValue="{StaticResource NopeBrush}" />
</Page.Resources>
```

and using it like that :

```xaml
<Border Background="{x:Bind Path=MyBoolValue, Converter={StaticResource BoolToBrushConverter}}" />
```

An other example is to switch between two images by specifying their source :

```xaml
<Page.Resources>
    <converters:BoolToObjectConverter x:Key="BoolToImageConverter" TrueValue="ms-appx:///Assets/Yes.png" FalseValue="ms-appx:///Assets/No.png" />
</Page.Resources>
```

and using it like that :

```xaml
<Image Source="{x:Bind Path=MyBoolValue, Converter={StaticResource BoolToImageConverter}}" />
```

## BoolToVisibilityConverter Examples

`BoolToVisibilityConverter` can be used to easily change a boolean value to a Visibility based one.
If targeting 14393 or later, this is done automatically through [x:Bind](/windows/uwp/xaml-platform/x-bind-markup-extension).  First, declare the converter in your resources:

```xaml
<Page.Resources>
    <converters:BoolToVisibilityConverter x:Key="BoolToVisibilityConverter"/>
</Page.Resources>
```

and use it like this :

```xaml
<Image Visibility="{Binding Path=MyBoolValue, Converter={StaticResource BoolToVisibilityConverter}}" />
```

you can also invert the boolean as a ConverterParameter:

```xaml
<Image Visibility="{Binding Path=MyBoolValue, Converter={StaticResource BoolToVisibilityConverter}, ConverterParameter=True}" />
```

or if you want to not pass a parameter, you can use `BoolToObjectConverter` to create an `InverseBoolToVisibilityConveter`:

```xaml
<Page.Resources>
    <converters:BoolToObjectConverter x:Key="InverseBoolToVisibilityConverter" TrueValue="Collapsed" FalseValue="Visible"/>
</Page.Resources>
```

## DoubleToVisibilityConverter Examples

`DoubleToVisibilityConverter` can be used to easily change a double value to a Visibility based one based on a given threshold value.  If both `GreaterThan` and `LessThan` are set, the converter will set the visibility if the target value is in-between those two values.  Otherwise, it will look for the target being greater than or less than the specified value.

If a `True` value is provided for a `ConverterParameter` than the equation will be inverted.

```xaml
<Page.Resources>
    <converters:DoubleToVisibilityConverter x:Key="GreaterThanToleranceVisibilityConverter" GreaterThan="65.0"/>
</Page.Resources>
```

and use it like this :

```xaml
<Button x:Name="ScrollBackButton"
        Visibility="{Binding ScrollableWidth, Converter={StaticResource GreaterThanToleranceVisibilityConverter}, ElementName=ScrollViewer, FallbackValue=Collapsed, TargetNullValue=Collapsed}"/>
```

## EmptyObjectToObjectConverter Examples

`EmptyObjectToObjectConverter`, `EmptyCollectionToObjectConverter`, and `EmptyStringToObjectConverter` work similarly to the `BoolToObjectConverter` except using `EmptyValue` and `NotEmptyValue` instead of `TrueValue`/`FalseValue`.

They inspect the type of 'empty'/null object value and return the specific value `EmptyValue` or `NotEmptyValue` based on the result.
That result can also be inverted withe a ConverterParameter.

For instance you can generalize the `CollectionVisibilityConverter` using the `EmptyCollectionToObjectConverter`:

```xaml
<Page.Resources>
    <converters:EmptyCollectionToObjectConverter x:Key="CollectionVisibilityConverter" EmptyValue="Collapsed" NotEmptyValue="Visible"/>
</Page.Resources>
```

this can be used as follows to hide a list with no items and instead show text through inversion with the ConverterParameter:

```xaml
<ListView Visibility="{Binding Path=MyCollectionValue, Converter={StaticResource CollectionVisibilityConverter}}" />

<TextBlock Text="No Items." Visibility="{Binding Path=MyCollectionValue, Converter={StaticResource CollectionVisibilityConverter}, ConverterParameter=True}">
```

## StringFormatConverter Examples

`StringFormatConverter` allows you to format a string property upon binding wrapping [string.Format](/dotnet/api/system.string.format?view=netstandard-2.0).  
It only allows for a single input value (the binding string), but can be formatted with the regular string.Format
methods.  First, add it to your page resources:

> [!Sample StringFormatConverterSample]
