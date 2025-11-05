using Microsoft.AspNetCore.Mvc;

namespace OpenCRM.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpGet(Name = "user")]
        public IActionResult GetUser()
        {
            return Ok("User data");
        }
    }
}
