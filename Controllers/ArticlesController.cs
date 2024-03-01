using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using ShumenNews.Data;
using ShumenNews.Data.Models;
using ShumenNews.Models.BindingModels;
using ShumenNews.Models.ViewModels;
using ShumenNews.Services;

namespace ShumenNews.Controllers
{
    public class ArticlesController : Controller
    {
        private readonly ShumenNewsDbContext db;
        private readonly UserManager<ShumenNewsUser> userManager;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IUserService userService;
        private readonly ICategoryService categoryService;
        private readonly IArticleService articleService;
        private readonly IImageService imageService;

        public ArticlesController(ShumenNewsDbContext db,
            UserManager<ShumenNewsUser> userManager,
            IWebHostEnvironment webHostEnvironment,
            IUserService userService,
            ICategoryService categoryService,
            IArticleService articleService,
            IImageService imageService)
        {
            this.db = db;
            this.userManager = userManager;
            this.webHostEnvironment = webHostEnvironment;
            this.userService = userService;
            this.categoryService = categoryService;
            this.articleService = articleService;
            this.imageService = imageService;
        }
        public IActionResult Index(int id)
        {
            var lastArticleId = articleService.GetLastArticleId();
            if (id > 0 && id <= lastArticleId)
            {
                var article = articleService.GetArticleById(id);
                if (article is not null)
                {
                    var mainImage = imageService.GetArticleMainImageUrl(article.MainImageId, article);

                    var model = new ArticleViewModel
                    {
                        Id = article.Id,
                        Title = article.Title,
                        Content = article.Content,
                        LikesCount = article.LikesCount,
                        DislikesCount = article.DislikesCount,
                        ViewsCount = article.ViewsCount,
                        PublishedOn = article.PublishedOn,
                        MainImage = mainImage,
                        Comments = article.Comments,
                        Images = article.Images.Select(a => a.Url)
                    };
                    if (User.Identity!.IsAuthenticated)
                    {
                        var userArticle = articleService.GetUserArticleAsDTOByUsername(User.Identity.Name!, article);
                        model.UserArticle = userArticle;
                    }
                    return View(model);
                }
            }
            return RedirectToAction("Index", "Home");
        }
        [Authorize]
        [HttpPost]
        public IActionResult GetUserAttitude(ArticleViewModel article)
        {
            if (article is not null)
            {
                if (article.UserArticle is not null)
                {
                    articleService.SetAttitudeToArticle(article);
                }
                else
                {
                    var user = userManager.FindByNameAsync(User.Identity!.Name)
                        .GetAwaiter().GetResult();
                    articleService.CreateUserArticle(article, user);
                }
                return RedirectToAction("Index", "Articles", article.Id); //Връща Home/Index
            }
            return RedirectToAction("Index", article);
        }
        [Authorize]
        [HttpPost]
        public IActionResult GetUserComment(ArticleViewModel article)
        {
            //TODO Comments
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin, Moderator")]
        public IActionResult All()
        {
            var articles = articleService.GetAllArticlesWithShortContent();
            var model = articles.Select(a => new ArticleViewModel
            {
                Id = a.Id,
                Author = a.UserArticles.SingleOrDefault()!.User,
                Category = a.Category,
                Title = a.Title,
                PublishedOn = a.PublishedOn,
                LikesCount = a.LikesCount,
                DislikesCount = a.DislikesCount,
                ViewsCount = a.ViewsCount,
                MainImage = imageService.GetImageUrlById(a.MainImageId)
            }).OrderByDescending(a => a.Id)
                .ToList();
            return View(model);
        }
        [Authorize(Roles = "Admin, Author")]
        public IActionResult Create()
        {
            var categories = categoryService.GetAllCategoriesAsSelectListItem();
            var model = new ArticleCreateBindingModel
            {
                Categories = categories
            };
            return View(model);
        }
        [Authorize(Roles = "Admin, Author")]
        [HttpPost]
        public IActionResult Create(ArticleCreateBindingModel bindingModel)
        {
            if (ModelState.IsValid)
            {
                var category = categoryService.GetCategoryById(bindingModel.CategoryId);
                var article = new ShumenNewsArticle
                {
                    Title = bindingModel.Title,
                    Content = bindingModel.Content,
                    CategoryId = bindingModel.CategoryId,
                    Category = category
                };
                if (bindingModel.Images is not null)
                {
                    List<ShumenNewsImage> images = new List<ShumenNewsImage>();
                    var image = new ShumenNewsImage();
                    foreach (var img in bindingModel.Images)
                    {
                        var extension = Path.GetExtension(img.FileName);
                        var fileName = Guid.NewGuid().ToString();
                        var physicalPath = $"{webHostEnvironment.WebRootPath}/img/{fileName}.{extension}";
                        using (FileStream fs = new FileStream(physicalPath, FileMode.CreateNew))
                        {
                            img.CopyTo(fs);
                        };
                        image.Id = fileName;
                        image.Extension = extension;
                        article.Images.Add(image);
                        images.Add(image);
                        db.Articles.Add(article);
                    }
                    article.MainImageId = images[0].Id;
                    var userName = User.Identity!.Name;
                    var user = userService.GetUserByUserName(userName!);
                    var userArticle = new ShumenNewsUserArticle
                    {
                        User = user,
                        Article = article,
                        IsAuthor = true,
                    };
                    db.UserArticles.Add(userArticle);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                var categories = categoryService.GetAllCategoriesAsSelectListItem();
                bindingModel.Categories = categories;
            }
            return View(bindingModel);
        }
        [Authorize(Roles = "Author")]
        public IActionResult MyArticles()
        {
            var user = db.Users.SingleOrDefault(u => u.Email == User.Identity!.Name);
            var articles = articleService.GetArticlesByAuthor(user!);
            var model = articles.Select(a => new ArticleViewModel
            {
                Id = a.Id,
                Title = a.Title,
                PublishedOn = a.PublishedOn,
                MainImage = imageService.GetImageUrlById(a.MainImageId),
            }).ToList();
            return View(model);
        }
        public IActionResult Details(int id)
        {
            var isAdmin = User.IsInRole("Admin");
            var article = articleService.GetArticleById(id);
            var author = articleService.GetArticleAuthor(article);
            var isArticleAuthor = false;
            if (author is not null)
            {
                isArticleAuthor = User.Identity!.Name == author.UserName;
            }
            if (article is not null)
            {
                if (isAdmin || isArticleAuthor)
                {
                    var model = new ArticleUpdateBindingModel
                    {
                        Id = article.Id,
                        Title = article.Title,
                        Content = article.Content,
                        //LikesCount = article.LikesCount,
                        //DislikesCount = article.DislikesCount,
                        //ViewsCount = article.ViewsCount,
                        //PublishedOn = article.PublishedOn,
                        //MainImage = imageService.GetArticleMainImageUrl(article.MainImageId, article),
                        //Images = article.Images.Select(a => a.Url),
                        //Category = article.Category,
                        IsDeleted = article.IsDeleted,
                        Author = author
                    };
                    return View(model);
                }

            }
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public IActionResult Details(ArticleUpdateBindingModel bindingModel)
        {
            var article = articleService.GetArticleById(bindingModel.Id);
            article.Title = bindingModel.Title;
            if (bindingModel.Content is not null)
            {
                article.Content = bindingModel.Content;
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            var article = articleService.GetArticleById(id);
            article.IsDeleted = true;
            db.SaveChanges();
            return RedirectToAction("All", "Articles");
        }
        public IActionResult Restore(int id)
        {
            var article = articleService.GetArticleById(id);
            article.IsDeleted = false;
            db.SaveChanges();
            return RedirectToAction("All", "Articles");
        }
    }
}
