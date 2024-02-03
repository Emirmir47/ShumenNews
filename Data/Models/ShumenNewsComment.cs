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
        public virtual ShumenNewsUserArticle? Article { get; set; }
        public int? ParentCommentId { get; set; }
        public virtual ShumenNewsComment? ParentComment { get; set; }
        public virtual ICollection<ShumenNewsComment>? Kids { get; set; }
    }
}
