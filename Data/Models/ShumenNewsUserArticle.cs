using System.ComponentModel.DataAnnotations;

namespace ShumenNews.Data.Models
{
    public class ShumenNewsUserArticle
    {
        [Key]
        public int Id { get; set; }
        public virtual ShumenNewsUser User { get; set; }
        public virtual ShumenNewsImages Article { get; set; }
        public bool IsAuthor { get; set; } = false;
        public bool IsLiked { get; set; }
        public bool IsDisliked { get; set; }
    }
}
