namespace ShumenNews.Data.Models
{

    public class Image
    {
        public Image()
        {
            this.Id = Guid.NewGuid().ToString();
        }
        public string Id { get; set; }
        public string Location { get; set; }//"../special-pictures/babajaba.jpg"
        
        public int ArticleId { get; set; }
        public virtual Article Article { get; set; }
    }
}
