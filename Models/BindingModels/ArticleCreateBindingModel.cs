using ShumenNews.Data.Models;
using ShumenNews.Models.ViewModels;

namespace ShumenNews.Models.BindingModels
{
    public class ArticleCreateBindingModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string AuthorId { get; set; } = string.Empty;
        public virtual IEnumerable<IFormFile> Images { get; set; } 
        public virtual UserViewModel Author { get; set; }
    }
}
