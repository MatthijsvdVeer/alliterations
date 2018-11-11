using System;
using System.Collections.Generic;
using System.Linq;
using Alliterations.Api;
using Alliterations.Api.Generator;
using Alliterations.Tests.Mocks;
using FakeItEasy;
using FluentAssertions;
using Xunit;

namespace Alliterations.Tests.Generator
{
    public class AlliterationsProviderTest : IDisposable
    {
        private readonly IAlliterationOptionsFactory alliterationOptionsFactory;

        private readonly IRandomNumberGenerator randomNumberGenerator;

        private readonly AlliterationsProvider provider;

        private static readonly char[] StartingCharacters = new char[] {'L', 'M'};

        private static readonly Dictionary<char, string[]> Adjectives = new Dictionary<char, string[]>
        {
            {'L', new[] {"Leaky"}},
            {'M', new[] {"Mauled"}}
        };

        private static readonly Dictionary<char, string[]> Nouns = new Dictionary<char, string[]>
        {
            {'L', new[] {"Leprechaun"}},
            {'M', new[] {"Marker"}}
        };

        public AlliterationsProviderTest()
        {
            this.alliterationOptionsFactory = A.Fake<IAlliterationOptionsFactory>();
            this.randomNumberGenerator = A.Fake<IRandomNumberGenerator>();
            this.provider = new AlliterationsProvider(this.alliterationOptionsFactory, this.randomNumberGenerator,
                new MockCachingProvider());
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void ThrowsExceptionOnZeroOrNegative(int count)
        {
            void Test() => this.provider.GetAlliterationsByCategory(AlliterationCategory.Full, count).Any();

            Assert.Throws<ArgumentException>((Action) Test);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void ThrowsExceptionOnZeroOrNegativeWithSpecificStartingChar(int count)
        {
            void Test() => this.provider
                .GetAlliterationsByCategoryAndStartingChar(AlliterationCategory.Full, 'S', count).Any();

            Assert.Throws<ArgumentException>((Action) Test);
        }

        [Fact]
        public void CanGetSingleAlliterationWithRandomStartingChar()
        {
            A.CallTo(() =>
                    this.alliterationOptionsFactory.CreateAlliterationOptionsForCategory(AlliterationCategory.Full))
                .Returns(new AlliterationOptions
                    {Adjectives = Adjectives, Nouns = Nouns, AllowedStartingCharacters = StartingCharacters});
            A.CallTo(() => this.randomNumberGenerator.GetNext(1)).Returns(0);

            var alliterations = this.provider.GetAlliterationsByCategory(AlliterationCategory.Full, 1);

            alliterations.Should().HaveCount(1);
            alliterations.Single().Should().BeEquivalentTo("Leaky Leprechaun");
        }

        [Fact]
        public void GetsCorrectCountOfAlliterations()
        {
            A.CallTo(() =>
                    this.alliterationOptionsFactory.CreateAlliterationOptionsForCategory(AlliterationCategory.Full))
                .Returns(new AlliterationOptions
                    {Adjectives = Adjectives, Nouns = Nouns, AllowedStartingCharacters = StartingCharacters});
            A.CallTo(() => this.randomNumberGenerator.GetNext(1)).Returns(0);

            var alliterations = this.provider.GetAlliterationsByCategory(AlliterationCategory.Full, 10);

            alliterations.Should().HaveCount(10);
        }

        [Fact]
        public void CanGetSingleAlliterationWithSelectedStartingChar()
        {
            A.CallTo(() =>
                    this.alliterationOptionsFactory.CreateAlliterationOptionsForCategory(AlliterationCategory.Full))
                .Returns(new AlliterationOptions
                    {Adjectives = Adjectives, Nouns = Nouns, AllowedStartingCharacters = StartingCharacters});
            A.CallTo(() => this.randomNumberGenerator.GetNext(1)).Returns(0);

            var alliterations =
                this.provider.GetAlliterationsByCategoryAndStartingChar(AlliterationCategory.Full, 'M', 1);

            alliterations.Should().HaveCount(1);
            alliterations.Single().Should().BeEquivalentTo("Mauled Marker");
        }

        [Fact]
        public void InvalidStartingCharacterThrowsException()
        {
            A.CallTo(() =>
                    this.alliterationOptionsFactory.CreateAlliterationOptionsForCategory(AlliterationCategory.Full))
                .Returns(new AlliterationOptions
                    {Adjectives = Adjectives, Nouns = Nouns, AllowedStartingCharacters = StartingCharacters});

            void Test() => this.provider.GetAlliterationsByCategoryAndStartingChar(AlliterationCategory.Full, 'S', 1)
                .Any();

            Assert.Throws<ArgumentException>((Action) Test);
        }

        public void Dispose()
        {
        }
    }
}