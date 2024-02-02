using ShumenNews.Data.Models;

namespace ShumenNews.Models.ViewModels
{
    public class ArticleViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int Likes { get; set; }
        public int Dislikes { get; set; }
        public int Views { get; set; }
        public DateTime PublishedOn { get; set; }
        public string MainImage { get; set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<UserArticleAttitude> UserArticles { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual IEnumerable<string> Images { get; set; }
    }
}
