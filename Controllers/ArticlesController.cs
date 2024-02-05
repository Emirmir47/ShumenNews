using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IUserService userService;
        private readonly ICategoryService categoryService;
        private readonly IArticleService articleService;
        private readonly IImageService imageService;

        public ArticlesController(ShumenNewsDbContext db,
            IWebHostEnvironment webHostEnvironment,
            IUserService userService,
            ICategoryService categoryService,
            IArticleService articleService,
            IImageService imageService)
        {
            this.db = db;
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
                var model = db.Articles
                    .Include(a => a.Images)
                    .Select(a => new ArticleViewModel
                    {
                        Id = a.Id,
                        Title = a.Title,
                        Content = a.Content,
                        LikesCount = a.LikesCount,
                        DislikesCount = a.DislikesCount,
                        ViewsCount = a.ViewsCount,
                        PublishedOn = a.PublishedOn,
                        MainImage = imageService.GetArticleMainImageUrl(a.MainImageId, a),
                        Images = imageService.GetAllArticleImageUrls(a)
                    }).FirstOrDefault(a => a.Id == id);
                return View(model);
            }
            return RedirectToAction("Index", "Home");
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
            var categories = categoryService.GetAllCategories();
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
                var categories = categoryService.GetAllCategories();
                bindingModel.Categories = categories;
            }
            return View(bindingModel);
        }
    }
}
