using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using OpenCRM.Core.Data;
using OpenCRM.Core.Web.Areas.Identity.Models;
using OpenCRM.Core.Web.Models.Auth;
using OpenCRM.Core.Web.Services.EmailService;
using OpenCRM.Core.Web.Services.IdentityService;

namespace OpenCRM.Core.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IIdentityService _identityService;
        private readonly IEmailService _emailService;
        private readonly UserManager<UserEntity> _userManager;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            IIdentityService identityService,
            IEmailService emailService,
            UserManager<UserEntity> userManager,
            ILogger<AuthController> logger)
        {
            _identityService = identityService;
            _emailService = emailService;
            _userManager = userManager;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                var validationErrors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();
                return BadRequest(new LoginResponse { Success = false, Errors = validationErrors, Message = "Dati non validi." });
            }

            var result = await _identityService.SignInUser(request.Email, request.Password, request.RememberMe);
            if (!result.Succeeded)
            {
                var errors = new List<string>();
                if (result.IsLockedOut)
                {
                    errors.Add("Account bloccato. Riprova piu tardi.");
                }
                else if (result.IsNotAllowed)
                {
                    errors.Add("Accesso non consentito. Verifica la tua email.");
                }
                else
                {
                    errors.Add("Credenziali non valide.");
                }

                return Unauthorized(new LoginResponse
                {
                    Success = false,
                    Errors = errors,
                    Message = "Accesso non riuscito."
                });
            }

            var user = await _userManager.FindByEmailAsync(request.Email);

            return Ok(new LoginResponse
            {
                Success = true,
                Message = "Accesso eseguito.",
                UserId = user?.Id.ToString(),
                Email = user?.Email,
                EmailConfirmed = user?.EmailConfirmed ?? false
            });
        }

        [HttpPost("register")]
        public async Task<ActionResult<RegisterResponse>> Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                var validationErrors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();
                return BadRequest(new RegisterResponse { Success = false, Errors = validationErrors, Message = "Dati non validi." });
            }

            var input = new InputRegisterModel
            {
                Name = request.Name,
                Lastname = request.Lastname,
                Email = request.Email,
                Password = request.Password,
                ConfirmPassword = request.ConfirmPassword
            };

            var registerResult = await _identityService.RegisterUser(input);
            if (!registerResult.Item1.Succeeded)
            {
                var errors = registerResult.Item1.Errors.Select(e => e.Description).ToList();
                return BadRequest(new RegisterResponse
                {
                    Success = false,
                    Errors = errors,
                    Message = "Registrazione non riuscita."
                });
            }

            var user = registerResult.Item2;
            bool confirmationSent = false;

            try
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
                var confirmationUrl = Url.Action(
                    nameof(ConfirmEmail),
                    "Auth",
                    new { userId = user.Id, token = encodedToken },
                    Request.Scheme);

                if (!string.IsNullOrEmpty(user.Email) && !string.IsNullOrEmpty(confirmationUrl))
                {
                    confirmationSent = _emailService.SendEmail(
                        user.Email,
                        "Conferma la tua email",
                        $"Conferma il tuo account cliccando qui: {confirmationUrl}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante l'invio della mail di conferma per l'utente {UserId}", user.Id);
            }

            return Ok(new RegisterResponse
            {
                Success = true,
                UserId = user?.Id.ToString(),
                ConfirmationEmailSent = confirmationSent,
                Message = "Registrazione completata. Controlla la tua email per confermare l'account."
            });
        }

        [HttpGet("confirm-email")]
        public async Task<ActionResult<ConfirmEmailResponse>> ConfirmEmail([FromQuery] ConfirmEmailRequest request)
        {
            if (!ModelState.IsValid)
            {
                var validationErrors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();
                return BadRequest(new ConfirmEmailResponse { Success = false, Errors = validationErrors, Message = "Dati non validi." });
            }

            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
            {
                return NotFound(new ConfirmEmailResponse
                {
                    Success = false,
                    Errors = new List<string> { "Utente non trovato." },
                    Message = "Conferma email fallita."
                });
            }

            var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Token));
            var result = await _identityService.ConfirmUserEmail(user, decodedToken);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                return BadRequest(new ConfirmEmailResponse
                {
                    Success = false,
                    Errors = errors,
                    Message = "Conferma email fallita."
                });
            }

            return Ok(new ConfirmEmailResponse
            {
                Success = true,
                Message = "Email confermata con successo."
            });
        }
    }
}
