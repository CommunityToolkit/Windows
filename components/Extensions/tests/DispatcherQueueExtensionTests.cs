// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.Tests;
using CommunityToolkit.Tooling.TestGen;
using CommunityToolkit.WinUI;
using System.Threading.Tasks;

#if WINAPPSDK
using DispatcherQueue = Microsoft.UI.Dispatching.DispatcherQueue;
using DispatcherQueuePriority = Microsoft.UI.Dispatching.DispatcherQueuePriority;
#else
using Windows.Foundation.Metadata;
using DispatcherQueue = Windows.System.DispatcherQueue;
using DispatcherQueuePriority = Windows.System.DispatcherQueuePriority;
#endif

namespace ExtensionsTests;

[TestClass]
public partial class DispatcherQueueExtensionTests : VisualUITestBase
{
    private const int TIME_OUT = 5000;

    [TestCategory("DispatcherQueueExtensions")]
    [UIThreadTestMethod]
    public async Task DispatcherQueueHelper_Action_Exception()
    {
        var task = DispatcherQueue.GetForCurrentThread().EnqueueAsync(() =>
        {
            throw new ArgumentException(nameof(this.DispatcherQueueHelper_Action_Exception));
        });

        Assert.IsNotNull(task);

        try
        {
            await task;
        }
        catch { }

        Assert.AreEqual(TaskStatus.Faulted, task.Status);
        Assert.IsNotNull(task.Exception);
        Assert.IsInstanceOfType(task.Exception.InnerExceptions.FirstOrDefault(), typeof(ArgumentException));
    }

    [TestCategory("DispatcherQueueExtensions")]
    [UIThreadTestMethod]
    public async Task DispatcherQueueHelper_Action_Null()
    {
        var task = DispatcherQueue.GetForCurrentThread().EnqueueAsync(default(Action)!);

        Assert.IsNotNull(task);

        try
        {
            await task;
        }
        catch { }

        Assert.AreEqual(TaskStatus.Faulted, task.Status);
        Assert.IsNotNull(task.Exception);
        Assert.IsInstanceOfType(task.Exception.InnerExceptions.FirstOrDefault(), typeof(NullReferenceException));
    }

    [TestCategory("DispatcherQueueExtensions")]
    [UIThreadTestMethod]
    public async Task DispatcherQueueHelper_Action_Ok_UIThread()
    {
        await DispatcherQueue.GetForCurrentThread().EnqueueAsync(() =>
        {
            var textBlock = new TextBlock { Text = nameof(DispatcherQueueHelper_Action_Ok_UIThread) };

            Assert.AreEqual(textBlock.Text, nameof(DispatcherQueueHelper_Action_Ok_UIThread));
        });
    }

    [TestCategory("DispatcherQueueExtensions")]
    [UIThreadTestMethod]
    public async Task DispatcherQueueHelper_Action_Ok_From_NonUIThread()
    {
        var taskSource = new TaskCompletionSource<object?>();
        try
        {
            var dispatcherQueue = DispatcherQueue.GetForCurrentThread();
            await Task.Run(async () =>
            {
                await dispatcherQueue.EnqueueAsync(() =>
                {
                    var textBlock = new TextBlock { Text = nameof(DispatcherQueueHelper_Action_Ok_From_NonUIThread) };

                    Assert.AreEqual(textBlock.Text, nameof(DispatcherQueueHelper_Action_Ok_From_NonUIThread));

                    taskSource.SetResult(null);
                });
            });
        }
        catch (Exception e)
        {
            taskSource.SetException(e);
        }
        await taskSource.Task;

        Assert.AreEqual(TaskStatus.RanToCompletion, taskSource.Task.Status);
        Assert.IsNull(taskSource.Task.Exception);
    }

    [TestCategory("DispatcherQueueExtensions")]
    [UIThreadTestMethod]
    public async Task DispatcherQueueHelper_FuncOfT_Null()
    {
        var task = DispatcherQueue.GetForCurrentThread().EnqueueAsync(default(Func<int>)!);

        Assert.IsNotNull(task);

        try
        {
            await task;
        }
        catch { }

        Assert.AreEqual(TaskStatus.Faulted, task.Status);
        Assert.IsNotNull(task.Exception);
        Assert.IsInstanceOfType(task.Exception.InnerExceptions.FirstOrDefault(), typeof(NullReferenceException));
    }

