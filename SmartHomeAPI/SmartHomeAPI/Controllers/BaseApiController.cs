using Microsoft.AspNetCore.Mvc;

namespace SmartHomeAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseApiController: ControllerBase { }
}