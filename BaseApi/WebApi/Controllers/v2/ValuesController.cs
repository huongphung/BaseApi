using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Asp.Versioning;

namespace WebApi.Controllers.v2
{
    [ApiVersion("2.0")]
    public class ValuesController : BaseApiController
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("API v2.0 Value Controller is working!");
        }
    }
}
