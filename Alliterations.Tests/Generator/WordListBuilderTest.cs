using System;
using Alliterations.Api.Generator;
using FakeItEasy;
using FluentAssertions;
using Xunit;

namespace Alliterations.Tests.Generator
{
    public class WordListBuilderTest : IDisposable
    {
        private readonly IWordListPathConfigurationProvider wordListPathConfigurationProvider;
        private readonly IWordFileReader wordFileReader;
        private readonly WordListBuilder builder;

        public WordListBuilderTest()
        {
            this.wordListPathConfigurationProvider = A.Fake<IWordListPathConfigurationProvider>();
            this.wordFileReader = A.Fake<IWordFileReader>();
            this.builder = new WordListBuilder(this.wordListPathConfigurationProvider, this.wordFileReader);
        }

        [Fact]
        public void GroupsWordsByFirstLetter()
        {
            A.CallTo(() => this.wordFileReader.ReadLines(A<string>._))
            .Returns(new[] { "monkey", "maroon", "horse" });

            var dictionary = this.builder.BuildDictionaryForCategory(AlliterationPart.Adjective, AlliterationCategory.Full);

            dictionary['M'].Should().HaveCount(2);
            dictionary['H'].Should().HaveCount(1);
        }

        [Fact]
        public void KeysAreUpperCase()
        {
            A.CallTo(() => this.wordFileReader.ReadLines(A<string>._))
            .Returns(new[] { "monkey", "Maroon" });

            var dictionary = this.builder.BuildDictionaryForCategory(AlliterationPart.Adjective, AlliterationCategory.Full);

            dictionary['M'].Should().HaveCount(2);
            dictionary.Keys.Should().NotContain('m');
        }

        public void Dispose()
        {
        }
    }
}