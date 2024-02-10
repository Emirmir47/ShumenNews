using ShumenNews.Data.Models;

namespace ShumenNews.Models.ViewModels
{
    public class ArticleViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int LikesCount { get; set; }
        public int DislikesCount { get; set; }
        public int ViewsCount { get; set; }
        public int CommentsCount { get; set; }
        public DateTime PublishedOn { get; set; }
        public string MainImage { get; set; }
        public virtual ShumenNewsUser Author { get; set; }
        public virtual ShumenNewsCategory Category { get; set; }
        public virtual ICollection<ShumenNewsUserArticle> UserArticles { get; set; }
        public virtual ICollection<ShumenNewsComment> Comments { get; set; }
        public virtual IEnumerable<string> Images { get; set; }
    }
}
