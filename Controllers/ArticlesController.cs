using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using ShumenNews.Data;
using ShumenNews.Data.Models;
using ShumenNews.Models.BindingModels;
using ShumenNews.Services;

namespace ShumenNews.Controllers
{
    public class ArticlesController : Controller
    {
        private readonly ShumenNewsDbContext db;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IUserService userService;

        public ArticlesController(ShumenNewsDbContext db, IWebHostEnvironment webHostEnvironment, IUserService userService)
        {
            this.db = db;
            this.webHostEnvironment = webHostEnvironment;
            this.userService = userService;
        }
        [Authorize(Roles = "Admin, Moderator")]
        public IActionResult Index()
        {
            return View();
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
                    return Redirect("Index");
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
