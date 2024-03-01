namespace ShumenNews.Data.Models
{
    public class ShumenNewsComment
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int LikesCount { get; set; }
        public int DislikesCount { get; set; }
        public string UserId { get; set; }
        public virtual ShumenNewsUser User { get; set; }
        public int? ArticleId { get; set; }
        public virtual ShumenNewsArticle? Article { get; set; }
    }
}
