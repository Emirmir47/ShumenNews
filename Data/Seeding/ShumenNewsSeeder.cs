using Microsoft.AspNetCore.Identity;
using ShumenNews.Data.Models;

namespace ShumenNews.Data.Seeding
{
    public class ShumenNewsSeeder : ISeeder
    {
        private readonly ShumenNewsDbContext db;

        public ShumenNewsSeeder(ShumenNewsDbContext db)
        {
            this.db = db;
        }

        public async Task Seed()
        {
            if (!db.Articles.Any())
            {
                //Users
                var user = new ShumenNewsUser
                {
                    UserName = "root",
                    NormalizedUserName = "ROOT",
                    Email = "root@shumen_news.com",
                    NormalizedEmail = "ROOT@SHUMEN_NEWS.COM",
                    FirstName = "Root",
                    LastName = "Root",
                    EmailConfirmed = true
                };
                await db.Users.AddAsync(user);

                //Articles
                var article = new ShumenNewsArticle
                {
                    Title = "Момче блъснат от камион",
                    Content = "",
                    Likes = 150,
                    Dislikes = 10,
                    PublishedOn = DateTime.UtcNow,
                    ViewCounter = 1500,
                    Author = user,
                    MainImageId = "seederImg1"
                };
                db.Articles.Add(article);
                var article2 = new ShumenNewsArticle
                {
                    Title = "Момче блъснат от камион2",
                    Content = "",
                    Likes = 150,
                    Dislikes = 10,
                    PublishedOn = DateTime.UtcNow,
                    ViewCounter = 1500,
                    Author = user,
                    MainImageId = "seederImg1"
                };
                db.Articles.Add(article2);

                //Images
                ShumenNewsImage image = new ShumenNewsImage
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "seederImg1",
                    Extension = "jpg",
                    Article = article
                };
                ShumenNewsImage image2 = new ShumenNewsImage
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "seederImg2",
                    Extension = "jpg",
                    Article = article
                };
                ShumenNewsImage image3 = new ShumenNewsImage
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "seederImg3",
                    Extension = "jpg",
                    Article = article
                };
                db.Images.Add(image);
                db.Images.Add(image2);
                db.Images.Add(image3);

                //Roles
                db.Roles.Add(new IdentityRole
                {
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                });
                db.Roles.Add(new IdentityRole
                {
                    Name = "Moderator",
                    NormalizedName = "MODERATOR"
                });
                db.Roles.Add(new IdentityRole
                {
                    Name = "Author",
                    NormalizedName = "AUTHOR"
                });
                db.SaveChanges();
            }
        }
    }
}