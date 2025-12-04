using Microsoft.AspNetCore.Authorization;
using OpenCRM.Core.Web.Models;

namespace OpenCRM.Web.Pages
{
    [Authorize(Roles = Roles.SUPER_ADMIN_ROLE)]
    public class PrivacyModel : CorePageModel
    {
        private readonly ILogger<PrivacyModel> _logger;

        public PrivacyModel(ILogger<PrivacyModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}