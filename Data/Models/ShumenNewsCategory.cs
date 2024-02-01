namespace ShumenNews.Data.Models
{
    public class ShumenNewsCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<ShumenNewsArticle> Articles { get; set; }
    }
}
