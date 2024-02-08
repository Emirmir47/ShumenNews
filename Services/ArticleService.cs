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
        public int GetLastArticleId()
        {
            return db.Articles.OrderBy(a => a.Id).Select(a => a.Id)
                .LastOrDefault();
        }
        public ShumenNewsArticle GetArticleById(int id)
        {
            var article = db.Articles.Include(a => a.Images)
                .FirstOrDefault(a => a.Id == id)!;
            return article;
        }
        public ShumenNewsUser GetArticleAuthor(ShumenNewsArticle article)
        {
            var author = article.UserArticles.Where(ua => ua.IsAuthor == true).Select(ua => ua.User).SingleOrDefault();
            return author!;
        }
        public List<ShumenNewsArticle> GetAllArticlesByAuthor(ShumenNewsUser author)
        {
            var articles = db.UserArticles.Where(ua => ua.User.Id == author.Id).Select(ua => ua.Article)
                .Include(a => a.Images)
                .Include(a => a.Category)
                .ToList();
            return articles;
        }
        public List<ShumenNewsArticle> GetAllArticlesWithShortContent()
        {
            var articles = db.Articles
                .Include(a => a.Images)
                .Include(a => a.Category)
                .Include(a => a.UserArticles.Where(ua => ua.IsAuthor == true))
                .ThenInclude(ua => ua.User).OrderByDescending(a => a.Id)
                .ToList();
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