    [TestCategory("DispatcherQueueExtensions")]
    [UIThreadTestMethod]
    public async Task DispatcherQueueHelper_FuncOfT_Ok_UIThread()
    {
        var textBlock = await DispatcherQueue.GetForCurrentThread().EnqueueAsync(() =>
        {
            return new TextBlock { Text = nameof(DispatcherQueueHelper_FuncOfT_Ok_UIThread) };
        });

        Assert.AreEqual(textBlock.Text, nameof(DispatcherQueueHelper_FuncOfT_Ok_UIThread));
    }

    [TestCategory("DispatcherQueueExtensions")]
    [UIThreadTestMethod]
    public async Task DispatcherQueueHelper_FuncOfT_Ok_From_NonUIThread()
    {
        var taskSource = new TaskCompletionSource<object?>();
        try
        {
            var dispatcherQueue = DispatcherQueue.GetForCurrentThread();
            await Task.Run(async () =>
            {
                var textBlock = await dispatcherQueue.EnqueueAsync(() =>
                {
                    return new TextBlock { Text = nameof(DispatcherQueueHelper_FuncOfT_Ok_From_NonUIThread) };
                });
                await dispatcherQueue.EnqueueAsync(() =>
                {
                    Assert.AreEqual(textBlock.Text, nameof(DispatcherQueueHelper_FuncOfT_Ok_From_NonUIThread));
                    taskSource.SetResult(null);
                });
            });
        }
        catch (Exception e)
        {
            taskSource.SetException(e);
        }
        await taskSource.Task;

        Assert.AreEqual(TaskStatus.RanToCompletion, taskSource.Task.Status);
        Assert.IsNull(taskSource.Task.Exception);
    }

    [TestCategory("DispatcherQueueExtensions")]
    [UIThreadTestMethod]
    public async Task DispatcherQueueHelper_FuncOfT_Exception()
    {
        var task = DispatcherQueue.GetForCurrentThread().EnqueueAsync(new Func<int>(() =>
        {
            throw new ArgumentException(nameof(this.DispatcherQueueHelper_FuncOfT_Exception));
        }));

        Assert.IsNotNull(task);

        try
        {
            await task;
        }
        catch { }

        Assert.AreEqual(TaskStatus.Faulted, task.Status);
        Assert.IsNotNull(task.Exception);
        Assert.IsInstanceOfType(task.Exception.InnerExceptions.FirstOrDefault(), typeof(ArgumentException));
    }

    [TestCategory("DispatcherQueueExtensions")]
    [UIThreadTestMethod]
    public async Task DispatcherQueueHelper_FuncOfTask_Null()
    {
        var task = DispatcherQueue.GetForCurrentThread().EnqueueAsync(default(Func<Task>)!);

        Assert.IsNotNull(task);

        try
        {
            await task;
        }
        catch { }

        Assert.AreEqual(TaskStatus.Faulted, task.Status);
        Assert.IsNotNull(task.Exception);
        Assert.IsInstanceOfType(task.Exception.InnerExceptions.FirstOrDefault(), typeof(NullReferenceException));
    }

    [TestCategory("Helpers")]
    [UIThreadTestMethod]
    public async Task DispatcherQueueHelper_FuncOfTask_Ok_UIThread()
    {
        await DispatcherQueue.GetForCurrentThread().EnqueueAsync(() =>
        {
            var textBlock = new TextBlock { Text = nameof(DispatcherQueueHelper_FuncOfTask_Ok_UIThread) };

            Assert.AreEqual(textBlock.Text, nameof(DispatcherQueueHelper_FuncOfTask_Ok_UIThread));

            return Task.CompletedTask;
        });
    }

    [TestCategory("DispatcherQueueExtensions")]
    [UIThreadTestMethod]
    public async Task DispatcherQueueHelper_FuncOfTask_Ok_From_NonUIThread()
    {
        var taskSource = new TaskCompletionSource<object?>();
        try
        {
            await DispatcherQueue.GetForCurrentThread().EnqueueAsync(async () =>
            {
                await Task.Yield();

                var textBlock = new TextBlock { Text = nameof(DispatcherQueueHelper_FuncOfTask_Ok_From_NonUIThread) };

                Assert.AreEqual(textBlock.Text, nameof(DispatcherQueueHelper_FuncOfTask_Ok_From_NonUIThread));

                taskSource.SetResult(null);
            });
        }
        catch (Exception e)
        {
            taskSource.SetException(e);
        }
        await taskSource.Task;

        Assert.AreEqual(TaskStatus.RanToCompletion, taskSource.Task.Status);
        Assert.IsNull(taskSource.Task.Exception);
    }

