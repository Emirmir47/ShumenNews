using System.ComponentModel.DataAnnotations;

namespace ShumenNews.Data.Models
{
    public class ShumenNewsUserArticle
    {
        [Key]
        public int Id { get; set; }
        public virtual ShumenNewsUser User { get; set; }
        public virtual ShumenNewsArticle Article { get; set; }
        public bool IsAuthor { get; set; } = false;
        //true=Like / false=Dislike /Null=neutral
        public bool? Attitude { get; set; }
    }
}
