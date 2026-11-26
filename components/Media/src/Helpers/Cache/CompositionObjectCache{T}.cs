// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Runtime.CompilerServices;

#if WINUI2
using Windows.UI.Composition;
#elif WINUI3
using Microsoft.UI.Composition;
#endif

namespace CommunityToolkit.WinUI.Media.Helpers.Cache;

/// <summary>
/// A <see langword="class"/> used to cache reusable <see cref="CompositionObject"/> instances in each UI thread
/// </summary>
/// <typeparam name="T">The type of instances to cache</typeparam>
internal sealed class CompositionObjectCache<T>
    where T : CompositionObject
{
    /// <summary>
    /// The cache of weak references of type <typeparamref name="T"/>, to avoid memory leaks
    /// </summary>
    /// <remarks>
    /// The values are always <see cref="WeakReference{T}"/> instances. They are typed as <see cref="object"/> for consistency with
    /// <see cref="CompositionObjectCache{TKey, TValue}"/>, to avoid generic <see cref="ConditionalWeakTable{TKey, TValue}"/> value types
    /// (see the remarks there). This field is private and only accessed here, so <see cref="Unsafe.As{T}(object)"/> is safe.
    /// </remarks>
    private readonly ConditionalWeakTable<Compositor, object> cache = new ConditionalWeakTable<Compositor, object>();

    /// <summary>
    /// Tries to retrieve a valid <typeparamref name="T"/> instance from the cache, and uses the provided factory if an existing item is not found
    /// </summary>
    /// <param name="compositor">The current <see cref="Compositor"/> instance to get the value for</param>
    /// <param name="producer">A <see cref="Func{TResult}"/> instance used to produce a <typeparamref name="T"/> instance</param>
    /// <returns>A <typeparamref name="T"/> instance that is linked to <paramref name="compositor"/></returns>
    public T GetValue(Compositor compositor, Func<Compositor, T> producer)
    {
        lock (cache)
        {
            if (this.cache.TryGetValue(compositor, out var reference) &&
                Unsafe.As<WeakReference<T>>(reference).TryGetTarget(out var instance))
            {
                return instance;
            }

            // Create a new instance when needed
            var fallback = producer(compositor);
            this.cache.AddOrUpdate(compositor, new WeakReference<T>(fallback));

            return fallback;
        }
    }
}
