using System.Collections.Generic;

namespace OpenCRM.Core.Web.Models.Auth
{
    public class ConfirmEmailResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<string> Errors { get; set; } = new();
    }
}
