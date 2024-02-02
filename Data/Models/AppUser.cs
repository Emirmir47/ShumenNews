using Microsoft.AspNetCore.Identity;

namespace ShumenNews.Data.Models
{
    public class AppUser : IdentityUser
    {
        public AppUser()
        {
            OwnedArticles = new HashSet<Article>();
            UserArticlesAttitudes = new HashSet<UserArticleAttitude>();
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public virtual ICollection<Article> OwnedArticles  { get; set; }
        public virtual ICollection<UserArticleAttitude> UserArticlesAttitudes { get; set; }
    }
}
