namespace InforceTestReact.Server.Models
{
    public class UrlMapping
    {
        public int Id { get; set; }
        public string OriginalUrl { get; set; } = string.Empty;
        public string ShortCode { get; set; } = string.Empty;
        public string CreatedById { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public int ClickCount { get; set; } = 0;
        public DateTime? LastAccessedDate { get; set; }

        // Navigation properties
        public User CreatedBy { get; set; } = null!;
    }
}