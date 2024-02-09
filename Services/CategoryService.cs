using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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
            var category = db.Categories.Include(c=>c.Articles)
                .ThenInclude(a=>a.Images)
                .FirstOrDefault(c => c.Id == id);
            return category!;
        }
        public List<ShumenNewsCategory> GetAllArticles()
        {
            var categories = db.Categories.ToList();
            return categories;
        }
        public List<SelectListItem> GetAllCategoriesAsSelectListItem()
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
