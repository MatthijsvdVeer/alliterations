using System;
using System.Collections.Generic;
using System.Linq;
using Alliterations.Api.Generator;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;

namespace Alliterations.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAllOrigins")]
    public class AlliterationsController : ControllerBase
    {
        private const int MaxCount = 500;
        private const int MinCount = 1;

        private readonly IAlliterationsProvider alliterationsProvider;

        public AlliterationsController(IAlliterationsProvider alliterationsProvider)
        {
            this.alliterationsProvider = alliterationsProvider;
        }

        // GET api/alliterations?count=5
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get(int count = MinCount)
        {
            if (count < MinCount || count > MaxCount)
            {
                return BadRequest($"Can't generate more than {MaxCount} or less than {MinCount}.");
            }

            return this.alliterationsProvider.GetAlliterationsByCategory(AlliterationCategory.Full, count).ToArray();
        }
    }
}
