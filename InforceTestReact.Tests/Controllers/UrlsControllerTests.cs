using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using InforceTestReact.Server.Controllers;
using InforceTestReact.Server.Models;
using InforceTestReact.Server.Services;
using Microsoft.AspNetCore.Http;
using InforceTestReact.Tests.Helpers;
using InforceTestReact.Server.Data;

namespace InforceTestReact.Tests.Controllers
{
    public class UrlsControllerTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly UrlService _urlService;
        private readonly UrlsController _controller;
        private readonly User _testUser;
        private const string TestUserId = "test-user-id";

        public UrlsControllerTests()
        {
            _context = TestDbContextFactory.CreateInMemoryDbContext();
            _urlService = new UrlService(_context);
            _controller = new UrlsController(_urlService);

            // Create test user
            _testUser = new User
            {
                Id = TestUserId,
                UserName = "testuser",
                Email = "test@example.com",
                FirstName = "Test",
                LastName = "User"
            };
            _context.Users.Add(_testUser);
            _context.SaveChanges();

            // Setup controller context for authentication
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, TestUserId),
                new(ClaimTypes.Name, "testuser")
            };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var principal = new ClaimsPrincipal(identity);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = principal }
            };
        }

        [Fact]
        public async Task GetAllUrls_ReturnsOkWithUrls()
        {
            // Arrange
            var request1 = new CreateUrlRequest { OriginalUrl = "https://example1.com" };
            var request2 = new CreateUrlRequest { OriginalUrl = "https://example2.com" };

            await _urlService.CreateShortUrlAsync(request1, TestUserId);
            await _urlService.CreateShortUrlAsync(request2, TestUserId);

            // Act
            var result = await _controller.GetAllUrls();

            // Assert
            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            var returnedUrls = okResult.Value.Should().BeAssignableTo<IEnumerable<UrlMappingDto>>().Subject;
            returnedUrls.Should().HaveCount(2);
        }

        [Fact]
        public async Task CreateShortUrl_ValidRequest_ReturnsCreatedResult()
        {
            // Arrange
            var request = new CreateUrlRequest { OriginalUrl = "https://example.com" };

            // Act
            var result = await _controller.CreateShortUrl(request);

            // Assert
            var createdResult = result.Result.Should().BeOfType<CreatedAtActionResult>().Subject;
            var returnedUrl = createdResult.Value.Should().BeOfType<UrlMappingDto>().Subject;
            returnedUrl.OriginalUrl.Should().Be("https://example.com");
            returnedUrl.ShortCode.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task CreateShortUrl_DuplicateUrl_ReturnsBadRequest()
        {
            // Arrange
            var request = new CreateUrlRequest { OriginalUrl = "https://duplicate.com" };

            // Create first URL
            await _controller.CreateShortUrl(request);

            // Act - try to create duplicate
            var result = await _controller.CreateShortUrl(request);

            // Assert
            var badRequestResult = result.Result.Should().BeOfType<BadRequestObjectResult>().Subject;
            badRequestResult.Value.Should().NotBeNull();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}