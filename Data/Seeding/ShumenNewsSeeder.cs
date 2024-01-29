using Microsoft.AspNetCore.Identity;
using NuGet.Protocol;
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
                var article = new ShumenNewsImages
                {
                    Title = "Момче блъснат от камион",
                    Content = "",
                    Likes = 150,
                    Dislikes = 10,
                    PublishedOn = DateTime.UtcNow,
                    Views = 1500,
                    MainImageId = "seederImg1"
                };
                db.Articles.Add(article);

                var article2 = new ShumenNewsImages
                {
                    Title = "Откраднаха 10 кубика дърва!",
                    Content = "Вчера собственик на склад за дърва докладва, " +
                    "че 10 кубика от стоката му е липсвала. След разследване се разбра, " +
                    "че са били откраднати.",
                    Likes = 2350,
                    Dislikes = 24,
                    PublishedOn = DateTime.UtcNow,
                    Views = 25000,
                    MainImageId = "seederImg4"
                };
                db.Articles.Add(article2);

                //UserArticles
                var userArticle = new ShumenNewsUserArticle
                {
                    User = user,
                    Article = article,
                    IsAuthor = true
                };
                db.UserArticles.Add(userArticle);
                var userArticle2 = new ShumenNewsUserArticle
                {
                    User = user,
                    Article = article2,
                    IsAuthor = true
                };
                db.UserArticles.Add(userArticle2);

                //Images
                ShumenNewsImage image = new ShumenNewsImage
                {
                    Id = "seederImg1",
                    Extension = "jpg",
                    Article = article
                };
                ShumenNewsImage image2 = new ShumenNewsImage
                {
                    Id = "seederImg2",
                    Extension = "jpg",
                    Article = article
                };
                ShumenNewsImage image3 = new ShumenNewsImage
                {
                    Id = "seederImg3",
                    Extension = "jpg",
                    Article = article2
                };
                ShumenNewsImage image4 = new ShumenNewsImage
                {
                    Id = "seederImg4",
                    Extension = "jpg",
                    Article = article2
                };
                db.Images.Add(image);
                db.Images.Add(image2);
                db.Images.Add(image3);
                db.Images.Add(image4);

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