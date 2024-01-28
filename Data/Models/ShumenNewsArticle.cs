using System.Net;

namespace ShumenNews.Data.Models
{
    public class ShumenNewsArticle
    {
        public ShumenNewsArticle()
        {
            PublishedOn = DateTime.UtcNow;
            UserArticles = new HashSet<ShumenNewsUserArticle>();
            Comments = new HashSet<ShumenNewsComment>();
            Images = new HashSet<ShumenNewsImage>();
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int Likes { get; set; }
        public int Dislikes { get; set; }
        public DateTime PublishedOn { get; set; }
        public int Views { get; set; }
        public virtual ICollection<ShumenNewsUserArticle> UserArticles { get; set; }
        public virtual ICollection<ShumenNewsComment> Comments { get; set; }
        public virtual ICollection<ShumenNewsImage> Images { get; set; }
        public string MainImageId { get; set; }

    }
}
