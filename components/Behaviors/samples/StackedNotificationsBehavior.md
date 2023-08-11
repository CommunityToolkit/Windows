---
title: StackedNotificationsBehavior
author: vgromfeld
description: A behavior to add stacked notifications to a WinUI InfoBar control.
keywords: StackedNotificationsBehavior, Control, Layout, InfoBar, Behavior
dev_langs:
  - csharp
category: Xaml
subcategory: Behaviors
discussion-id: 0
issue-id: 0
icon: Assets/StackedNotificationsBehavior.png
---

The `StackedNotificationsBehavior` allows you to provide notifications within your app using an `InfoBar` control. This is a replacement for the prior `InAppNotification` control in the Toolkit.

With the default settings, a notification will be displayed until it is dismissed by the user. Any subsequent notifications will be displayed
in the order of being sent afterwards one-by-one.

## Example

Clicking on the button multiple times will queue up multiple messages to be displayed one after another.

> [!Sample StackedNotificationsBehaviorCustomSample]

## Notification options

By default, the properties provided on the attached `InfoBar` will be used, like `ContentTemplate` or `IsIconVisible`.

However, there are a number of options available on the `Notification` class to override these. When set, these will override any defaults
or modified properties set on the parent `InfoBar` itself. They will be restored to the previously set value on the `InfoBar` after the message has been displayed.

> [!WARNING]
> Properties set on the `InfoBar` will be modified directly by the behavior with notification overrides, this means any bindings will
> be broken by that change when it is overridden or restored by the notification. Therefore, it is best to only provide constants on the
> parent `InfoBar` itself that will be consistent for all messages and set any dynamic options in the `Notification` options.

When a `Duration` is provided, if the user has their pointer over the message, it will not be dismissed. It will instead reset the time before
being dismissed once the pointer has left the active notification.

## Migrating from InAppNotification

If you previously used the `InAppNotification` component from the Windows Community Toolkit, like so:

```xml
<controls:InAppNotification x:Name="ExampleInAppNotification"/>
```

You can simply replace it with an `InfoBar` control and add the attached behavior:

```xml
<muxc:InfoBar>
  <interactivity:Interaction.Behaviors>
    <behaviors:StackedNotificationsBehavior x:Name="ExampleInAppNotification" />
  </interactivity:Interaction.Behaviors>
</muxc:InfoBar>
```

There are some changes to the `Show` method, however a simple text based one has been provided for backwards compatibility,
otherwise it's best to construct your own `Notification` object for greater flexibility or set common properties on the
parent `InfoBar` itself.

> [!NOTE]
> There is no `StackMode` property to control the behavior of the queue. Providing a stable queue of messages one after another
> provides the best user experience as it reduces the risk when interacting with a notification for a new one to suddenly appear
> and replace the one being displayed.

The `ShowDismissButton` property should be mapped to the `InfoBar.IsClosable` property instead. Similar to any adjustments to position
should be handled by the layout of the `InfoBar` control itself within the XAML layout.

The `Closing` and `Closed` events can be mapped to those on the `InfoBar` as well.

### Complete example

This example shows sending simple text based notifications that will appear only for 2 seconds:

> [!Sample StackedNotificationsBehaviorToolkitSample]
