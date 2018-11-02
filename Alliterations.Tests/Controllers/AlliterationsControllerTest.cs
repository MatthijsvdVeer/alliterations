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
            .Returns(new[] { "Foo", "Bar" });
            var result = this.controller.Get(2);

            result.Value.Should().HaveCount(2);
        }

        [Fact]
        public void Get_DefaultValueForCountIsOne()
        {
            var result = this.controller.Get();

            A.CallTo(() => this.alliterationsProvider.GetAlliterationsByCategory(AlliterationCategory.Full, 1))
            .MustHaveHappened();
        }

        public void Dispose()
        {
        }
    }
}
