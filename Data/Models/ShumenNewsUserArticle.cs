using System.ComponentModel.DataAnnotations;

namespace ShumenNews.Data.Models
{
    public class ShumenNewsUserArticle
    {
        [Key]
        public int ArticleId { get; set; }
        public string UserId { get; set; }
        public virtual ShumenNewsUser User { get; set; }
        public virtual ShumenNewsUserArticle Article { get; set; }
        public bool IsLiked { get; set; }
        public bool IsDisliked { get; set; }
    }
}
