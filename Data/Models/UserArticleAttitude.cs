using System.ComponentModel.DataAnnotations;

namespace ShumenNews.Data.Models
{
    public class UserArticleAttitude
    {
        //[Key]
        //public int Id { get; set; }

        //TODO PK = Composite Key Based on ShumenNewsUserId + ShumenNewsArticleId
        public int AppUserId { get; set; }
        public virtual AppUser User { get; set; }

        public int ArticleId { get; set; }
        public virtual Article Article { get; set; }

        //public bool IsAuthor { get; set; } = false;
      
        //true=Like / false=Dislike /Null=neutral
        public bool? Attitude { get; set; }
       // public bool IsDeleted { get; set; }
    }
}
