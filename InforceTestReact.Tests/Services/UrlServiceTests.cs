using FluentAssertions;
using InforceTestReact.Server.Data;
using InforceTestReact.Server.Models;
using InforceTestReact.Server.Services;
using InforceTestReact.Tests.Helpers;

namespace InforceTestReact.Tests.Services
{
    public class UrlServiceTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly UrlService _urlService;
        private readonly User _testUser;

        public UrlServiceTests()
        {
            _context = TestDbContextFactory.CreateInMemoryDbContext();
            _urlService = new UrlService(_context);

            // Create test user
            _testUser = new User
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "testuser",
                Email = "test@example.com",
                FirstName = "Test",
                LastName = "User"
            };
            _context.Users.Add(_testUser);
            _context.SaveChanges();
        }

        [Fact]
        public async Task CreateShortUrlAsync_ValidUrl_ReturnsUrlMappingDto()
        {
            // Arrange
            var request = new CreateUrlRequest
            {
                OriginalUrl = "https://example.com"
            };

            // Act
            var result = await _urlService.CreateShortUrlAsync(request, _testUser.Id);

            // Assert
            result.Should().NotBeNull();
            result.OriginalUrl.Should().Be("https://example.com");
            result.ShortCode.Should().NotBeNullOrEmpty();
            result.ShortCode.Length.Should().Be(6);
            result.CreatedBy.Should().Be(_testUser.UserName);
        }

        [Fact]
        public async Task CreateShortUrlAsync_DuplicateUrl_ThrowsInvalidOperationException()
        {
            // Arrange
            var originalUrl = "https://duplicate.com";
            var request1 = new CreateUrlRequest { OriginalUrl = originalUrl };
            var request2 = new CreateUrlRequest { OriginalUrl = originalUrl };

            await _urlService.CreateShortUrlAsync(request1, _testUser.Id);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _urlService.CreateShortUrlAsync(request2, _testUser.Id));

            exception.Message.Should().Be("URL already exists");
        }

        [Fact]
        public async Task GetAllUrlsAsync_WithUrls_ReturnsAllUrls()
        {
            // Arrange
            var request1 = new CreateUrlRequest { OriginalUrl = "https://example1.com" };
            var request2 = new CreateUrlRequest { OriginalUrl = "https://example2.com" };

            await _urlService.CreateShortUrlAsync(request1, _testUser.Id);
            await _urlService.CreateShortUrlAsync(request2, _testUser.Id);

            // Act
            var result = await _urlService.GetAllUrlsAsync();

            // Assert
            result.Should().HaveCount(2);
            result.Should().Contain(u => u.OriginalUrl == "https://example1.com");
            result.Should().Contain(u => u.OriginalUrl == "https://example2.com");
        }

        [Fact]
        public async Task GetOriginalUrlAsync_ValidShortCode_ReturnsOriginalUrl()
        {
            // Arrange
            var request = new CreateUrlRequest { OriginalUrl = "https://example.com" };
            var created = await _urlService.CreateShortUrlAsync(request, _testUser.Id);

            // Act
            var result = await _urlService.GetOriginalUrlAsync(created.ShortCode);

            // Assert
            result.Should().Be("https://example.com");
        }

        [Fact]
        public async Task GetOriginalUrlAsync_InvalidShortCode_ReturnsNull()
        {
            // Act
            var result = await _urlService.GetOriginalUrlAsync("INVALID");

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task DeleteUrlAsync_UserOwnsUrl_ReturnsTrue()
        {
            // Arrange
            var request = new CreateUrlRequest { OriginalUrl = "https://example.com" };
            var created = await _urlService.CreateShortUrlAsync(request, _testUser.Id);

            // Act
            var result = await _urlService.DeleteUrlAsync(created.Id, _testUser.Id, false);

            // Assert
            result.Should().BeTrue();

            var deletedUrl = await _urlService.GetOriginalUrlAsync(created.ShortCode);
            deletedUrl.Should().BeNull();
        }

        [Fact]
        public async Task DeleteUrlAsync_UserDoesNotOwnUrl_ReturnsFalse()
        {
            // Arrange
            var request = new CreateUrlRequest { OriginalUrl = "https://example.com" };
            var created = await _urlService.CreateShortUrlAsync(request, _testUser.Id);
            var differentUserId = Guid.NewGuid().ToString();

            // Act
            var result = await _urlService.DeleteUrlAsync(created.Id, differentUserId, false);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task DeleteUrlAsync_AdminUser_ReturnsTrue()
        {
            // Arrange
            var request = new CreateUrlRequest { OriginalUrl = "https://example.com" };
            var created = await _urlService.CreateShortUrlAsync(request, _testUser.Id);
            var adminUserId = Guid.NewGuid().ToString();

            // Act
            var result = await _urlService.DeleteUrlAsync(created.Id, adminUserId, true);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task UpdateClickCountAsync_ValidShortCode_IncrementsCounter()
        {
            // Arrange
            var request = new CreateUrlRequest { OriginalUrl = "https://example.com" };
            var created = await _urlService.CreateShortUrlAsync(request, _testUser.Id);

            // Act
            await _urlService.UpdateClickCountAsync(created.ShortCode);

            // Assert
            var details = await _urlService.GetUrlDetailsAsync(created.Id);
            details.Should().NotBeNull();
            details!.ClickCount.Should().Be(1);
            details.LastAccessedDate.Should().NotBeNull();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}