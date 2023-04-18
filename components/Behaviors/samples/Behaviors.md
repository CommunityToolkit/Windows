---
title: Behaviors
author: Arlodotexe
description: 
keywords: Behaviors
dev_langs:
  - csharp
category: Xaml
subcategory: Behaviors
discussion-id: 0
issue-id: 0
---

# Behaviors

A behavior is a class that attaches to a XAML control and invokes an Action when triggered. 

The `Microsoft.Xaml.Behaviors.*` packages contains several useful triggers and actions, and the Windows Community Toolkit provides even more.

See also [XamlBehaviors Wiki](https://github.com/Microsoft/XamlBehaviors/wiki)

## KeyDownTriggerBehavior

A behavior that listens to a key press event on the associated UIElement and triggers the set of actions.

> [!Sample KeyDownTriggerBehaviorSample]

## AutoSelectBehavior

The AutoSelectBehavior automatically selects the entire content of its associated TextBox when it is loaded.

> [!Sample AutoSelectBehaviorSample]

## ViewportBehavior
This behavior allows you to listen an element enter or exit the ScrollViewer viewport.

> [!Sample ViewportBehaviorSample]

## FocusBehavior

Of the given targets, this behavior sets the focus on the first control which accepts it.

A control only receives focus if it is enabled and loaded into the visual tree:
> [!Sample FocusBehaviorButtonSample]

Empty lists do not receive focus:
> [!Sample FocusBehaviorListSample]
