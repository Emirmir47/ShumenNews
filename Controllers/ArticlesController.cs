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
        private readonly IImageService imageService;

        public ArticlesController(ShumenNewsDbContext db,
            IWebHostEnvironment webHostEnvironment,
            IUserService userService,
            IImageService imageService)
        {
            this.db = db;
            this.webHostEnvironment = webHostEnvironment;
            this.userService = userService;
            this.imageService = imageService;
        }
        public IActionResult Index(int id)
        {
            var model = db.Articles
                .Include(a=> a.Images)
                .Select(a => new ArticleViewModel
            {
                Id = a.Id,
                Title = a.Title,
                Content = a.Content,
                Likes = a.Likes,
                Dislikes = a.Dislikes,
                Views = a.Views,
                PublishedOn = a.PublishedOn,
                MainImage = imageService.GetArticleMainImageUrl(a.MainImageId, a),
                Images = imageService.GetAllArticleImageUrls(a)
            }).FirstOrDefault(a=>a.Id == id);
            return View(model);
        }
        [Authorize(Roles = "Admin, Moderator")]
        public IActionResult All()
        {
            var model = db.Articles.Select(a => new ArticleViewModel
            {
                Id = a.Id,
                Title = a.Title,
                PublishedOn = a.PublishedOn,
                Likes = a.Likes,
                Dislikes = a.Dislikes,
                Views = a.Views,
                MainImage = imageService.GetImageUrlById(a.MainImageId)
            }).OrderByDescending(a => a.PublishedOn)
                .ToList();
            return View(model);
        }
        [Authorize(Roles = "Admin, Author")]
        public IActionResult Create()
        {
            return View();
        }
        [Authorize(Roles = "Admin, Author")]
        [HttpPost]
        public IActionResult Create(ArticleCreateBindingModel bindingModel)
        {
            if (ModelState.IsValid)
            {
                var article = new ShumenNewsArticle
                {
                    Title = bindingModel.Title,
                    Content = bindingModel.Content,
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
                else
                {
                    ModelState.AddModelError("images collection is null", "Добавете поне една снимка!");
                }
            }
            return View();
        }
    }
}
