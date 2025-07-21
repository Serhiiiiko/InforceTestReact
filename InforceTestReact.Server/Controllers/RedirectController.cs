using Microsoft.AspNetCore.Mvc;
using InforceTestReact.Server.Services;

namespace InforceTestReact.Server.Controllers
{
    [ApiController]
    public class RedirectController : ControllerBase
    {
        private readonly UrlService _urlService;

        public RedirectController(UrlService urlService)
        {
            _urlService = urlService;
        }

        [HttpGet("{shortCode}")]
        public async Task<ActionResult> RedirectToUrl(string shortCode)
        {
            var originalUrl = await _urlService.GetOriginalUrlAsync(shortCode);
            if (originalUrl == null)
                return NotFound();

            await _urlService.UpdateClickCountAsync(shortCode);
            return Redirect(originalUrl);
        }
    }
}