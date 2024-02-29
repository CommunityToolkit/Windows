---
title: Converters
author: niels9001
description: Commonly used converters that allow the data to be modified as it passes through the binding engine.
keywords: Converters, Control, Layout
dev_langs:
  - csharp
category: Xaml
subcategory: Miscellaneous
discussion-id: 0
issue-id: 0
icon: Assets/Converters.png
---

Converters are an easy way to handle situations where the source and target properties are of a different type. These are often declared in the `.Resources` of your app or page and can be referenced in XAML. In some cases, default converter values can be inverted by setting `ConverterParameter=True`.

## BoolNegationConverter

Converts a boolean to the inverse value (True to False and vice versa)

> [!Sample BoolNegationConverterSample]

## BoolToObjectConverter

Converts a boolean value into an other object. Can be used to convert true/false to e.g. visibility, color, or different images.

> [!Sample BoolToObjectConverterSample]

## BoolToVisibilityConverter

 Converts a boolean value into a `Visibility` enumeration. The `ConverterParameter` can be used to invert the logic.

> [!Sample BoolToVisibilityConverterSample]

## ColorToDisplayNameConverter

Converts a color to the approximated display name.

> [!Sample ColorToDisplayNameConverterSample]

## DoubleToObjectConverter

Converts a double value into an other object. Can be used to convert doubles to e.g. visibility, colors , or different images. If `GreaterThan` and `LessThan` are both set, the logic looks for a value between the two values. Otherwise the logic looks for the value to be `GreaterThan` or `LessThan` the specified value.

> [!Sample DoubleToObjectConverterSample]

## DoubleToVisibilityConverter

Converts a double value into an other object. Can be used to convert doubles to e.g. visibility, colors , or different images. If `GreaterThan` and `LessThan` are both set, the logic looks for a value between the two values. Otherwise the logic looks for the value to be `GreaterThan` or `LessThan` the specified value.

> [!Sample DoubleToVisibilityConverterSample]

## EmptyCollectionToObjectConverter & CollectionVisibilityConverter

Converts a collection size into an other object. Can be used to convert to bind a visibility, a color or an image to the size of the collection. The `CollectionVisibilityConverter` is derived from it and can be easily used to convert a collection into a `Visibility` enumeration (Collapsed if the given collection is empty or null).
Note: this converter only checks the initial state of a collection and does not detect any updates. For that scenario, using a `IsNullOrEmptyStateTrigger` is more appropriate.

> [!Sample CollectionVisibilityConverterSample]

## EmptyObjectToObjectConverter, EmptyStringToObjectConverter & StringToVisibilityConverter

Converts an object value into a an object (if the value is null returns the false value). Can be used to bind a visibility, a color or an image to the value of an object.
The `EmptyStringToObjectConverter` converts a string value into a an object (if the value is null or empty returns the false value). Can be used to bind a visibility, a color or an image to the value of a string.

> [!Sample EmptyStringToObjectConverterSample]

## FileSizeToFriendlyStringConverter

Converts a file size in bytes to a more human-readable friendly format using `ToFileSizeString(Int64)`.

> [!Sample FileSizeToFriendlyStringConverterSample]

## ResourceNameToResourceStringConverter

Converts a source string from the App resources and returns its value, if found.

## StringFormatConverter

This allows you to format a string property upon binding wrapping [string.Format](https://learn.microsoft.com/dotnet/api/system.string.format?view=netstandard-2.0).  
It only allows for a single input value (the binding string), but can be formatted with the regular string.Format
methods.

> [!Sample StringFormatConverterSample]

## StringVisibilityConverter

Converts a string value into a Visibility value (if the value is null or empty returns a collapsed value).

> [!Sample StringVisibilityConverterSample]

## TaskResultConverter

Converter that can be used to safely retrieve results from `Task<TResult>` instances. See the [MVVM Toolkit Gallery](https://aka.ms/mvvmtoolkit/app) for an example.

## TypeToObjectConverter

Returns an object or another, depending on whether the type of the provided value matches another provided Type.
> [!Sample TypeToObjectConverterSample]

## VisibilityToBoolConverter

Converts a `Visibility` enumeration to a boolean value.

> [!Sample VisibilityToBoolConverterSample]
