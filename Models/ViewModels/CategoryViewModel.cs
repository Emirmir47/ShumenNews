using ShumenNews.Data.Models;

namespace ShumenNews.Models.ViewModels
{
    public class CategoryViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<ShumenNewsArticle> Articles { get; set; }
    }
}
