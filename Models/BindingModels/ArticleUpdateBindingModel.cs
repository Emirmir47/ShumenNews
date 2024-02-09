using ShumenNews.Data.Models;

namespace ShumenNews.Models.BindingModels
{
    public class ArticleUpdateBindingModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public bool IsDeleted { get; set; }
        public ShumenNewsUser Author { get; set; }
    }
}
