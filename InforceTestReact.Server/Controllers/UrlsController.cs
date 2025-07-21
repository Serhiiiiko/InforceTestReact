using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using InforceTestReact.Server.Models;
using InforceTestReact.Server.Services;

namespace InforceTestReact.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UrlsController : ControllerBase
    {
        private readonly UrlService _urlService;

        public UrlsController(UrlService urlService)
        {
            _urlService = urlService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UrlMappingDto>>> GetAllUrls()
        {
            var urls = await _urlService.GetAllUrlsAsync();
            return Ok(urls);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<UrlDetailsDto>> GetUrlDetails(int id)
        {
            var url = await _urlService.GetUrlDetailsAsync(id);
            if (url == null)
                return NotFound();

            return Ok(url);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<UrlMappingDto>> CreateShortUrl(CreateUrlRequest request)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId == null)
                    return Unauthorized();

                var result = await _urlService.CreateShortUrlAsync(request, userId);
                return CreatedAtAction(nameof(GetUrlDetails), new { id = result.Id }, result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> DeleteUrl(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized();

            var isAdmin = User.IsInRole("Admin");
            var result = await _urlService.DeleteUrlAsync(id, userId, isAdmin);

            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}