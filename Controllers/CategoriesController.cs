using Microsoft.AspNetCore.Mvc;
using ShumenNews.Models.ViewModels;
using ShumenNews.Services;

namespace ShumenNews.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoryService categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }
        public IActionResult Index(string id)
        {
            var category = categoryService.GetCategoryById(id);
            var model = new CategoryViewModel
            {
                Name = category.Name,
            };
            return View(model);
        }
    }
}
