// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.Common;

namespace CommunityToolkit.WinUI.Converters;

/// <summary>
/// A converter that can be used to safely retrieve results from <see cref="Task{T}"/> instances.
/// This is needed because accessing <see cref="Task{TResult}.Result"/> when the task has not
/// completed yet will block the current thread and might cause a deadlock (eg. if the task was
/// scheduled on the same synchronization context where the result is being retrieved from).
/// The methods in this converter will safely return <see langword="null"/> if the input
/// task is not set yet, still running, has faulted, or has been canceled.
/// </summary>
#if NET8_0_OR_GREATER
[System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("This method uses reflection to try to access the Task<T>.Result property of the input Task instance.")]
#endif
public sealed partial class TaskResultConverter : IValueConverter
{
    /// <inheritdoc/>
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is Task task)
        {
            return task.GetResultOrDefault()!;
        }
        else if (value is null)
        {
            // Note: We don't return unset value here as normal, as whatever expecting the result
            // may be trying to process this value and would get an unexpected type not normally seen.
            // See this comment here: https://github.com/CommunityToolkit/WindowsCommunityToolkit/issues/4505#issuecomment-1286221861
            return null!;
        }

        // Otherwise, we'll just pass through whatever value/result was given to us.
        return value;
    }

    /// <inheritdoc/>
    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
