using Microsoft.AspNetCore.Mvc;
using ShumenNews.Data;
using ShumenNews.Models;
using ShumenNews.Models.ViewModels;
using ShumenNews.Services;
using System.Diagnostics;

namespace ShumenNews.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ShumenNewsDbContext db;
        private readonly IImageService imageService;
        private readonly IArticleService articleService;

        public HomeController(ILogger<HomeController> logger,
            ShumenNewsDbContext db,
            IImageService imageService,
            IArticleService articleService)
        {
            _logger = logger;
            this.db = db;
            this.imageService = imageService;
            this.articleService = articleService;
        }

        public IActionResult Index()
        {
            var articles = articleService.GetAllArticlesWithShortContent();
            var model = articles.Select(a => new ArticleViewModel
            {
                Id = a.Id,
                Title = a.Title,
                Content = a.Content,
                Likes = a.Likes,
                Dislikes = a.Dislikes,
                Views = a.Views,
                PublishedOn = a.PublishedOn,
                MainImage = imageService.GetArticleMainImageUrl(a.MainImageId, a),
                //
            }).ToList();
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
