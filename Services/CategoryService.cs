using Microsoft.AspNetCore.Mvc.Rendering;
using ShumenNews.Data;
using ShumenNews.Data.Models;

namespace ShumenNews.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ShumenNewsDbContext db;

        public CategoryService(ShumenNewsDbContext db)
        {
            this.db = db;
        }
        public ShumenNewsCategory GetCategoryById(string id)
        {
            var category = db.Categories.FirstOrDefault(c => c.Id == id);
            return category!;
        }
        public List<SelectListItem> GetAllCategories()
        {
            var categories = db.Categories.Select(c => new SelectListItem
            {
                Value = c.Id,
                Text = c.Name
            }).ToList();
            return categories;
        }

    }
}
