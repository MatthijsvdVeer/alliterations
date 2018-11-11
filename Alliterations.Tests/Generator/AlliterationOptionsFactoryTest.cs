using System;
using System.Collections.Generic;
using Alliterations.Api.Generator;
using FakeItEasy;
using FluentAssertions;
using Xunit;

namespace Alliterations.Tests.Generator
{
    public class AlliterationOptionsFactoryTest : IDisposable
    {
        private readonly IWordListBuilder wordListBuilder;

        private readonly IAllowedStartingCharacterBuilder allowedStartingCharacterBuilder;

        private readonly AlliterationOptionsFactory factory;

        public AlliterationOptionsFactoryTest()
        {
            this.wordListBuilder = A.Fake<IWordListBuilder>();
            this.allowedStartingCharacterBuilder = A.Fake<IAllowedStartingCharacterBuilder>();
            this.factory = new AlliterationOptionsFactory(this.wordListBuilder, this.allowedStartingCharacterBuilder);
        }

        [Fact]
        public void ReturnsOptionsWithWordLists()
        {
            var adjectives = new Dictionary<char, string[]>();
            A.CallTo(() => wordListBuilder.BuildDictionaryForCategory(AlliterationPart.Adjective, AlliterationCategory.Full))
                .Returns(adjectives);
            var nouns = new Dictionary<char, string[]>();
            A.CallTo(() => wordListBuilder.BuildDictionaryForCategory(AlliterationPart.Noun, AlliterationCategory.Full))
                .Returns(nouns);

            var options = this.factory.CreateAlliterationOptionsForCategory(AlliterationCategory.Full);

            options.Adjectives.Should().BeSameAs(adjectives);
            options.Nouns.Should().BeSameAs(nouns);
        }

        [Fact]
        public void ReturnsOptionsWithStartingCharacters()
        {
            A.CallTo(() => wordListBuilder.BuildDictionaryForCategory(AlliterationPart.Adjective, AlliterationCategory.Full))
                .Returns(new Dictionary<char, string[]>());
            A.CallTo(() => wordListBuilder.BuildDictionaryForCategory(AlliterationPart.Noun, AlliterationCategory.Full))
                .Returns(new Dictionary<char, string[]>());
            var startingCharacters = new char[0];
            A.CallTo(() => this.allowedStartingCharacterBuilder.GetAllowedStartingCharacters(A<IEnumerable<char>>._, A<IEnumerable<char>>._))
                .Returns(startingCharacters);
            
            var options = this.factory.CreateAlliterationOptionsForCategory(AlliterationCategory.Full);

            options.AllowedStartingCharacters.Should().BeSameAs(startingCharacters);
        }

        public void Dispose()
        {
        }
    }
}