using System.Collections.Generic;
using System.Threading.Tasks;
using AppAuthorizationService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Authorize(Policy = "protectedScope")]
[ApiController]
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
    [Route("", Name = nameof(GetAll))]
    public ActionResult<IEnumerable<string>> GetAll()
    {
        return new string[] { "data 1 from the api for the native application", "data 2 from the api for the native application" };
    }

    [Authorize("ValuesRoutePolicy")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet]
    [Route("{user}", Name = nameof(GetWithRouteParam))]
    public IActionResult GetWithRouteParam([FromRoute] string user)
    {
        return Ok($"get this data [{user}] using the route");
    }

    [Authorize("ValuesQueryPolicy")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet]
    [Route("q", Name = nameof(GetWithQueryParam))]
    public IActionResult GetWithQueryParam([FromQuery] string fruit)
    {
        return Ok($"get this data [{fruit}] using the query parameter");
    }

    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Produces(typeof(BodyData))]
    [HttpPost]
    [Route("", Name = nameof(Post))]
    public async Task<IActionResult> Post([FromBody] BodyData user)
    {
        var requirement = new ValuesRequestBodyRequirement();
        var resource = user;

        var authorizationResult =
            await _authorizationService.AuthorizeAsync(
                User, resource, requirement);

        if (authorizationResult.Succeeded)
        {
            return Ok($"posted this data [{user.User}] using the body");
        }
        else
        {
            return new ForbidResult();
        }
    }
}