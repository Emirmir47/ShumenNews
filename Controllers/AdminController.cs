using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShumenNews.Data;
using ShumenNews.Models.ViewModels;
using ShumenNews.Services;

namespace ShumenNews.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ShumenNewsDbContext db;
        private readonly IImageService imageService;

        public AdminController(ShumenNewsDbContext db, IImageService imageService)
        {
            this.db = db;
            this.imageService = imageService;
        }
        public IActionResult Index()
        {
            var model = db.Articles.Select(a => new ArticleViewModel
            {
                Id = a.Id,
                Title = a.Title,
                Content = a.Content,
                Likes = a.Likes,
                Dislikes = a.Dislikes,
                Views = a.Views,
                PublishedOn = a.PublishedOn,
                MainImage = imageService.GetImageUrlById(a.MainImageId),
                Images = imageService.GetAllArticleImageUrls(a)
            }).ToList();
            return View(model);
        }
        public IActionResult Details(int id)
        {
            return View();
        }
    }
}
