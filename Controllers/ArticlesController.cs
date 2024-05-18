using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
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
                if (article is not null && article.IsDeleted == false)
                {
                    articleService.AddAViewToArticle(article);
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SearchArticle(string words)
        {
            if (words != null)
            {
                var serviceId = articleService.GetServiceIdBySearchingWords(words);
                if (serviceId != 0)
                {
                    return RedirectToAction("Index", new { id = serviceId});
                }
            }
            return RedirectToAction("NoResults");
        }
        public IActionResult NoResults()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
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
                return RedirectToAction("Index", "Articles", new { id = article.Id });
            }
            return RedirectToAction("Index", article);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult GetUserComment(ArticleViewModel articleViewModel)
        {
            if (articleViewModel.UserComment.Content != null)
            {
                var user = userManager.FindByNameAsync(User.Identity?.Name)
                                .GetAwaiter().GetResult();
                var article = db.Articles.FirstOrDefault(a => a.Id == articleViewModel.Id);
                var userId = user.Id;
                var comment = new ShumenNewsComment
                {
                    Content = articleViewModel.UserComment.Content,
                    Article = article,
                    User = user
                };
                db.Comments.Add(comment);
                db.SaveChanges();
                return RedirectToAction("Index", "Articles", new { id = articleViewModel.Id });
            }
            return RedirectToAction("Index", "Articles", new { id = articleViewModel.Id });
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RemoveUserComment(ArticleViewModel articleViewModel)
        {
            var user = userManager.FindByNameAsync(User.Identity?.Name)
                .GetAwaiter().GetResult();
            var article = db.Articles.FirstOrDefault(a => a.Id == articleViewModel.Id);
            var comment = new ShumenNewsComment();
            if (user is not null)
            {
                if (User.IsInRole("Admin") || User.IsInRole("Moderator"))
                {
                    comment = db.Comments
                        .FirstOrDefault(c => c.Id == articleViewModel.UserComment.Id);
                }
                else
                {
                    comment = db.Comments
                        .FirstOrDefault(c => c.Id == articleViewModel.UserComment.Id
                                        && c.UserId == user.Id);
                }

            }
            if (comment is not null && article is not null)
            {
                comment.IsDeleted = true;
                article!.CommentsCount--;
                db.SaveChanges();
            }
            return RedirectToAction("Index", new { id = articleViewModel.Id });
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
        [ValidateAntiForgeryToken]
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

                    foreach (var img in bindingModel.Images)
                    {
                        var extension = Path.GetExtension(img.FileName);
                        var fileName = Guid.NewGuid().ToString();
                        var physicalPath = $"{webHostEnvironment.WebRootPath}/img/{fileName}.{extension}";
                        using (FileStream fs = new FileStream(physicalPath, FileMode.CreateNew))
                        {
                            img.CopyTo(fs);
                        };
                        var image = new ShumenNewsImage
                        {
                            Id = fileName,
                            Extension = extension
                        };
                        images.Add(image);
                    }
                    article.Images.AddRange(images);
                    article.MainImageId = images[0].Id;
                    db.Articles.Add(article);
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
        [Authorize(Roles = "Admin, Author")]
        public IActionResult Details(int id)
        {
            var isAdmin = User.IsInRole("Admin");
            var article = articleService.GetArticleById(id);
            var categories = categoryService.GetAllCategoriesAsSelectListItem();
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
                        PublishedOn = article.PublishedOn,
                        Images = article.Images.Select(a => a.Url),
                        CategoryId = article.Category.Id,
                        Categories = categories,
                        IsDeleted = article.IsDeleted,
                        Author = author!
                    };
                    return View(model);
                }
            }
            return RedirectToAction("Index", "Home");
        }
        [Authorize(Roles = "Admin, Author")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Details(ArticleUpdateBindingModel bindingModel)
        {
            var article = articleService.GetArticleById(bindingModel.Id);
            if (article is not null)
            {
                article.Title = bindingModel.Title;
                if (bindingModel.Content is not null)
                {
                    article.Content = bindingModel.Content;
                }
                article.PublishedOn = new DateTime(
                    bindingModel.PublishedOn.Year, bindingModel.PublishedOn.Month, bindingModel.PublishedOn.Day,
                    article.PublishedOn.Hour, article.PublishedOn.Minute, article.PublishedOn.Second);
                article.CategoryId = bindingModel.CategoryId;
                db.SaveChanges();
                if (User.IsInRole("Author"))
                {
                    return RedirectToAction("MyArticles");
                }
                else if (User.IsInRole("Admin"))
                {
                    return RedirectToAction("Index", "Admin");
                }
            }
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var article = articleService.GetArticleById(id);
            article.IsDeleted = true;
            db.SaveChanges();
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Admin");
            }
            return RedirectToAction("All", "Articles");
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Restore(int id)
        {
            var article = articleService.GetArticleById(id);
            article.IsDeleted = false;
            db.SaveChanges();
            return RedirectToAction("Index", "Admin");
        }
    }
}
