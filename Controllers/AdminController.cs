using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Areas;
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
            var articles = db.Articles.Select(a => new ArticleViewModel
            {
                Id = a.Id,
                Title = a.Title,
                Content = a.Content,
                LikesCount = a.LikesCount,
                DislikesCount = a.DislikesCount,
                ViewsCount = a.ViewsCount,
                CommentsCount = a.CommentsCount,
                PublishedOn = a.PublishedOn,
                MainImage = imageService.GetArticleMainImageUrl(a.MainImageId, a),
                Images = a.Images.Select(a => a.Url),
                Category = a.Category
            }).ToList();
            var authors = db.Users
                .Where(u => u.UserArticles.Any(ua => ua.IsAuthor == true))
                .Select(u => new UserViewModel
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email
                }).ToList();
            var categories = db.Categories
                .Select(c => new CategoryViewModel
                {
                    Id = c.Id,
                    Name = c.Name
                }).ToList();
            var model = new AdminViewModel
            {
                Authors = authors,
                Categories = categories,
                Articles = articles
            };
            return View(model);
        }
    }
}
