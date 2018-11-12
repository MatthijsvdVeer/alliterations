using System;
using System.Collections.Generic;
using Alliterations.Api.Controllers;
using Alliterations.Api.Generator;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Alliterations.Tests.Controllers
{
    public class AlliterationsControllerTest : IDisposable
    {
        private readonly IAlliterationsProvider alliterationsProvider;

        private readonly AlliterationsController controller;

        public AlliterationsControllerTest()
        {
            this.alliterationsProvider = A.Fake<IAlliterationsProvider>();
            this.controller = new AlliterationsController(this.alliterationsProvider);
        }

        [Fact]
        public void Get_ResultMustBeActionResult()
        {
            var result = this.controller.Get();
            result.Should().BeOfType(typeof(ActionResult<IEnumerable<String>>));
        }

        [Fact]
        public void Get_ReturnsAllAlliterations()
        {
            A.CallTo(() => this.alliterationsProvider.GetAlliterationsByCategory(AlliterationCategory.Full, 2))
                .Returns(new[] {"Foo", "Bar"});
            var result = this.controller.Get(2);

            result.Value.Should().HaveCount(2);
        }

        [Fact]
        public void Get_DefaultValueForCountIsOne()
        {
            this.controller.Get();

            A.CallTo(() => this.alliterationsProvider.GetAlliterationsByCategory(AlliterationCategory.Full, 1))
                .MustHaveHappened();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(501)]
        public void Get_ReturnsBadRequestWhenCountOutOfBounds(int count)
        {
            var result = this.controller.Get(count);

            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public void Get_AcceptsStartingCharactersUpperAndLower()
        {
            A.CallTo(() =>
                    this.alliterationsProvider.GetAlliterationsByCategoryAndStartingChar(AlliterationCategory.Full,
                        A<char>._, 1))
                .Returns(new[] {"Foo", "Bar"});

            // Could do this as an XUnit Fact but would generate 52 unit tests.
            const string acceptedStartingChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

            foreach (var startingChar in acceptedStartingChars)
            {
                var result = this.controller.Get(1, startingChar);
                result.Value.Should().HaveCount(2);
            }
        }

        [Theory]
        [InlineData('@')]
        [InlineData('-')]
        public void Get_RejectsCharsNotInLatinAlphabet(char startingCharacter)
        {
            var result = this.controller.Get(1, startingCharacter);

            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        public void Dispose()
        {
        }
    }
}