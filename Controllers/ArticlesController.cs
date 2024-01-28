using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShumenNews.Data;
using ShumenNews.Data.Models;
using ShumenNews.Models.BindingModels;

namespace ShumenNews.Controllers
{
    public class ArticlesController : Controller
    {
        private readonly ShumenNewsDbContext db;
        private readonly IWebHostEnvironment webHostEnvironment;

        public ArticlesController(ShumenNewsDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            this.db = db;
            this.webHostEnvironment = webHostEnvironment;
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
                    AuthorId = bindingModel.AuthorId,
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
                    article.MainImageId = images[1].Id;
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
