namespace ShumenNews.Data.Models
{
    public class ShumenNewsComment : BaseEntity<int>
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string UserId { get; set; }
        public virtual ShumenNewsUser User { get; set; }
        public int? ArticleId { get; set; }
        public virtual ShumenNewsArticle? Article { get; set; }
    }
}
