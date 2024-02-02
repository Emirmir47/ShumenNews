using Microsoft.AspNetCore.Mvc.Rendering;
using ShumenNews.Data.Models;
using ShumenNews.Models.ViewModels;

namespace ShumenNews.Models.BindingModels
{
    public class ArticleCreateBindingModel
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string CategoryId { get; set; }
        public List<SelectListItem> Categories { get; set; } = new List<SelectListItem>();
        public virtual IEnumerable<IFormFile> Images { get; set; }
    }
}
