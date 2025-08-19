using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
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
