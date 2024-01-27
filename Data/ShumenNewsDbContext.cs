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
        public DbSet<ShumenNewsArticle> Articles { get; set; }
        public DbSet<ShumenNewsComment> Comments { get; set; }
        public DbSet<ShumenNewsImage> Images { get; set; }
        public DbSet<ShumenNewsUserArticle> UserArticles { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ShumenNewsUserArticle>().HasOne(ua => ua.User).WithMany(ua => ua.UserArticles).OnDelete(DeleteBehavior.NoAction);
                //builder.Entity<ShumenNewsUserArticle>().HasOne(u => u.User).WithMany(a => a.UserArticles).OnDelete(DeleteBehavior.NoAction);
            base.OnModelCreating(builder);
        }
    }
}
