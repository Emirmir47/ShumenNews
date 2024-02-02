using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShumenNews.Data.Models;

namespace ShumenNews.Data
{
    public class ShumenNewsDbContext : IdentityDbContext<AppUser, IdentityRole, string>
    {
        public ShumenNewsDbContext(DbContextOptions<ShumenNewsDbContext> options)
            : base(options)
        {
            this.Database.Migrate();
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Image> Images { get; set; } 
        public DbSet<UserArticleAttitude> UserArticles { get; set; }



        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<UserArticleAttitude>().HasKey(x => new { x.AppUser.Id, x.Article.Id });

            builder.Entity<Article>().HasMany(x => x.Images).WithOne(x => x.Article);


            base.OnModelCreating(builder);
        }
    }
}
