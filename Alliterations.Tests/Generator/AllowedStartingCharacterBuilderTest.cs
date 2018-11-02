using System;
using Alliterations.Api.Generator;
using FluentAssertions;
using Xunit;

namespace Alliterations.Tests.Generator
{
    public class AllowedStartingCharacterBuilderTest : IDisposable
    {
        private readonly AllowedStartingCharacterBuilder builder;

        public AllowedStartingCharacterBuilderTest()
        {
            this.builder = new AllowedStartingCharacterBuilder();
        }

        [Fact]
        public void ReturnsEmptyArrayWhenNoMatch()
        {
            var a = new[] { 'A', 'B', 'C' };
            var b = new[] { 'D', 'E', 'F' };

            var startingCharacters = this.builder.GetAllowedStartingCharacters(a, b);

            startingCharacters.Should().BeEmpty();
        }

        [Fact]
        public void ReturnsStartingCharactersThatAreInBothCollections()
        {
            var a = new[] { 'A', 'B', 'C' };
            var b = new[] { 'A', 'X', 'C' };

            var startingCharacters = this.builder.GetAllowedStartingCharacters(a, b);

            startingCharacters.Should().HaveCount(2);
            startingCharacters.Should().Contain('A');
            startingCharacters.Should().Contain('C');
        }

        public void Dispose()
        {
        }
    }
}