using System;
using Microsoft.Extensions.Caching.Memory;

namespace Alliterations.Api
{
    internal interface ICachingProvider
    {
        T GetOrSetInCache<T>(string key, Func<T> factoryFunction);
    }

    internal sealed class CachingProvider : ICachingProvider
    {
        private readonly IMemoryCache memoryCache;

        private static readonly object CacheLock = new object();

        private static readonly MemoryCacheEntryOptions MemoryCacheEntryOptions = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromMinutes(15));

        public CachingProvider(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }

        public T GetOrSetInCache<T>(string key, Func<T> factoryFunction)
        {
            if (this.memoryCache.TryGetValue(key, out T result))
            {
                return result;
            }

            lock (CacheLock)
            {
                return this.memoryCache.TryGetValue(key, out result)
                    ? result
                    : this.memoryCache.Set(key, factoryFunction.Invoke(), MemoryCacheEntryOptions);
            }
        }
    }
}