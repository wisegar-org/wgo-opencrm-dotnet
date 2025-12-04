using Microsoft.AspNetCore.Mvc;
using OpenCRM.Core.DataBlock;
using OpenCRM.Core.Web.Models;
using OpenCRM.Core.Web.Services.CardBlockService;
using OpenCRM.Core.Web.Services.IdentityService;

namespace OpenCRM.Web.Pages
{
    public class IndexModel : CorePageModel
    {
        private readonly ILogger<IndexModel> _logger;

        private readonly ICardBlockService _blockService;
        private readonly IIdentityService _identityService;

        [BindProperty]
        public CardBlockModel? Block { get; set; }

        public string? Lang { get; set; }
        public IndexModel(ILogger<IndexModel> logger, ICardBlockService blockService, IIdentityService identityService)
        {
            _logger = logger;
            _blockService = blockService;
            _identityService = identityService;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            var block = await _blockService.ShowCardBlock();

            if (block != null)
            {
                Block = block.Data;
            }

            var dataSesison = _identityService.GetSession();
            if (dataSesison == null) Lang = "IT";
            Lang = dataSesison != null ? dataSesison.Lang : "Default dal browser";

            return Page();
        }
    }
}
