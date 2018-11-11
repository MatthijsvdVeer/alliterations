using System;
using Alliterations.Api;

namespace Alliterations.Tests.Mocks
{
    /// <summary>
    /// Mock caching provider that always returns the factory function.
    /// </summary>
    internal sealed class MockCachingProvider : ICachingProvider
    {
        public T GetOrSetInCache<T>(string key, Func<T> factoryFunction)
        {
            return factoryFunction();
        }
    }
}
