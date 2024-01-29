using ShumenNews.Data.Models;
using ShumenNews.Models.ViewModels;

namespace ShumenNews.Models.BindingModels
{
    public class ArticleCreateBindingModel
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public virtual IEnumerable<IFormFile> Images { get; set; }
    }
}
