---
title: DispatcherQueueTimerExtensions
author: michael-hawker
description: Helpers for executing code at specific times on a UI thread through a DispatcherQueue instance with a DispatcherQueueTimer.
keywords: dispatcher, dispatcherqueue, DispatcherHelper, DispatcherQueueExtensions, DispatcherQueueTimer, DispatcherQueueTimerExtensions
dev_langs:
  - csharp
category: Extensions
subcategory: Miscellaneous
discussion-id: 0
issue-id: 0
icon: Assets/Extensions.png
---

The `DispatcherQueueTimerExtensions` static class provides a collection of extensions methods for [`DispatcherQueueTimer`](https://learn.microsoft.com/windows/windows-app-sdk/api/winrt/microsoft.ui.dispatching.dispatcherqueue) objects that make it easier to execute code on a specific UI thread at a specific time.

## Syntax

The `DispatcherQueueTimerExtensions` static class currently exposes a single extension method, `Debounce`. This is a standard technique used to rate-limit input from a user to not overload requests on an underlying service of query elsewhere.

It can be used in a number of ways, but most simply like so:

> [!SAMPLE KeyboardDebounceSample]

## Examples

You can find more examples in the [unit tests](https://github.com/CommunityToolkit/Windows/blob/rel/8.1.240916/components/Extensions/tests/DispatcherQueueTimerExtensionTests.cs).
