---
title: SwitchPresenter
author: michael-hawker
description: A XAML ContentPresenter which can act like a switch statement for showing different UI based on a condition.
keywords: switch, logic, switchpresenter, contentpresenter, visibility, triggers, converters
dev_langs:
  - csharp
category: Layouts
subcategory: Miscellaneous
discussion-id: 0
issue-id: 0
icon: Assets/SwitchPresenter.png
---

The `SwitchPresenter` control acts like a switch statement for XAML. It allows a developer to display certain content based on the condition of another value as an alternative to managing multiple `Visibility` values or complex visual states.

Unlike traditional approaches of showing/hiding components within a page, the `SwitchPresenter` will only load and attach the matching Case's content to the Visual Tree.

## Examples

SwitchPresenter can make it easier to follow complex layout changes or layouts with varying logic that are based on a property, for instance the type of ticketing mode for an airline:

> [!SAMPLE SwitchPresenterLayoutSample]

Or it can simply be used to clearly display different outcomes based on some state which can be useful for a `NavigationView` or with a simple enum as in the following example:

> [!SAMPLE SwitchPresenterValueSample]

`SwitchPresenter` can also be used as a replacement for the deprecated `Loading` control. This provides more fine-grained control over animations and content within each state:

> [!SAMPLE SwitchPresenterLoaderSample]

We can also invert the paradigm a bit with a `SwitchPresenter` to do data transformations within XAML using a `ContentTemplate`. Imagine an alternate view of our first starting example:

> [!SAMPLE SwitchPresenterTemplateSample]

That's right! `SwitchPresenter` can be used not just for displaying different UIElements but in feeding different kinds of data into the `ContentTemplate` as well.

## SwitchConverter

A new analog to `SwitchPresenter` is the `SwitchConverter` which can be used in bindings to translate values into resources:

> [!SAMPLE SwitchConverterBrushSample]
