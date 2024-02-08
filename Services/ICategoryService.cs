using Microsoft.AspNetCore.Mvc.Rendering;
using ShumenNews.Data.Models;

namespace ShumenNews.Services
{
    public interface ICategoryService
    {
        public ShumenNewsCategory GetCategoryById(string id);
        List<ShumenNewsCategory> GetAllArticles();
        public List<SelectListItem> GetAllCategoriesAsSelectListItem();
    }
}
