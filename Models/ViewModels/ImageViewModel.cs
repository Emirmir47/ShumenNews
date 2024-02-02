using ShumenNews.Data.Models;

namespace ShumenNews.Models.ViewModels
{
    public class ImageViewModel
    {
        public string Id { get; set; }
        public string Extension { get; set; }
        public string Name { get; set; }
        public int ArticleId { get; set; }
        public virtual Article Article { get; set; }
    }
}
