using System.ComponentModel.DataAnnotations.Schema;
using System.Net;

namespace ShumenNews.Data.Models
{
    public class Article
    {
        public Article()
        {
            PublishedOn = DateTime.UtcNow;
            ArticleAttitudes = new HashSet<UserArticleAttitude>();
            Comments = new HashSet<Comment>();
            Images = new HashSet<Image>();
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int Likes { get; set; }
        public int Dislikes { get; set; }
        public DateTime PublishedOn { get; set; }
        public int Views { get; set; }
        
   
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
      
        [ForeignKey(nameof(Image))]
        public string MainImageId { get; set; }
        public virtual Image MainImage { get; set; }
       
        public virtual ICollection<UserArticleAttitude> ArticleAttitudes { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Image> Images { get; set; }



    }
}
