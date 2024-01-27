using System.Net;

namespace ShumenNews.Data.Models
{
    public class ShumenNewsArticle
    {
        public ShumenNewsArticle()
        {
            PublishedOn = DateTime.UtcNow;
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int Likes { get; set; }
        public int Dislikes { get; set; }
        public DateTime PublishedOn { get; set; }
        public int ViewCounter { get; set; }
        public string AuthorId { get; set; }
        public virtual ShumenNewsUser Author { get; set; }
        public virtual ICollection<ShumenNewsUserArticle> UserArticles { get; set; }
        public virtual ICollection<ShumenNewsComment> Comments { get; set; }
        public virtual ICollection<ShumenNewsImage> Images { get; set; }
        public string MainImageId { get; set; }

    }
}
