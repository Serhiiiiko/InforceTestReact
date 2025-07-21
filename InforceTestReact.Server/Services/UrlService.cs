using Microsoft.EntityFrameworkCore;
using InforceTestReact.Server.Data;
using InforceTestReact.Server.Models;

namespace InforceTestReact.Server.Services
{
    public class UrlService
    {
        private readonly ApplicationDbContext _context;
        private const string BaseUrl = "https://localhost:7264/";

        public UrlService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UrlMappingDto> CreateShortUrlAsync(CreateUrlRequest request, string userId)
        {
            var existingUrl = await _context.UrlMappings
                .FirstOrDefaultAsync(u => u.OriginalUrl == request.OriginalUrl);

            if (existingUrl != null)
            {
                throw new InvalidOperationException("URL already exists");
            }

            var shortCode = GenerateShortCode();

            while (await _context.UrlMappings.AnyAsync(u => u.ShortCode == shortCode))
            {
                shortCode = GenerateShortCode();
            }

            var urlMapping = new UrlMapping
            {
                OriginalUrl = request.OriginalUrl,
                ShortCode = shortCode,
                CreatedById = userId
            };

            _context.UrlMappings.Add(urlMapping);
            await _context.SaveChangesAsync();

            return MapToDto(urlMapping);
        }

        public async Task<IEnumerable<UrlMappingDto>> GetAllUrlsAsync()
        {
            var urls = await _context.UrlMappings
                .Include(u => u.CreatedBy)
                .OrderByDescending(u => u.CreatedDate)
                .ToListAsync();

            return urls.Select(MapToDto);
        }

        public async Task<UrlDetailsDto?> GetUrlDetailsAsync(int id)
        {
            var url = await _context.UrlMappings
                .Include(u => u.CreatedBy)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (url == null) return null;

            return new UrlDetailsDto
            {
                Id = url.Id,
                OriginalUrl = url.OriginalUrl,
                ShortCode = url.ShortCode,
                ShortUrl = BaseUrl + url.ShortCode,
                CreatedBy = url.CreatedBy.UserName ?? "",
                CreatedByFullName = $"{url.CreatedBy.FirstName} {url.CreatedBy.LastName}",
                CreatedDate = url.CreatedDate,
                ClickCount = url.ClickCount,
                LastAccessedDate = url.LastAccessedDate
            };
        }

        public async Task<string?> GetOriginalUrlAsync(string shortCode)
        {
            var url = await _context.UrlMappings
                .FirstOrDefaultAsync(u => u.ShortCode == shortCode);

            return url?.OriginalUrl;
        }

        public async Task<bool> DeleteUrlAsync(int id, string userId, bool isAdmin)
        {
            var url = await _context.UrlMappings.FindAsync(id);
            if (url == null) return false;

            if (!isAdmin && url.CreatedById != userId)
                return false;

            _context.UrlMappings.Remove(url);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task UpdateClickCountAsync(string shortCode)
        {
            var url = await _context.UrlMappings
                .FirstOrDefaultAsync(u => u.ShortCode == shortCode);

            if (url != null)
            {
                url.ClickCount++;
                url.LastAccessedDate = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

        private static string GenerateShortCode()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 6)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private UrlMappingDto MapToDto(UrlMapping url)
        {
            return new UrlMappingDto
            {
                Id = url.Id,
                OriginalUrl = url.OriginalUrl,
                ShortCode = url.ShortCode,
                ShortUrl = BaseUrl + url.ShortCode,
                CreatedBy = url.CreatedBy?.UserName ?? "",
                CreatedDate = url.CreatedDate,
                ClickCount = url.ClickCount,
                LastAccessedDate = url.LastAccessedDate
            };
        }
    }
}