using System.ComponentModel.DataAnnotations;

namespace OpenCRM.Core.Web.Models.Auth
{
    public class RegisterRequest
    {
        public string Name { get; set; } = string.Empty;

        public string Lastname { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [Compare(nameof(Password), ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
