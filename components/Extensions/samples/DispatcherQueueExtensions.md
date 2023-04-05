---
title: DispatcherQueueExtensions
author: Sergio0694
description: Helpers for executing code on a specific UI thread through a DispatcherQueue instance.
keywords: windows 10, uwp, windows community toolkit, uwp community toolkit, uwp toolkit, extensions, winui3, xaml islands, dispatcher, dispatcherqueue, DispatcherHelper, DispatcherQueueExtensions
dev_langs:
  - csharp
category: Extensions
subcategory: None
discussion-id: 0
issue-id: 0
---

# DispatcherQueueExtensions

The [`DispatcherQueueExtensions`](/dotnet/api/microsoft.toolkit.uwp.DispatcherQueueExtensions) type provides a collection of extensions methods for [`DispatcherQueue`](/uwp/api/windows.system.dispatcherqueue) objects that makes it easier to execute code on a specific UI thread. A `DispatcherQueue` instance can be retrieved and cached for later use, and then used through any of the available helper methods to dispatch a delegate invocation on it.

> **Platform APIs:** [`DispatcherQueueExtensions`](/dotnet/api/microsoft.toolkit.uwp.DispatcherQueueExtensions)

## Syntax

The `DispatcherQueueExtensions` type exposes a number of overloads of its `EnqueueAsync` method to dispatch either synchronous or asynchronous delegates, and to optionally have them return a value that is then relayed back to the caller through an awaitable task. Here are some examples of how these extension methods can be used:

```csharp
// Get a DispatcherQueue instance for later use. This has to be called on the UI thread,
// but it can then be cached for later use and accessed from a background thread as well.
DispatcherQueue dispatcherQueue = DispatcherQueue.GetForCurrentThread();

// Execute some code on the target dispatcher queue
await dispatcherQueue.EnqueueAsync(() =>
{
});

// Execute some code that also returns a value
int someValue = await dispatcherQueue.EnqueueAsync(() =>
{
    return 42;
});

// Execute some asynchronous code
await dispatcherQueue.EnqueueAsync(async () =>
{
    await Task.Delay(100);
});

// Execute some asynchronous code that also returns a value
int someOtherValue = await dispatcherQueue.EnqueueAsync(async () =>
{
    await Task.Delay(100);

    return 42;
});
```

## Migrating from [`DispatcherHelper`](..\helpers\DispatcherHelper.md)

The [`CoreDispatcher`](/uwp/api/windows.ui.core.coredispatcher) is being deprecated (and it will no longer work with XAML Islands or WinUI 3) and should no longer be used, as it had a number of limitations. Specifically, it relied on the assumption that each window had its own UI thread tied to it, which is not always the case. The new `DispatcherQueue` instead can be used going forwards, and it requires some changes in code that was previously relying on `CoreDispatcher` and `DispatcherHelper`. Specifically, a background thread can no longer retrieve the `CoreDispatcher` by just accessing the dispatcher associated to the "main window" for the application, because this concept does not apply anymore. Instead, a `DispatcherQueue` instance needs to be retrieved on the UI thread and cached for later use in a background thread.

If you were using `DispatcherHelper` on the UI thread, apply the following change (here we're using `Task.Run` to simulate some work being done in a background thread and accessing some UI component, and we're assuming for this example that there is a `TextBlock` control in our page called "MyTextBlock"):

```csharp
// Before
Task.Run(() =>
{
    await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
    {
        MyTextBlock.Text = "Hello from a background thread!";
    });
});

// After
DispatcherQueue dispatcherQueue = DispatcherQueue.GetForCurrentThread();

Task.Run(() =>
{
    await dispatcherQueue.EnqueueAsync(() =>
    {
        MyTextBlock.Text = "Hello from a background thread!";
    });
});
```

## Examples

You can find more examples in the [unit tests](https://github.com/windows-toolkit/WindowsCommunityToolkit/blob/rel/7.1.0/UnitTests/UnitTests.UWP/Extensions/Test_DispatcherQueueExtensions.cs).
