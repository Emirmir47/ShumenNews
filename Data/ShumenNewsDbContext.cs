using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShumenNews.Data.Models;

namespace ShumenNews.Data
{
    public class ShumenNewsDbContext : IdentityDbContext<ShumenNewsUser, IdentityRole, string>
    {
        public ShumenNewsDbContext(DbContextOptions<ShumenNewsDbContext> options)
            : base(options)
        {
            this.Database.Migrate();
        }
        public DbSet<ShumenNewsCategory> Categories { get; set; }
        public DbSet<ShumenNewsArticle> Articles { get; set; }
        public DbSet<ShumenNewsComment> Comments { get; set; }
        public DbSet<ShumenNewsImage> Images { get; set; } 
        public DbSet<ShumenNewsUserArticle> UserArticles { get; set; }
    }
}
