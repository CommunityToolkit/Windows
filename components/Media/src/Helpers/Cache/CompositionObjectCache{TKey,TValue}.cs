// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Runtime.CompilerServices;
#if NET8_0_OR_GREATER
using System.Runtime.InteropServices;
#endif

#if WINUI2
using Windows.UI.Composition;
#elif WINUI3
using Microsoft.UI.Composition;
#endif

namespace CommunityToolkit.WinUI.Media.Helpers.Cache;

/// <summary>
/// A <see langword="class"/> used to cache reusable <see cref="CompositionObject"/> instances with an associated key
/// </summary>
/// <typeparam name="TKey">The type of key to classify the items in the cache</typeparam>
/// <typeparam name="TValue">The type of items stored in the cache</typeparam>
internal sealed class CompositionObjectCache<TKey, TValue>
    where TValue : CompositionObject
    where TKey : notnull
{
    /// <summary>
    /// The cache of weak references of type <typeparamref name="TValue"/> to <typeparamref name="TKey"/> instances, to avoid memory leaks
    /// </summary>
    /// <remarks>
    /// The values are always <c>Dictionary&lt;TKey, WeakReference&lt;TValue&gt;&gt;</c> instances. They are typed as <see cref="object"/> because the
    /// <c>[DynamicallyAccessedMembers(PublicParameterlessConstructor)]</c> annotation on the value type of <see cref="ConditionalWeakTable{TKey, TValue}"/>
    /// would otherwise make trimming and NativeAOT keep the parameterless constructors of all generic instantiations (binary size).
    /// This field is private and only accessed here, so <see cref="Unsafe.As{T}(object)"/> is safe.
    /// </remarks>
    private readonly ConditionalWeakTable<Compositor, object> cache = new ConditionalWeakTable<Compositor, object>();

    /// <summary>
    /// Tries to retrieve a valid instance from the cache, and uses the provided factory if an existing item is not found
    /// </summary>
    /// <param name="compositor">The current <see cref="Compositor"/> instance to get the value for</param>
    /// <param name="key">The key to look for</param>
    /// <param name="result">The resulting value, if existing</param>
    /// <returns><see langword="true"/> if the target value has been found, <see langword="false"/> otherwise</returns>
    public bool TryGetValue(Compositor compositor, TKey key, out TValue? result)
    {
        lock (this.cache)
        {
            if (this.cache.TryGetValue(compositor, out var map) &&
                Unsafe.As<Dictionary<TKey, WeakReference<TValue>>>(map).TryGetValue(key, out var reference) &&
                reference.TryGetTarget(out result))
            {
                return true;
            }

            result = null;
            return false;
        }
    }

    /// <summary>
    /// Adds or updates a value with the specified key to the cache
    /// </summary>
    /// <param name="compositor">The current <see cref="Compositor"/> instance to get the value for</param>
    /// <param name="key">The key of the item to add</param>
    /// <param name="value">The value to add</param>
    public void AddOrUpdate(Compositor compositor, TKey key, TValue value)
    {
        lock (this.cache)
        {
            if (this.cache.TryGetValue(compositor, out var map))
            {
                Dictionary<TKey, WeakReference<TValue>> dictionary = Unsafe.As<Dictionary<TKey, WeakReference<TValue>>>(map);

#if NET8_0_OR_GREATER
                CollectionsMarshal.GetValueRefOrAddDefault(dictionary, key, out _) = new WeakReference<TValue>(value);
#else
                _ = dictionary.Remove(key);

                dictionary.Add(key, new WeakReference<TValue>(value));
#endif
            }
            else
            {
                map = new Dictionary<TKey, WeakReference<TValue>> { [key] = new WeakReference<TValue>(value) };

                this.cache.Add(compositor, map);
            }
        }
    }
}
