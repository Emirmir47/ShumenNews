using System.ComponentModel.DataAnnotations;

namespace ShumenNews.Data.Models
{
    public class ShumenNewsCategoryArticle
    {
        [Key]
        public int Id { get; set; }
        public virtual ShumenNewsCategory Category { get; set; }
        public virtual ShumenNewsArticle Article { get; set; }
    }
}
