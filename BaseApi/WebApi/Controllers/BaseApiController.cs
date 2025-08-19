using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/v{v:apiVersion}/[controller]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
    }
}
