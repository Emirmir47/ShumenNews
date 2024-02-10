using Humanizer;
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
        public ShumenNewsSeeder(ShumenNewsDbContext db)
        {
            this.db = db;
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
                db.Roles.Add(adminRole);
                db.Roles.Add(moderatorRole);
                db.Roles.Add(authorRole);

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
                    UserName = "ivanpetrov10",
                    NormalizedUserName = "IVANPETROV10",
                    Email = "ivanpetrov10@gmail.com",
                    NormalizedEmail = "IVANPETROV@GMAIL.COM",
                    FirstName = "Иван",
                    LastName = "Петров",
                    EmailConfirmed = true
                };
                await db.Users.AddAsync(author);

                var author2 = new ShumenNewsUser
                {
                    UserName = "mihail8elenkov",
                    NormalizedUserName = "MIHAIL8ELENKOV",
                    Email = "mihail8elenkov@gmail.com",
                    NormalizedEmail = "MIHAIL8ELENKOV@GMAIL.COM",
                    FirstName = "Михаил",
                    LastName = "Еленков",
                    EmailConfirmed = true
                };
                await db.Users.AddAsync(author2);

                var author3 = new ShumenNewsUser
                {
                    UserName = "eli34",
                    NormalizedUserName = "ELI34",
                    Email = "eli34@gmail.com",
                    NormalizedEmail = "ELI34@GMAIL.COM",
                    FirstName = "Ели",
                    LastName = "Недялкова",
                    EmailConfirmed = true
                };
                await db.Users.AddAsync(author3);

                var author4 = new ShumenNewsUser
                {
                    UserName = "ivi677",
                    NormalizedUserName = "IVI677",
                    Email = "ivi677@gmail.com",
                    NormalizedEmail = "IVI677@GMAIL.COM",
                    FirstName = "Петя",
                    LastName = "Иванова",
                    EmailConfirmed = true
                };
                await db.Users.AddAsync(author4);

                var author5 = new ShumenNewsUser
                {
                    UserName = "emilian233",
                    NormalizedUserName = "EMILIAN233",
                    Email = "emilian233@gmail.com",
                    NormalizedEmail = "EMILIAN233@GMAIL.COM",
                    FirstName = "Емилиян",
                    LastName = "Петров",
                    EmailConfirmed = true
                };
                await db.Users.AddAsync(author5);

                var author6 = new ShumenNewsUser
                {
                    UserName = "vladi43",
                    NormalizedUserName = "VALDIMIR21",
                    Email = "vladi21@gmail.com",
                    NormalizedEmail = "VLADI21@GMAIL.COM",
                    FirstName = "Владимир",
                    LastName = "Пенков",
                    EmailConfirmed = true
                };
                await db.Users.AddAsync(author6);

                var author7 = new ShumenNewsUser
                {
                    UserName = "petrovcho12",
                    NormalizedUserName = "PETROVCHO12",
                    Email = "golemia12@gmail.com",
                    NormalizedEmail = "GOLEMIA12@GMAIL.COM",
                    FirstName = "Петров",
                    LastName = "Пенков",
                    EmailConfirmed = true
                };
                await db.Users.AddAsync(author7);

                var author8 = new ShumenNewsUser
                {
                    UserName = "mityoochite51",
                    NormalizedUserName = "MITYOOCHITE51",
                    Email = "mitlo51@gmail.com",
                    NormalizedEmail = "MITKO51@GMAIL.COM",
                    FirstName = "Димитър",
                    LastName = "Владимиров",
                    EmailConfirmed = true
                };
                await db.Users.AddAsync(author8);

                var author9 = new ShumenNewsUser
                {
                    UserName = "golemiqpesho",
                    NormalizedUserName = "GOLEMIQPESHO51",
                    Email = "pesho81@gmail.com",
                    NormalizedEmail = "PESHO81@GMAIL.COM",
                    FirstName = "Петър",
                    LastName = "Петров",
                    EmailConfirmed = true
                };
                await db.Users.AddAsync(author9);

                var author10 = new ShumenNewsUser
                {
                    UserName = "galin21",
                    NormalizedUserName = "ГGALIN21",
                    Email = "galin21@gmail.com",
                    NormalizedEmail = "GALIN21@GMAIL.COM",
                    FirstName = "Гален",
                    LastName = "Галнов",
                    EmailConfirmed = true
                };
                await db.Users.AddAsync(author10);

                var author11 = new ShumenNewsUser
                {
                    UserName = "toshkokuratacha",
                    NormalizedUserName = "TOSHKOKURATACHA",
                    Email = "todor21@gmail.com",
                    NormalizedEmail = "TODOR21@GMAIL.COM",
                    FirstName = "Тодор",
                    LastName = "Тодоров",
                    EmailConfirmed = true
                };
                await db.Users.AddAsync(author11);

                var author12 = new ShumenNewsUser
                {
                    UserName = "viktoriq31",
                    NormalizedUserName = "VIKTORIQ31",
                    Email = "viki31@gmail.com",
                    NormalizedEmail = "VIKI31@GMAIL.COM",
                    FirstName = "Виктория",
                    LastName = "Петорва",
                    EmailConfirmed = true
                };
                await db.Users.AddAsync(author12);

                //Ordinary users
                var user = new ShumenNewsUser
                {
                    UserName = "hari28",
                    NormalizedUserName = "HARI28",
                    Email = "hari28@gmail.com",
                    NormalizedEmail = "HARI28@GMAIL.COM",
                    FirstName = "Хари",
                    LastName = "Добрев",
                    EmailConfirmed = true
                };
                await db.Users.AddAsync(user);

                var user2 = new ShumenNewsUser
                {
                    UserName = "hrisi19",
                    NormalizedUserName = "HRISI19",
                    Email = "hristina1@gmail.com",
                    NormalizedEmail = "HRISTINA1@GMAIL.COM",
                    FirstName = "Христина",
                    LastName = "Добромирова",
                    EmailConfirmed = true
                };
                await db.Users.AddAsync(user2);

                //Commenting users
                var commenter1 = new ShumenNewsUser
                {
                    UserName = "gavrilgod",
                    NormalizedUserName = "GAVRILGOD",
                    Email = "god@gmail.com",
                    NormalizedEmail = "GOD@GMAIL.COM",
                    FirstName = "Гаврил",
                    LastName = "Гаврилов",
                    EmailConfirmed = true
                };
                await db.Users.AddAsync(commenter1);

                var commenter2 = new ShumenNewsUser
                {
                    UserName = "dani93",
                    NormalizedUserName = "DANI93",
                    Email = "daniel01@gmail.com",
                    NormalizedEmail = "DANIEL01@GMAIL.COM",
                    FirstName = "Даниел",
                    LastName = "Димитров",
                    EmailConfirmed = true
                };
                await db.Users.AddAsync(commenter2);

                var commenter3 = new ShumenNewsUser
                {
                    UserName = "rosen55",
                    NormalizedUserName = "ROSEN55",
                    Email = "rosen!@gmail.com",
                    NormalizedEmail = "ROSEN!@GMAIL.COM",
                    FirstName = "Росен",
                    LastName = "Тодоров",
                    EmailConfirmed = true
                };
                await db.Users.AddAsync(commenter3);

                var commenter4 = new ShumenNewsUser
                {
                    UserName = "svetlana99",
                    NormalizedUserName = "SVETLANA99",
                    Email = "svetlana99@gmail.com",
                    NormalizedEmail = "svetlana99@GMAIL.COM",
                    FirstName = "Светлана",
                    LastName = "Михайлова",
                    EmailConfirmed = true
                };
                await db.Users.AddAsync(commenter4);

                var commenter5 = new ShumenNewsUser
                {
                    UserName = "raq96",
                    NormalizedUserName = "RAQ96",
                    Email = "raq96@gmail.com",
                    NormalizedEmail = "RAQ96@GMAIL.COM",
                    FirstName = "Рая",
                    LastName = "Радославова",
                    EmailConfirmed = true
                };
                await db.Users.AddAsync(commenter5);

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
                    UserName = "hoseJamal",
                    NormalizedUserName = "HOSEJAMAl",
                    Email = "jamaltreti3@gmail.com",
                    NormalizedEmail = "JAMLTRETI3@GMAIL.COM",
                    FirstName = "Hose",
                    LastName = "Jamal",
                    EmailConfirmed = true,
                };
                await db.Users.AddAsync(adminUser);

                db.SaveChanges();
            }
        }
    }
}