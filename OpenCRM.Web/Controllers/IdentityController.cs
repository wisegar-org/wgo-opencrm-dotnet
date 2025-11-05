using Microsoft.AspNetCore.Mvc;
using OpenCRM.Core.Web.Models;
using OpenCRM.Core.Web.Services.IdentityService;
using OpenCRM.Web.DTOs;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OpenCRM.Web.Controllers
{
    [Route("api/identity")]
    [ApiController]
    public class IdentityController : ControllerBase
    {

        private readonly IIdentityService _identityService;
        private readonly ILogger<IdentityController> _logger;   

        public IdentityController(IIdentityService identityService, ILogger<IdentityController> logger)
        {
            _identityService = identityService;
            _logger = logger;   
        }

        [HttpPost("signin")]
        public async Task<ActionResult<UserModel>> SignInAsync([FromBody] IdentityDTOModel data)
        {
            var result = await _identityService.SignInUser(data.Username, data.Password, false);
            if (!result.Succeeded) {
                return Unauthorized("Invalid authentication");
            }

            var user = await _identityService.GetUserByUsername(data.Username);
            if (user == null) {
                return Unauthorized("User not found after authentication");
            }

            UserModel userModel = new UserModel()
            {
                UserId = user.Id,
                Name = user.Name,
                Lastname = user.Lastname,
                UserName = user.UserName ?? data.Username,
                Email = user.Email ?? "",
                Avatar = user.Avatar
            };

            return Ok(userModel);
        }

    }
}
