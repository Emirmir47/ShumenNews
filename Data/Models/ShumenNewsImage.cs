namespace ShumenNews.Data.Models
{
    public class ShumenNewsImage
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Extension { get; set; }
        public int ArticleId { get; set; }
        public virtual ShumenNewsArticle Article { get; set; }
    }
}
