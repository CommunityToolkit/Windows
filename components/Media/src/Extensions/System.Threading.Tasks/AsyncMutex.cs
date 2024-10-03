// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Runtime.CompilerServices;

namespace CommunityToolkit.WinUI.Media;

/// <summary>
/// An <see langword="async"/> <see cref="AsyncMutex"/> implementation that can be easily used inside a <see langword="using"/> block
/// </summary>
#pragma warning disable CA1001 // Types that own disposable fields should be disposable
#pragma warning disable CsWinRT1028 // Partial not required for types never passed to native WinRT method.
internal sealed class AsyncMutex
#pragma warning restore CsWinRT1028
#pragma warning restore CA1001 // Types that own disposable fields should be disposable
{
    /// <summary>
    /// The underlying <see cref="SemaphoreSlim"/> instance in use
    /// </summary>
    private readonly SemaphoreSlim semaphore = new SemaphoreSlim(1);

    /// <summary>
    /// Acquires a lock for the current instance, that is automatically released outside the <see langword="using"/> block
    /// </summary>
    /// <returns>A <see cref="Task{T}"/> that returns an <see cref="IDisposable"/> instance to release the lock</returns>
    public async Task<IDisposable> LockAsync()
    {
        await this.semaphore.WaitAsync().ConfigureAwait(false);

        return new Lock(this.semaphore);
    }

    /// <summary>
    /// Private class that implements the automatic release of the semaphore
    /// </summary>
#pragma warning disable CsWinRT1028 // Partial not required for types never passed to native WinRT method.
    private sealed class Lock : IDisposable
#pragma warning restore CsWinRT1028
    {
        /// <summary>
        /// The <see cref="SemaphoreSlim"/> instance of the parent class
        /// </summary>
        private readonly SemaphoreSlim semaphore;

        /// <summary>
        /// Initializes a new instance of the <see cref="Lock"/> class.
        /// </summary>
        /// <param name="semaphore">The <see cref="SemaphoreSlim"/> instance of the parent class</param>
        public Lock(SemaphoreSlim semaphore)
        {
            this.semaphore = semaphore;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void IDisposable.Dispose()
        {
            this.semaphore.Release();
        }
    }
}
