namespace InforceTestReact.Server.Models
{
    public class About
    {
        public int Id { get; set; }
        public string Content { get; set; } = "URL Shortener Algorithm: This application uses a simple random character generation approach to create short codes. Each URL is assigned a unique 6-character code using a combination of uppercase letters, lowercase letters, and numbers. The system checks for duplicates to ensure uniqueness.";
        public DateTime LastModified { get; set; } = DateTime.UtcNow;
        public string ModifiedById { get; set; } = string.Empty;

        // Navigation property
        public User? ModifiedBy { get; set; }
    }
}