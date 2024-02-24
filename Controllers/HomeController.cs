using Microsoft.AspNetCore.Components;
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
        private readonly ICategoryService categoryService;
        private readonly IArticleService articleService;
        private readonly IUserService userService;

        public HomeController(ILogger<HomeController> logger,
            ShumenNewsDbContext db,
            IImageService imageService,
            ICategoryService categoryService,
            IArticleService articleService,
            IUserService userService)
        {
            _logger = logger;
            this.db = db;
            this.imageService = imageService;
            this.categoryService = categoryService;
            this.articleService = articleService;
            this.userService = userService;
        }

        public IActionResult Index()
        {
            //userService.SetAuthorRoles();
            var articles = articleService.GetArticlesByCategoryId("Week", 20);
            var articleViewModels = articles.Select(a => new ArticleViewModel
            {
                Id = a.Id,
                Title = a.Title,
                LikesCount = a.LikesCount,
                ViewsCount = a.ViewsCount,
                PublishedOn = a.PublishedOn,
                MainImage = imageService.GetArticleMainImageUrl(a.MainImageId, a),
            }).ToList();
            var articleViewModelsForCategoryWeek = articleViewModels
                .ToList();
            var categories = categoryService.GetAllCategoriesWithThreeArticles();
            var categoryViewModels = categories.Select(c => new CategoryViewModel
            {
                Id = c.Id,
                Name = c.Name,
                Articles = c.Articles.Select(a => new ArticleViewModel
                {
                    Id = a.Id,
                    Title = a.Title,
                    Content = a.Content,
                    LikesCount = a.LikesCount,
                    ViewsCount = a.ViewsCount,
                    CommentsCount = a.CommentsCount,
                    PublishedOn = a.PublishedOn,
                    MainImage = imageService.GetArticleMainImageUrl(a.MainImageId, a),
                })
            });
            var model = new PreViewModel
            {
                WeekArticles = articleViewModelsForCategoryWeek,
                Categories = categoryViewModels,
            };
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
