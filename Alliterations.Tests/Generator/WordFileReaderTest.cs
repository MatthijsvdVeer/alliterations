using System;
using Alliterations.Api.Generator;
using FluentAssertions;
using Xunit;

namespace Alliterations.Tests.Generator
{
    public class WordFileReaderTest : IDisposable
    {
        private readonly WordFileReader reader;

        public WordFileReaderTest()
        {
            this.reader = new WordFileReader();
        }

        [Fact]
        public void ThrowsExceptionWhenPathNotExists()
        {
            Action codeToTest = () => this.reader.ReadLines("nope");

            Assert.Throws<InvalidOperationException>(codeToTest);
        }

        // Technically an integration test. Included for documentation.
        [Fact]
        public void ReadsAllLinesFromFile()
        {
            var lines = this.reader.ReadLines(".\\wordList.txt");

            lines.Should().HaveCount(2);
        }

        public void Dispose()
        {
        }
    }
}