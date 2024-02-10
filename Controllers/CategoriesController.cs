using Microsoft.AspNetCore.Mvc;
using ShumenNews.Models.ViewModels;
using ShumenNews.Services;

namespace ShumenNews.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoryService categoryService;
        private readonly IArticleService articleService;
        private readonly IImageService imageService;

        public CategoriesController(ICategoryService categoryService, IArticleService articleService, IImageService imageService)
        {
            this.categoryService = categoryService;
            this.articleService = articleService;
            this.imageService = imageService;
        }
        public IActionResult Index(string id)
        {
            var category = categoryService.GetCategoryById(id);
            var articles = articleService.GetArticlesByCategoryId(id, 20);
            var articleViewModels = articles.Select(a => new ArticleViewModel
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
                Category = a.Category,
            }).ToList();
            var model = new CategoryViewModel
            {
                Name = category.Name,
                Articles = articleViewModels,
            };
            return View(model);
        }
    }
}
