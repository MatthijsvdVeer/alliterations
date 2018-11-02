using System;
using System.Collections.Generic;
using System.Linq;
using Alliterations.Api.Generator;
using FakeItEasy;
using FluentAssertions;
using Xunit;

namespace Alliterations.Tests.Generator
{
    public class AlliterationsProviderTest : IDisposable
    {
        private readonly static char[] startingCharacters = new char[] { 'L', 'M' };

        private readonly IAlliterationOptionsBuilder alliterationOptionsBuilder;

        private readonly IRandomNumberGenerator randomNumberGenerator;

        private readonly AlliterationsProvider provider;

        private readonly static Dictionary<char, string[]> adjectives = new Dictionary<char, string[]>
        {
            {'L', new[] {"Leaky"}},
            { 'M', new[] {"Mauled"} }
        };

        private readonly static Dictionary<char, string[]> nouns = new Dictionary<char, string[]>
        {
            {'L', new[] {"Leprechaun"}},
            {'M', new[] {"Marker"}}
        };

        public AlliterationsProviderTest()
        {
            this.alliterationOptionsBuilder = A.Fake<IAlliterationOptionsBuilder>();
            this.randomNumberGenerator = A.Fake<IRandomNumberGenerator>();
            this.provider = new AlliterationsProvider(this.alliterationOptionsBuilder, this.randomNumberGenerator);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void ThrowsExceptionOnZeroOrNegative(int count)
        {
            Action test = () => this.provider.GetAlliterationsByCategory(AlliterationCategory.Full, count).Any();
            
            Assert.Throws<ArgumentException>(test);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void ThrowsExceptionOnZeroOrNegativeWithSpecificStartingChar(int count)
        {
            Action test = () => this.provider.GetAlliterationsByCategoryAndStartingChar(AlliterationCategory.Full, 'S', count).Any();

            Assert.Throws<ArgumentException>(test);
        }

        [Fact]
        public void CanGetSingleAlliterationWithRandomStartingChar()
        {
            A.CallTo(() => this.alliterationOptionsBuilder.CreateAlliterationOptionsForCategory(AlliterationCategory.Full))
            .Returns(new AlliterationOptions { Adjectives = adjectives, Nouns = nouns, AllowedStartingCharacters = startingCharacters });
            A.CallTo(() => this.randomNumberGenerator.GetNext(1)).Returns(0);

            var alliterations = this.provider.GetAlliterationsByCategory(AlliterationCategory.Full, 1);

            alliterations.Should().HaveCount(1);
            alliterations.Single().Should().BeEquivalentTo("Leaky Leprechaun");
        }

        [Fact]
        public void GetsCorrectCountOfAlliterations()
        {
            A.CallTo(() => this.alliterationOptionsBuilder.CreateAlliterationOptionsForCategory(AlliterationCategory.Full))
            .Returns(new AlliterationOptions { Adjectives = adjectives, Nouns = nouns, AllowedStartingCharacters = startingCharacters });
            A.CallTo(() => this.randomNumberGenerator.GetNext(1)).Returns(0);

            var alliterations = this.provider.GetAlliterationsByCategory(AlliterationCategory.Full, 10);

            alliterations.Should().HaveCount(10);
        }

        [Fact]
        public void CanGetSingleAlliterationWithSelectedStartingChar()
        {
            A.CallTo(() => this.alliterationOptionsBuilder.CreateAlliterationOptionsForCategory(AlliterationCategory.Full))
            .Returns(new AlliterationOptions { Adjectives = adjectives, Nouns = nouns, AllowedStartingCharacters = startingCharacters });
            A.CallTo(() => this.randomNumberGenerator.GetNext(1)).Returns(0);

            var alliterations = this.provider.GetAlliterationsByCategoryAndStartingChar(AlliterationCategory.Full, 'M', 1);

            alliterations.Should().HaveCount(1);
            alliterations.Single().Should().BeEquivalentTo("Mauled Marker");
        }

        [Fact]
        public void InvalidStartingCharacterThrowsException()
        {
            
            A.CallTo(() => this.alliterationOptionsBuilder.CreateAlliterationOptionsForCategory(AlliterationCategory.Full))
            .Returns(new AlliterationOptions { Adjectives = adjectives, Nouns = nouns, AllowedStartingCharacters = startingCharacters });
            
            Action test = () => this.provider.GetAlliterationsByCategoryAndStartingChar(AlliterationCategory.Full, 'S', 1).Any();

            Assert.Throws<ArgumentException>(test);
        }

        public void Dispose()
        {
        }
    }
}