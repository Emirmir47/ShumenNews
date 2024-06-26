﻿using Humanizer;
using Microsoft.AspNetCore.Identity;
using NuGet.Packaging.Signing;
using NuGet.Protocol;
using ShumenNews.Data.Models;
using System.Configuration;
using System.Globalization;
using System.Reflection.Metadata;
using System.Text.Json;

namespace ShumenNews.Data.Seeding
{
    public class ShumenNewsSeeder : ISeeder
    {
        private readonly ShumenNewsDbContext db;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ShumenNewsUser> userManager;

        public ShumenNewsSeeder(ShumenNewsDbContext db,
            RoleManager<IdentityRole> roleManager,
            UserManager<ShumenNewsUser> userManager)
        {
            this.db = db;
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        public async Task Seed()
        {
            if (!db.Categories.Any())
            {
                //Roles
                var adminRole = new IdentityRole
                {
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                };
                var moderatorRole = new IdentityRole
                {
                    Name = "Moderator",
                    NormalizedName = "MODERATOR"
                };
                var authorRole = new IdentityRole
                {
                    Name = "Author",
                    NormalizedName = "AUTHOR"
                };
                await roleManager.CreateAsync(adminRole);
                await roleManager.CreateAsync(moderatorRole);
                await roleManager.CreateAsync(authorRole);

                //Root user
                var rootUser = new ShumenNewsUser
                {
                    UserName = "root",
                    NormalizedUserName = "ROOT",
                    Email = "root@shumen_news.com",
                    NormalizedEmail = "ROOT@SHUMEN_NEWS.COM",
                    FirstName = "Root",
                    LastName = "Root",
                    EmailConfirmed = true
                };
                await db.Users.AddAsync(rootUser);

                //Categories
                List<ShumenNewsCategory> categories = new List<ShumenNewsCategory>();
                var categoriesPath = Path.Combine(Directory.GetCurrentDirectory());
                using (StreamReader r = new StreamReader($"{categoriesPath}\\Data\\Seeding\\categories.json"))
                {
                    string json = r.ReadToEnd();
                    categories = JsonSerializer.Deserialize<List<ShumenNewsCategory>>(json)!;
                }
                foreach (var category in categories)
                { db.Categories.Add(category); }

                //Articles
                List<ShumenNewsArticle> articles = new List<ShumenNewsArticle>();
                var articlesPath = Path.Combine(Directory.GetCurrentDirectory());
                using (StreamReader r = new StreamReader($"{articlesPath}\\Data\\Seeding\\articles.json"))
                {
                    string json = r.ReadToEnd();
                    articles = JsonSerializer.Deserialize<List<ShumenNewsArticle>>(json)!;
                }
                Random rnd = new Random();
                foreach (var article in articles)
                {
                    article.LikesCount = rnd.Next(0, 2000);
                    article.DislikesCount = rnd.Next(0, 1500);
                    article.ViewsCount = rnd.Next(article.LikesCount, 15000);
                    int day = rnd.Next(1, 28);
                    int month = rnd.Next(1, 12);
                    int year = rnd.Next(2005, DateTime.UtcNow.Year);
                    int hours = rnd.Next(0, 23);
                    int minutes = rnd.Next(0, 59);
                    string datetime = $"{year}/{month}/{day} {hours}:{minutes}:0";
                    article.PublishedOn = DateTime.Parse(datetime);
                    article.Category = categories.Find(c => c.Id == article.CategoryId)!;
                    db.Articles.Add(article);
                }
                //var article1 = new ShumenNewsArticle
                //{
                //    Title = "Text", //From Json
                //    Content = "Text", //From Json
                //    LikesCount = 0, //Random
                //    DislikesCount = 0, //Random
                //    PublishedOn = DateTime.UtcNow, //Random
                //    ViewsCount = 0, //Random
                //    MainImageId = "seederImg1", //For
                //    CategoryId = "Text", //From Json
                //    Category = null //category //For
                //};

                int articleCount = 0;
                List<int> indexes = new List<int>();
                List<ShumenNewsImage> images = new List<ShumenNewsImage>();
                for (int i = 1; i <= 168; i++)
                {
                    ShumenNewsImage image = new ShumenNewsImage
                    {
                        Id = $"seederImg{i}",
                        Extension = "jpg",
                        Article = articles[articleCount],
                    };
                    images.Add(image);
                    db.Images.Add(image);
                    //Аrithmetic progression
                    if (i == 3)
                    {
                        indexes.Add(i);
                        articles[articleCount].MainImageId = $"seederImg{i - 2}";
                        articleCount++;
                    }
                    for (int j = 0; j < indexes.Count(); j++)
                    {
                        if (i - indexes[j] == 3 && i != 168)
                        {
                            indexes.Add(i);
                            articles[articleCount].MainImageId = $"seederImg{i - 2}";
                            articleCount++;
                        }
                        else if (i == 168)
                        {
                            articles[articleCount].MainImageId = $"seederImg{i - 2}";
                        }
                    }
                }
                //Images
                //ShumenNewsImage image1 = new ShumenNewsImage
                //{
                //    Id = "seederImg1",
                //    Extension = "jpg",
                //    Article = article1
                //};


                //Authors
                var author = new ShumenNewsUser
                {
                    UserName = "ivanpetrov10@gmail.com",
                    NormalizedUserName = "IVANPETROV@GMAIL.COM",
                    Email = "ivanpetrov10@gmail.com",
                    NormalizedEmail = "IVANPETROV@GMAIL.COM",
                    FirstName = "Иван",
                    LastName = "Петров",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(author, "123456");
                await userManager.AddToRoleAsync(author, authorRole.Name);

                var author2 = new ShumenNewsUser
                {
                    UserName = "mihail8elenkov@gmail.com",
                    NormalizedUserName = "MIHAIL8ELENKOV@GMAIL.COM",
                    Email = "mihail8elenkov@gmail.com",
                    NormalizedEmail = "MIHAIL8ELENKOV@GMAIL.COM",
                    FirstName = "Михаил",
                    LastName = "Еленков",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(author2, "123456");
                await userManager.AddToRoleAsync(author2, authorRole.Name);

                var author3 = new ShumenNewsUser
                {
                    UserName = "eli34@gmail.com",
                    NormalizedUserName = "ELI34@GMAIL.COM",
                    Email = "eli34@gmail.com",
                    NormalizedEmail = "ELI34@GMAIL.COM",
                    FirstName = "Ели",
                    LastName = "Недялкова",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(author3, "123456");
                await userManager.AddToRoleAsync(author3, authorRole.Name);

                var author4 = new ShumenNewsUser
                {
                    UserName = "ivi677@gmail.com",
                    NormalizedUserName = "IVI677@GMAIL.COM",
                    Email = "ivi677@gmail.com",
                    NormalizedEmail = "IVI677@GMAIL.COM",
                    FirstName = "Петя",
                    LastName = "Иванова",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(author4, "123456");
                await userManager.AddToRoleAsync(author4, authorRole.Name);

                var author5 = new ShumenNewsUser
                {
                    UserName = "emilian233@gmail.com",
                    NormalizedUserName = "EMILIAN233@GMAIL.COM",
                    Email = "emilian233@gmail.com",
                    NormalizedEmail = "EMILIAN233@GMAIL.COM",
                    FirstName = "Емилиян",
                    LastName = "Петров",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(author5, "123456");
                await userManager.AddToRoleAsync(author5, authorRole.Name);

                var author6 = new ShumenNewsUser
                {
                    UserName = "vladi43@gmail.com",
                    NormalizedUserName = "VALDIMIR21@GMAIL.COM",
                    Email = "vladi21@gmail.com",
                    NormalizedEmail = "VLADI21@GMAIL.COM",
                    FirstName = "Владимир",
                    LastName = "Пенков",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(author6, "123456");
                await userManager.AddToRoleAsync(author6, authorRole.Name);

                var author7 = new ShumenNewsUser
                {
                    UserName = "petrovcho12@gmail.com",
                    NormalizedUserName = "PETROVCHO12@GMAIL.COM",
                    Email = "golemia12@gmail.com",
                    NormalizedEmail = "GOLEMIA12@GMAIL.COM",
                    FirstName = "Петров",
                    LastName = "Пенков",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(author7, "123456");
                await userManager.AddToRoleAsync(author7, authorRole.Name);

                var author8 = new ShumenNewsUser
                {
                    UserName = "mityoochite51@gmail.com",
                    NormalizedUserName = "MITYOOCHITE51@GMAIL.COM",
                    Email = "mitlo51@gmail.com",
                    NormalizedEmail = "MITKO51@GMAIL.COM",
                    FirstName = "Димитър",
                    LastName = "Владимиров",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(author8, "123456");
                await userManager.AddToRoleAsync(author8, authorRole.Name);

                var author9 = new ShumenNewsUser
                {
                    UserName = "pesho81@gmail.com",
                    NormalizedUserName = "PESHO81@GMAIL.COM",
                    Email = "pesho81@gmail.com",
                    NormalizedEmail = "PESHO81@GMAIL.COM",
                    FirstName = "Петър",
                    LastName = "Петров",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(author9, "123456");
                await userManager.AddToRoleAsync(author9, authorRole.Name);

                var author10 = new ShumenNewsUser
                {
                    UserName = "galin21@gmail.com",
                    NormalizedUserName = "GALIN21@GMAIL.COM",
                    Email = "galin21@gmail.com",
                    NormalizedEmail = "GALIN21@GMAIL.COM",
                    FirstName = "Гален",
                    LastName = "Галнов",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(author10, "123456");
                await userManager.AddToRoleAsync(author10, authorRole.Name);

                var author11 = new ShumenNewsUser
                {
                    UserName = "todor21@gmail.com",
                    NormalizedUserName = "TODOR21GMAIL.COM",
                    Email = "todor21@gmail.com",
                    NormalizedEmail = "TODOR21@GMAIL.COM",
                    FirstName = "Тодор",
                    LastName = "Тодоров",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(author11, "123456");
                await userManager.AddToRoleAsync(author11, authorRole.Name);

                var author12 = new ShumenNewsUser
                {
                    UserName = "viki31@gmail.com",
                    NormalizedUserName = "VIKI31@GMAIL.COM",
                    Email = "viki31@gmail.com",
                    NormalizedEmail = "VIKI31@GMAIL.COM",
                    FirstName = "Виктория",
                    LastName = "Петрова",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(author12, "123456");
                await userManager.AddToRoleAsync(author12, authorRole.Name);

                //Moderators
                var moderator = new ShumenNewsUser
                {
                    UserName = "nikolaipetrov@gmail.com",
                    NormalizedUserName = "NIKOLAIPETROV@GMAIL.COM",
                    Email = "nikolaipetrov@gmail.com",
                    NormalizedEmail = "NIKOLAIPETROV@GMAIL.COM",
                    FirstName = "Николай",
                    LastName = "Петров",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(moderator, "123456");
                await userManager.AddToRoleAsync(moderator, moderatorRole.Name);

                var moderator2 = new ShumenNewsUser
                {
                    UserName = "aleksanderpetrov@gmail.com",
                    NormalizedUserName = "ALEKSANDERPETROV@GMAIL.COM",
                    Email = "aleksanderpetrov@gmail.com",
                    NormalizedEmail = "ALEKSANDERPETROV@GMAIL.COM",
                    FirstName = "Александър",
                    LastName = "Петров",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(moderator2, "123456");
                await userManager.AddToRoleAsync(moderator2, moderatorRole.Name);

                //Ordinary users
                var user = new ShumenNewsUser
                {
                    UserName = "hari28@gmail.com",
                    NormalizedUserName = "HARI28@GMAIL.COM",
                    Email = "hari28@gmail.com",
                    NormalizedEmail = "HARI28@GMAIL.COM",
                    FirstName = "Хари",
                    LastName = "Добрев",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(user, "123456");

                var user2 = new ShumenNewsUser
                {
                    UserName = "hristina1@gmail.com",
                    NormalizedUserName = "HRISTINA1@GMAIL.COM",
                    Email = "hristina1@gmail.com",
                    NormalizedEmail = "HRISTINA1@GMAIL.COM",
                    FirstName = "Христина",
                    LastName = "Добромирова",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(user2, "123456");

                //Commenting users
                var commenter1 = new ShumenNewsUser
                {
                    UserName = "god@gmail.com",
                    NormalizedUserName = "GOD@GMAIL.COM",
                    Email = "god@gmail.com",
                    NormalizedEmail = "GOD@GMAIL.COM",
                    FirstName = "Гаврил",
                    LastName = "Гаврилов",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(commenter1, "123456");

                var commenter2 = new ShumenNewsUser
                {
                    UserName = "danieil01@gmail.com",
                    NormalizedUserName = "DANIEL01@GMAIL.COM",
                    Email = "daniel01@gmail.com",
                    NormalizedEmail = "DANIEL01@GMAIL.COM",
                    FirstName = "Даниел",
                    LastName = "Димитров",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(commenter2, "123456");

                var commenter3 = new ShumenNewsUser
                {
                    UserName = "rosen55@gmail.com",
                    NormalizedUserName = "ROSEN55@GMAIL.COM",
                    Email = "rosen55@gmail.com",
                    NormalizedEmail = "ROSEN55@GMAIL.COM",
                    FirstName = "Росен",
                    LastName = "Тодоров",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(commenter3, "123456");

                var commenter4 = new ShumenNewsUser
                {
                    UserName = "svetlana99@gmail.com",
                    NormalizedUserName = "SVETLANA99@GMAIL.COM",
                    Email = "svetlana99@gmail.com",
                    NormalizedEmail = "svetlana99@GMAIL.COM",
                    FirstName = "Светлана",
                    LastName = "Михайлова",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(commenter4, "123456");

                var commenter5 = new ShumenNewsUser
                {
                    UserName = "raq96@gmail.com",
                    NormalizedUserName = "RAQ96@GMAIL.COM",
                    Email = "raq96@gmail.com",
                    NormalizedEmail = "RAQ96@GMAIL.COM",
                    FirstName = "Рая",
                    LastName = "Радославова",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(commenter5, "123456");

                var commenter6 = new ShumenNewsUser
                {
                    UserName = "atanas21@gmail.com",
                    NormalizedUserName = "ATANAS21@GMAIL.COM",
                    Email = "atanas21@gmail.com",
                    NormalizedEmail = "ATANAS21@GMAIL.COM",
                    FirstName = "Атанас",
                    LastName = "Георгиев",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(commenter6, "123456");

                var commenter7 = new ShumenNewsUser
                {
                    UserName = "nikolay76@gmail.com",
                    NormalizedUserName = "NIKOLAY76@GMAIL.COM",
                    Email = "nikolay76@gmail.com",
                    NormalizedEmail = "NIKOLAY76@GMAIL.COM",
                    FirstName = "Николай",
                    LastName = "Николов",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(commenter7, "123456");

                var commenter8 = new ShumenNewsUser
                {
                    UserName = "angel@gmail.com",
                    NormalizedUserName = "ANGEL@GMAIL.COM",
                    Email = "angel@gmail.com",
                    NormalizedEmail = "ANGEL@GMAIL.COM",
                    FirstName = "Ангел",
                    LastName = "Григотов",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(commenter8, "123456");

                var commenter9 = new ShumenNewsUser
                {
                    UserName = "marin55@gmail.com",
                    NormalizedUserName = "MARIN55@GMAIL.COM",
                    Email = "marin55@gmail.com",
                    NormalizedEmail = "MARIN55@GMAIL.COM",
                    FirstName = "Марин",
                    LastName = "Маринов",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(commenter9, "123456");

                var commenter10 = new ShumenNewsUser
                {
                    UserName = "gabriela6@gmail.com",
                    NormalizedUserName = "GABRIELA6@GMAIL.COM",
                    Email = "gabriela6@gmail.com",
                    NormalizedEmail = "GABRIELA6@GMAIL.COM",
                    FirstName = "Габриела",
                    LastName = "Георгиева",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(commenter10, "123456");

                //Comments
                ShumenNewsComment comment1 = new ShumenNewsComment
                {
                    Content = "Това грозде е много полезно наистина!",
                    User = commenter1,
                    Article = articles[35],
                };
                db.Comments.Add(comment1); 
                
                ShumenNewsComment comment2 = new ShumenNewsComment
                {
                    Content = "Колко килограма грозде ядете?",
                    User = commenter1,
                    Article = articles[35],
                };
                db.Comments.Add(comment2);   
                
                ShumenNewsComment comment3 = new ShumenNewsComment
                {
                    Content = "5 на ден!",
                    User = commenter5,
                    Article = articles[35],
                };
                db.Comments.Add(comment3); 
                
                ShumenNewsComment comment4 = new ShumenNewsComment
                {
                    Content = "От малък мразя вкуса на гроздето.",
                    User = commenter3,
                    Article = articles[35],
                };
                db.Comments.Add(comment4);

                ShumenNewsComment comment5 = new ShumenNewsComment
                {
                    Content = "Защото сме висши същества.",
                    User = commenter4,
                    Article = articles[34],
                };
                db.Comments.Add(comment5);

                ShumenNewsComment comment6 = new ShumenNewsComment
                {
                    Content = "Заради това кучетата имат нощно виждане.",
                    User = commenter7,
                    Article = articles[34],
                };
                db.Comments.Add(comment6);

                ShumenNewsComment comment7 = new ShumenNewsComment
                {
                    Content = "Заради това кучетата имат нощно виждане.",
                    User = commenter7,
                    Article = articles[34],
                };
                db.Comments.Add(comment7);
                  
                //UserArticles
                List<ShumenNewsUserArticle> userArticles = new List<ShumenNewsUserArticle>
                {
                    new ShumenNewsUserArticle
                {
                    User = author,
                    Article = articles[0],
                    IsAuthor = true
                },
                    new ShumenNewsUserArticle
                {
                    User = author2,
                    Article = articles[1],
                    IsAuthor = true
                },
                    new ShumenNewsUserArticle
                {
                    User = author,
                    Article = articles[2],
                    IsAuthor = true
                },
                    new ShumenNewsUserArticle
                {
                    User = author3,
                    Article = articles[3],
                    IsAuthor = true
                },
                    new ShumenNewsUserArticle
                {
                    User = author2,
                    Article = articles[4],
                    IsAuthor = true
                },
                    new ShumenNewsUserArticle
                {
                    User = author,
                    Article = articles[5],
                    IsAuthor = true
                },
                    new ShumenNewsUserArticle
                {
                    User = author2,
                    Article = articles[6],
                    IsAuthor = true
                },
                    new ShumenNewsUserArticle
                {
                    User = author3,
                    Article = articles[7],
                    IsAuthor = true
                },
                    new ShumenNewsUserArticle
                {
                    User = author,
                    Article = articles[8],
                    IsAuthor = true
                },
                    new ShumenNewsUserArticle
                {
                    User = author3,
                    Article = articles[9],
                    IsAuthor = true
                },
                    new ShumenNewsUserArticle
                {
                    User = author2,
                    Article = articles[10],
                    IsAuthor = true
                },
                    new ShumenNewsUserArticle
                {
                    User = author3,
                    Article = articles[11],
                    IsAuthor = true
                },
                    new ShumenNewsUserArticle
                {
                    User = author4,
                    Article = articles[12],
                    IsAuthor = true
                },
                    new ShumenNewsUserArticle
                {
                    User = author5,
                    Article = articles[13],
                    IsAuthor = true
                },
                    new ShumenNewsUserArticle
                {
                    User = author5,
                    Article = articles[14],
                    IsAuthor = true
                },
                    new ShumenNewsUserArticle
                {
                    User = author4,
                    Article = articles[15],
                    IsAuthor = true
                },
                    new ShumenNewsUserArticle
                {
                    User = author6,
                    Article = articles[16],
                    IsAuthor = true
                },
                    new ShumenNewsUserArticle
                {
                    User = author6,
                    Article = articles[17],
                    IsAuthor = true
                },
                    new ShumenNewsUserArticle
                {
                    User = author5,
                    Article = articles[18],
                    IsAuthor = true
                },
                    new ShumenNewsUserArticle
                {
                    User = author5,
                    Article = articles[19],
                    IsAuthor = true
                },
                    new ShumenNewsUserArticle
                {
                    User = author4,
                    Article = articles[20],
                    IsAuthor = true
                },
                    new ShumenNewsUserArticle
                {
                    User = author4,
                    Article = articles[21],
                    IsAuthor = true
                },
                    new ShumenNewsUserArticle
                {
                    User = author6,
                    Article = articles[22],
                    IsAuthor = true
                },
                    new ShumenNewsUserArticle
                {
                    User = author7,
                    Article = articles[23],
                    IsAuthor = true
                },
                    new ShumenNewsUserArticle
                {
                    User = author6,
                    Article = articles[24],
                    IsAuthor = true
                },
                    new ShumenNewsUserArticle
                {
                    User = author7,
                    Article = articles[25],
                    IsAuthor = true
                },
                    new ShumenNewsUserArticle
                {
                    User = author7,
                    Article = articles[26],
                    IsAuthor = true
                },
                    new ShumenNewsUserArticle
                {
                    User = author7,
                    Article = articles[27],
                    IsAuthor = true
                },
                    new ShumenNewsUserArticle
                {
                    User = author8,
                    Article = articles[28],
                    IsAuthor = true
                },
                    new ShumenNewsUserArticle
                {
                    User = author9,
                    Article = articles[29],
                    IsAuthor = true
                },
                    new ShumenNewsUserArticle
                {
                    User = author9,
                    Article = articles[30],
                    IsAuthor = true
                },
                    new ShumenNewsUserArticle
                {
                    User = author10,
                    Article = articles[31],
                    IsAuthor = true
                },
                    new ShumenNewsUserArticle
                {
                    User = author8,
                    Article = articles[32],
                    IsAuthor = true
                },
                    new ShumenNewsUserArticle
                {
                    User = author8,
                    Article = articles[33],
                    IsAuthor = true
                },
                    new ShumenNewsUserArticle
                {
                    User = author9,
                    Article = articles[34],
                    IsAuthor = true
                },
                    new ShumenNewsUserArticle
                {
                    User = author9,
                    Article = articles[35],
                    IsAuthor = true
                },
                    new ShumenNewsUserArticle
                {
                    User = author10,
                    Article = articles[36],
                    IsAuthor = true
                },
                    new ShumenNewsUserArticle
                {
                    User = author10,
                    Article = articles[37],
                    IsAuthor = true
                },
                    new ShumenNewsUserArticle
                {
                    User = author11,
                    Article = articles[38],
                    IsAuthor = true
                },
                    new ShumenNewsUserArticle
                {
                    User = author12,
                    Article = articles[39],
                    IsAuthor = true
                },
                    new ShumenNewsUserArticle
                {
                    User = author11,
                    Article = articles[40],
                    IsAuthor = true
                },
                    new ShumenNewsUserArticle
                {
                    User = author11,
                    Article = articles[41],
                    IsAuthor = true
                },
                    new ShumenNewsUserArticle
                {
                    User = author12,
                    Article = articles[42],
                    IsAuthor = true
                },
                    new ShumenNewsUserArticle
                {
                    User = author11,
                    Article = articles[43],
                    IsAuthor = true
                },
                    new ShumenNewsUserArticle
                {
                    User = author12,
                    Article = articles[44],
                    IsAuthor = true
                },
                    new ShumenNewsUserArticle
                {
                    User = author12,
                    Article = articles[45],
                    IsAuthor = true
                },
                    new ShumenNewsUserArticle
                {
                    User = author12,
                    Article = articles[46],
                    IsAuthor = true
                },
                    new ShumenNewsUserArticle
                {
                    User = author,
                    Article = articles[47],
                    IsAuthor = true
                },
                    new ShumenNewsUserArticle
                {
                    User = author2,
                    Article = articles[48],
                    IsAuthor = true
                },
                    new ShumenNewsUserArticle
                {
                    User = author3,
                    Article = articles[49],
                    IsAuthor = true
                },
                    new ShumenNewsUserArticle
                {
                    User = author4,
                    Article = articles[50],
                    IsAuthor = true
                },
                    new ShumenNewsUserArticle
                {
                    User = author5,
                    Article = articles[51],
                    IsAuthor = true
                },
                    new ShumenNewsUserArticle
                {
                    User = author6,
                    Article = articles[52],
                    IsAuthor = true
                },
                    new ShumenNewsUserArticle
                {
                    User = author7,
                    Article = articles[53],
                    IsAuthor = true
                },
                    new ShumenNewsUserArticle
                {
                    User = author8,
                    Article = articles[54],
                    IsAuthor = true
                },
                    new ShumenNewsUserArticle
                {
                    User = author9,
                    Article = articles[55],
                    IsAuthor = true
                }
                };
                foreach (var userArticle in userArticles)
                { db.UserArticles.Add(userArticle); }

                //AdminUser
                var adminUser = new ShumenNewsUser
                {
                    UserName = "jamaltreti3@gmail.com",
                    NormalizedUserName = "JAMLTRETI3@GMAIL.COM",
                    Email = "jamaltreti3@gmail.com",
                    NormalizedEmail = "JAMLTRETI3@GMAIL.COM",
                    FirstName = "Hose",
                    LastName = "Jamal",
                    EmailConfirmed = true,
                };
                await userManager.CreateAsync(adminUser, "123456");
                await userManager.AddToRoleAsync(adminUser, adminRole.Name);
                db.SaveChanges();
            }
        }
    }
}
