using Microsoft.AspNetCore.Mvc;
using ShumenNews.Models.ViewModels;
using ShumenNews.Services;

namespace ShumenNews.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoryService categoryService;
        private readonly IArticleService articleService;

        public CategoriesController(ICategoryService categoryService, IArticleService articleService)
        {
            this.categoryService = categoryService;
            this.articleService = articleService;
        }
        public IActionResult Index(string id)
        {
            var category = categoryService.GetCategoryById(id);
            var categories = articleService.GetArticlesByCategoryId(id);
            var model = new CategoryViewModel
            {
                Name = category.Name,
                Articles = categories,
            };
            return View(model);
        }
    }
}