    [TestCategory("DispatcherQueueExtensions")]
    [UIThreadTestMethod]
    public async Task DispatcherQueueHelper_FuncOfTask_Exception()
    {
        var task = DispatcherQueue.GetForCurrentThread().EnqueueAsync(new Func<Task>(() =>
        {
            throw new ArgumentException(nameof(this.DispatcherQueueHelper_FuncOfTask_Exception));
        }));

        Assert.IsNotNull(task);

        try
        {
            await task;
        }
        catch { }

        Assert.AreEqual(TaskStatus.Faulted, task.Status);
        Assert.IsNotNull(task.Exception);
        Assert.IsInstanceOfType(task.Exception.InnerExceptions.FirstOrDefault(), typeof(ArgumentException));
    }

    [TestCategory("DispatcherQueueExtensions")]
    [UIThreadTestMethod]
    public async Task DispatcherQueueHelper_FuncOfTaskOfT_Null()
    {
        var task = DispatcherQueue.GetForCurrentThread().EnqueueAsync(default(Func<Task<int>>)!);

        Assert.IsNotNull(task);

        try
        {
            await task;
        }
        catch { }

        Assert.AreEqual(TaskStatus.Faulted, task.Status);
        Assert.IsNotNull(task.Exception);
        Assert.IsInstanceOfType(task.Exception.InnerExceptions.FirstOrDefault(), typeof(NullReferenceException));
    }

    [TestCategory("DispatcherQueueExtensions")]
    [UIThreadTestMethod]
    public async Task DispatcherQueueHelper_FuncOfTaskOfT_Ok_UIThread()
    {
        await DispatcherQueue.GetForCurrentThread().EnqueueAsync(() =>
        {
            var textBlock = new TextBlock { Text = nameof(DispatcherQueueHelper_FuncOfTaskOfT_Ok_UIThread) };

            Assert.AreEqual(textBlock.Text, nameof(DispatcherQueueHelper_FuncOfTaskOfT_Ok_UIThread));

            return Task.CompletedTask;
        });
    }

    [TestCategory("DispatcherQueueExtensions")]
    [UIThreadTestMethod]
    public async Task DispatcherQueueHelper_FuncOfTaskOfT_Ok_From_NonUIThread()
    {
        var taskSource = new TaskCompletionSource<object?>();
        try
        {
            await DispatcherQueue.GetForCurrentThread().EnqueueAsync(async () =>
            {
                await Task.Yield();

                var textBlock = new TextBlock { Text = nameof(DispatcherQueueHelper_FuncOfTaskOfT_Ok_From_NonUIThread) };

                Assert.AreEqual(textBlock.Text, nameof(DispatcherQueueHelper_FuncOfTaskOfT_Ok_From_NonUIThread));

                taskSource.SetResult(null);

                return textBlock;
            });
        }
        catch (Exception e)
        {
            taskSource.SetException(e);
        }
        await taskSource.Task;

        Assert.AreEqual(TaskStatus.RanToCompletion, taskSource.Task.Status);
        Assert.IsNull(taskSource.Task.Exception);
    }

    [Ignore]
    [TestCategory("DispatcherQueueExtensions")]
    ////[UIThreadTestMethod] - TODO: https://github.com/CommunityToolkit/Tooling-Windows-Submodule/issues/121
    public async Task DispatcherQueueHelper_FuncOfTaskOfT_Exception()
    {
        var task = DispatcherQueue.GetForCurrentThread().EnqueueAsync(new Func<Task<int>>(() =>
        {
            throw new ArgumentException(nameof(this.DispatcherQueueHelper_FuncOfTaskOfT_Exception));
        }));

        Assert.IsNotNull(task);

        try
        {
            await task;
        }
        catch { }

        Assert.AreEqual(TaskStatus.Faulted, task.Status);
        Assert.IsNotNull(task.Exception);
        Assert.IsInstanceOfType(task.Exception.InnerExceptions.FirstOrDefault(), typeof(ArgumentException));
    }
}
