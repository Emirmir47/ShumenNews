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

        public HomeController(ILogger<HomeController> logger, ShumenNewsDbContext db, IImageService imageService)
        {
            _logger = logger;
            this.db = db;
            this.imageService = imageService;
        }

        public IActionResult Index()
        {
            var images = imageService.GetAllArticleMainImages();
            var model = images.Select(i=> new ImageViewModel
            {
                Name = $"/img/{i.Id}.{i.Extension}",
                ArticleId = i.ArticleId,
                Article = i.Article
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
