using System.ComponentModel.DataAnnotations.Schema;
using System.Net;

namespace ShumenNews.Data.Models
{
    public class ShumenNewsArticle : BaseEntity<int>
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
        public int LikesCount { get; set; }
        public int DislikesCount { get; set; }
        public int CommentsCount { get; set; }
        public DateTime PublishedOn { get; set; }
        public int ViewsCount { get; set; }
        public string CategoryId { get; set; }
        public virtual ShumenNewsCategory Category { get; set; }
        public virtual ICollection<ShumenNewsUserArticle> UserArticles { get; set; }
        public virtual ICollection<ShumenNewsComment> Comments { get; set; }
        public virtual ICollection<ShumenNewsImage> Images { get; set; }
        public string MainImageId { get; set; }
        public void CalCommentsCount()
        {
            CommentsCount = Comments.Count;
        }
    }
}
