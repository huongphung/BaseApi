using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;

namespace WebApi.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{v:apiVersion}/[controller]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
    }
}
