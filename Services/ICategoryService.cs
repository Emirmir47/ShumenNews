using Microsoft.AspNetCore.Mvc.Rendering;
using ShumenNews.Data.Models;

namespace ShumenNews.Services
{
    public interface ICategoryService
    {
        public ShumenNewsCategory GetCategoryById(string id);
        public List<ShumenNewsCategory> GetAllCategories();
        public List<ShumenNewsCategory> GetAllCategoriesWithThreeArticles();
        public List<SelectListItem> GetAllCategoriesAsSelectListItem();
    }
}
