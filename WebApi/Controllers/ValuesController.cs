using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppAuthorizationService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Authorize(Policy = "protectedScope")]
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly IAuthorizationService _authorizationService;

        public ValuesController(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        [Route("")]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "data 1 from the api for the native application", "data 2 from the api for the native application" };
        }

        [Authorize("ValuesRoutePolicy")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("{user}")]
        [Route("{user}")]
        public IActionResult Get([FromRoute]string user)
        {
            return Ok($"get this data {user}");
        }

        [Authorize("ValuesQueryPolicy")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("q/{user}")]
        [Route("q/{user}")]
        public IActionResult Get([FromRoute]string user, [FromQuery]string fruit)
        {
            return Ok($"get this data {user}, {fruit}");
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]BodyData user)
        {
            var requirement = new ValuesRequestBodyRequirement();
            var resource = user;

            var authorizationResult =
                await _authorizationService.AuthorizeAsync(
                    User, resource, requirement);

            if (authorizationResult.Succeeded)
            {
                return Ok($"posted this data {user.User}");
            }
            else
            {
                return new ForbidResult();
            }
        }
    }
}
