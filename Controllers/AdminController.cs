using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShumenNews.Data;
using ShumenNews.Data.Models;
using ShumenNews.Models.BindingModels;
using ShumenNews.Models.ViewModels;
using ShumenNews.Services;

namespace ShumenNews.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ShumenNewsDbContext db;
        private readonly IImageService imageService;
        private readonly IArticleService articleService;

        public AdminController(ShumenNewsDbContext db, IImageService imageService, IArticleService articleService)
        {
            this.db = db;
            this.imageService = imageService;
            this.articleService = articleService;
        }
        public IActionResult Index()
        {
            var model = db.Articles.Select(a => new ArticleViewModel
            {
                Id = a.Id,
                Title = a.Title,
                Content = a.Content,
                LikesCount = a.LikesCount,
                DislikesCount = a.DislikesCount,
                ViewsCount = a.ViewsCount,
                PublishedOn = a.PublishedOn,
                MainImage = imageService.GetArticleMainImageUrl(a.MainImageId, a),
                Images = a.Images.Select(a=>a.Url),
                Category = a.Category
            }).ToList();
            return View(model);
        }
        public IActionResult Details(int id)
        {
            var article = articleService.GetArticleById(id);
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
                Author = articleService.GetArticleAuthor(article)
            };
            return View(model);
        }
        [HttpPost]
        public IActionResult Details(ArticleUpdateBindingModel bindingModel)
        { 
            var article = articleService.GetArticleById(bindingModel.Id);
            article.Title = bindingModel.Title;
            article.Content = bindingModel.Content;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        }
}
