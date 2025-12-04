using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenCRM.Core.Web.Services.IdentityService;
using OpenCRM.Web.DTO;

namespace OpenCRM.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IIdentityService _identityService;

        public AuthController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _identityService.SignInUser(request.Username, request.Password, request.RememberMe);

            if (result.Succeeded)
            {
                return Ok(new { message = "Login successful" });
            }

            if (result.IsLockedOut)
            {
                return StatusCode(StatusCodes.Status423Locked, new { message = "Account locked" });
            }

            return Unauthorized(new { message = "Invalid credentials" });
        }
    }
}
