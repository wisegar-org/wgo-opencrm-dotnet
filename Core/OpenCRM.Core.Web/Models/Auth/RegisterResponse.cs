using System.Collections.Generic;

namespace OpenCRM.Core.Web.Models.Auth
{
    public class RegisterResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? UserId { get; set; }
        public bool ConfirmationEmailSent { get; set; }
        public List<string> Errors { get; set; } = new();
    }
}
