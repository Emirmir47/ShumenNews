using Microsoft.AspNetCore.Identity;

namespace ShumenNews.Data.Models
{
    public class ShumenNewsUser : IdentityUser
    {
        public ShumenNewsUser()
        {
            UserArticles = new HashSet<ShumenNewsUserArticle>();
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual ICollection<ShumenNewsUserArticle> UserArticles { get; set; }
    }
}
