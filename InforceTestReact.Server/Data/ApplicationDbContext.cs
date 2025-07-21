using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using InforceTestReact.Server.Models;

namespace InforceTestReact.Server.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<UrlMapping> UrlMappings { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UrlMapping>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.OriginalUrl).IsRequired().HasMaxLength(2000);
                entity.Property(e => e.ShortCode).IsRequired().HasMaxLength(10);
                entity.HasIndex(e => e.ShortCode).IsUnique();
                entity.HasIndex(e => e.OriginalUrl);

                entity.HasOne(e => e.CreatedBy)
                      .WithMany(u => u.UrlMappings)
                      .HasForeignKey(e => e.CreatedById)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}