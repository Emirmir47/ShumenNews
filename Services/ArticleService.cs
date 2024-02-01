using Microsoft.EntityFrameworkCore;
using ShumenNews.Data;
using ShumenNews.Data.Models;

namespace ShumenNews.Services
{
    public class ArticleService : IArticleService
    {
        private readonly ShumenNewsDbContext db;

        public ArticleService(ShumenNewsDbContext db)
        {
            this.db = db;
        }
        public List<ShumenNewsArticle> GetAllArticlesWithShortContent()
        {
            var articles = db.Articles
                .Include(a=>a.Images)
                .Include(a => a.CategoryArticles)
                .ThenInclude(ca => ca.Category).ToList();
            foreach (var article in articles)
            {
                var words = article.Content.Split(" ").Take(25).ToList();
                var shortContent = string.Empty;
                shortContent = string.Join(" ", words) + "...";
                article.Content = shortContent;
            }
            return articles;
        }
    }
}
