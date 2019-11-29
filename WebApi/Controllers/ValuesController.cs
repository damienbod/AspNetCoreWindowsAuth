using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Authorize(Policy = "protectedScope")]
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        [Route("")]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "data 1 from the api for the native application", "data 2 from the api for the native application" };
        }

        [Authorize("ValuesRoutePolicy")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("{id}")]
        [Route("{id}")]
        public IActionResult Get([FromRoute]string id)
        {
            return Ok($"get this data {id}");
        }

        [Authorize("ValuesRoutePolicy")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("q/{id}")]
        [Route("q/{id}")]
        public IActionResult Get([FromRoute]string id, [FromQuery]string fruit)
        {
            return Ok($"get this data {id}, {fruit}");
        }

        [Authorize("ValuesRequestBodyCheckPolicy")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost]
        public IActionResult Post([FromBody]string value)
        {
            return Ok($"post this data {value}");
        }
    }
}
