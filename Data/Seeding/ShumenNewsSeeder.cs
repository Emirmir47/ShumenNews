using Microsoft.AspNetCore.Identity;
using ShumenNews.Data.Models;

namespace ShumenNews.Data.Seeding
{
    public class ShumenNewsSeeder
    {
        private readonly ShumenNewsDbContext db;
        //private readonly UserManager<ShumenNewsUser> userManager;

        public ShumenNewsSeeder(ShumenNewsDbContext db/*, UserManager<ShumenNewsUser> userManager*/)
        {
            this.db = db;
            //this.userManager = userManager;
        }

        public async Task Seed()
        {
            if (db.Articles.Any())
            {

                var user = new ShumenNewsUser
                {
                    UserName = "root",
                    Email = "root@eventures.com",
                    FirstName = "Root",
                    LastName = "Root"
                };
                var result = true;
                //await userManager.CreateAsync(user, "root");
                if (result)//result.Successed
                {
                    var article = new ShumenNewsArticle
                    {
                        Title = "Момче блъснат от камион",
                        Content = "",
                        Likes = 150,
                        Dislikes = 10,
                        PublishedOn = DateTime.UtcNow,
                        ViewCounter = 1500,
                        //Author = user,
                        MainImageId = "seederImg1"
                    };
                    db.Articles.Add(article);
                    List<ShumenNewsImage> images = new List<ShumenNewsImage>
                    {
                    new() //image
                    {
                        Name = "seederImg1",
                        Extension = "jpg",
                        Article = article
                    },
                    new() //image
                    {
                        Name = "seederImg2",
                        Extension = "jpg",
                        Article = article
                    },
                    new() //image
                    {
                        Name = "seederImg3",
                        Extension = "jpg",
                        Article = article
                    }};
                    foreach (var image in images)
                    {
                        db.Images.Add(image);
                    }
                    db.SaveChanges();
                }
            }
        }
    }
}
