namespace InforceTestReact.Server.Models
{
    public class LoginRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
    }

    public class CreateUrlRequest
    {
        public string OriginalUrl { get; set; } = string.Empty;
    }

    public class UrlMappingDto
    {
        public int Id { get; set; }
        public string OriginalUrl { get; set; } = string.Empty;
        public string ShortCode { get; set; } = string.Empty;
        public string ShortUrl { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public int ClickCount { get; set; }
        public DateTime? LastAccessedDate { get; set; }
    }

    public class UrlDetailsDto : UrlMappingDto
    {
        public string CreatedByFullName { get; set; } = string.Empty;
    }
}