using Microsoft.AspNetCore.Mvc.Rendering;
using ShumenNews.Data.Models;

namespace ShumenNews.Services
{
    public interface ICategoryService
    {
        public ShumenNewsCategory GetCategoryById(string id);
        public List<SelectListItem> GetAllCategories();
    }
}
