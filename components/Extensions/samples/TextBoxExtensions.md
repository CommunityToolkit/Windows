---
title: TextBoxExtensions
author: Sergio0694
description: The TextBoxExtensions class provides extensions and additional functionality for the TextBox control.
keywords: TextBoxMask, XAML
dev_langs:
  - csharp
category: Extensions
subcategory: Controls
discussion-id: 0
issue-id: 0
icon: Assets/Extensions.png
---

The `TextBoxExtensions` type provides additional features for the [`TextBox`](https://learn.microsoft.com/uwp/api/windows.ui.xaml.controls.textbox) control through extension methods and attached properties.

## Text regex

The `Regex` attached property allows text validation using a regular expression or using built in validation types.

The developer adds a regular expression to validate the TextBox Text against the regular expression throw Regex property or from selecting ValidationType property on the TextBox.

The validation has 3 modes (`TextBoxExtensions.ValidationMode`):

1) `Normal` (Default): this mode will set the `IsValid` attached property to `false` or `true` whether the `TextBox` text is a valid or not against the `Regex` property.
2) `Forced`: this mode sets the `IsValid` property and removes the `TextBox` text if not valid when the `TextBox` loses focus.
3) `Dynamic`: this mode extends `Normal` and if is the newest input of the `TextBox` is invalid, the character which is invalid will be deleted. Note that the `TextBoxExtensions.ValidationType` values `Email` and `PhoneNumber` don't support this validation mode. If you set the validation mode to `Dynamic`, `Normal` is selected automatically instead.

> [!SAMPLE RegexSample]

## Text mask

The `Mask` attached property allows a user to more easily enter fixed width text in `TextBox` control where you would like them to enter the data in a certain format, ex: phone number, postal code.

The developer adds the mask property to prevent end user to enter any other format but the required one (eg. a postal code in the format "aaa-9999").

The `Mask` property provides 3 built-in variable characters that can be used to define a mask:

1) `a` which represents [a-Z]
2) `9` which represents [0-9]
3) `*` which represents `a` or `9`

At the post code example the user will only be allowed to enter from `a` to `Z` in the first 3 characters, then there is a fixed character `-` which the user can change or remove, and the last part which the user can change by entering from `0` to `9` at each character. The `Mask` property prevents the `TextBox` from having non specified characters (eg. if you entered `1` or `2` into a mask like `9999`).

The `Mask` property also supports 2 type of characters:

1) Variable: which the user can change like `a,9,*`
2) Fixed: which the user can't change and it is any non variable character (eg. the `-` in the first example)

Variable characters a represented to end user in form of placeholder so the user can know which characters he can change and which he can't, ex mask aaa-9999 will be presented to user as `___-____`. The default placeholder is `_`, but you can change it using the `MaskPlaceholder` property.

You can escape variable by using `\` (eg. the mask `+\964` will be presented to the user as `+964`). This way, the `9` in the mask is treated as a fixed character, not as a variable. If you needed `\` in the mask then you can write it as `\\` (eg. `99\\99\\9999` will be presented to the user as `__\__\__`).

In case you want to add a custom variable character you can use the `CustomMask` property. You can add a character that represents certain regex as `c:[a-c]` and once you use character `c` in the mask the mask will prevent any characters but from `a` to `c` inside the `TextBox`, also you specify multiple variable characters by adding comma `,` after every character and its representation. This feature is helpful if you want to allow certain language characters (eg. French or Arabic only `TextBox`).

> [!SAMPLE TextBoxMaskSample]

## Surface Dial support

The `SurfaceDialOptions` property adds features from the Surface Dial control to a numeric `TextBox`. This enables you to modify the content of the `TextBox` when rotating the Surface Dial (increasing or decreasing the value) and optionally go to the next focus element by tapping the Surface Dial click button. The various options are set through the `SurfaceDialOptions` type, which is declared in XAML and used to set all the values to use for a given `TextBox` from a single place.
Here is an example of the visual result when scrolling on a Surface Dial over a `TextBox`:

> [!SAMPLE SurfaceDialOptionsSample]

## Examples

You can find more examples in the [unit tests](https://github.com/windows-toolkit/WindowsCommunityToolkit/tree/rel/7.1.0/UnitTests).
