using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShumenNews.Data;
using ShumenNews.Data.Models;

namespace ShumenNews.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ShumenNewsDbContext db;
        private readonly IArticleService articleService;

        public CategoryService(ShumenNewsDbContext db, IArticleService articleService)
        {
            this.db = db;
            this.articleService = articleService;
        }
        public ShumenNewsCategory GetCategoryById(string id)
        {
            var category = db.Categories.Include(c=>c.Articles)
                .ThenInclude(a=>a.Images)
                .FirstOrDefault(c => c.Id == id);
            return category!;
        }
        public List<ShumenNewsCategory> GetAllCategories()
        {
            var categories = db.Categories.ToList();
            return categories;
        }
        public List<ShumenNewsCategory> GetAllCategoriesWithThreeArticles()
        {
            var categories = db.Categories
                .Include(c=>c.Articles)
                .ThenInclude(a=>a.Images)
                .ToList();
            foreach (var category in categories)
            {
                category.Articles = category.Articles.OrderByDescending(a=>a.Id).ToList();
                category.Articles = articleService.ArticlesWithShortContent(category.Articles.Take(3).ToList(), 20);
            }
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
