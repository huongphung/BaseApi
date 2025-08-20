using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;

namespace WebApi.Controllers.v2
{
    [ApiVersion("2.0")]
    public class TestController : BaseApiController
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("API v2.0 Test Controller is working!");
        }
    }
}
