using System;
using System.Collections.Generic;
using System.Text;
using Alliterations.Api;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Xunit;

namespace Alliterations.Tests
{
    public class CachingProviderTest : IDisposable
    {
        private readonly IMemoryCache memoryCache;
        private readonly CachingProvider cachingProvider;

        public CachingProviderTest()
        {
            this.memoryCache = A.Fake<IMemoryCache>();
            this.cachingProvider = new CachingProvider(memoryCache);
        }

        [Fact]
        public void ChecksAndReturnsFromCacheIfAvailable()
        {
            const string expectedResult = "right";
            // Foo is an object, changing it to string would target the extension method that can't be faked.
            object foo;
            const string cachingKey = "key";
            A.CallTo(() => this.memoryCache.TryGetValue(cachingKey, out foo))
                .Returns(true)
                .AssignsOutAndRefParameters(expectedResult);

            var actual = this.cachingProvider.GetOrSetInCache(cachingKey, () => "wrong");

            actual.Should().BeSameAs(expectedResult);
        }

        /// <summary>
        /// Checks that the cache is checked twice. This makes sure that threads
        /// that obtain the lock while waiting for the cache to be filled don't
        /// fill the cache as well.
        /// </summary>
        [Fact]
        public void UsesDoubleCheckedLocking()
        {
            object foo;
            const string cachingKey = "key";
            A.CallTo(() => this.memoryCache.TryGetValue(cachingKey, out foo)).Returns(false);

            this.cachingProvider.GetOrSetInCache(cachingKey, () => "value");

            A.CallTo(() => this.memoryCache.TryGetValue(cachingKey, out foo)).MustHaveHappened(Repeated.Exactly.Twice);
        }

        [Fact]
        public void InvokesFactoryMethodIfNotInCache()
        {
            object foo;
            const string cachingKey = "key";
            A.CallTo(() => this.memoryCache.TryGetValue(cachingKey, out foo)).Returns(false);

            const string expectedResult = "value";
            var result = this.cachingProvider.GetOrSetInCache(cachingKey, () => expectedResult);

            result.Should().BeSameAs(expectedResult);
        }

        public void Dispose()
        {
        }
    }
}