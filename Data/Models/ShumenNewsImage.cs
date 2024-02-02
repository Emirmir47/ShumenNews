namespace ShumenNews.Data.Models
{

    public class ShumenNewsImage
    {
        public ShumenNewsImage()
        {
            this.Id = Guid.NewGuid().ToString();
        }
        public string Id { get; set; }
        public string Extension { get; set; }
        public virtual string Url => $"/img/{Id}.{Extension}";
        public int ArticleId { get; set; }
        public virtual ShumenNewsArticle Article { get; set; }
    }
}
