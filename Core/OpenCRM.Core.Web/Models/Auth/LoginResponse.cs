using System.Collections.Generic;

namespace OpenCRM.Core.Web.Models.Auth
{
    public class LoginResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? UserId { get; set; }
        public string? Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public List<string> Errors { get; set; } = new();
    }
}
