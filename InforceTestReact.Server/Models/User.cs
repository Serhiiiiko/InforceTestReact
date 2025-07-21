using Microsoft.AspNetCore.Identity;

namespace InforceTestReact.Server.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public ICollection<UrlMapping> UrlMappings { get; set; } = new List<UrlMapping>();
    }
}