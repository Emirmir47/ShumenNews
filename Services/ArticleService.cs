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
        public ShumenNewsArticle GetArticleById(int id)
        {
            var article = db.Articles.Include(a => a.Images)
                .FirstOrDefault(a => a.Id == id)!;
            return article;
        }
        public int GetLastArticleId()
        {
            return db.Articles.OrderBy(a => a.Id).Select(a => a.Id)
                .LastOrDefault();
        }
        public ShumenNewsUser GetArticleAuthor(ShumenNewsArticle article)
        {
            if (article is not null)
            {
                var author = db.UserArticles
                    .Where(ua => ua.Article.Id == article.Id && ua.IsAuthor == true)
                    .Select(ua => ua.User).SingleOrDefault();
                return author!;
            }
            return null!;
        }
        public List<ShumenNewsArticle> GetArticlesByAuthor(ShumenNewsUser author)
        {
            var articles = db.UserArticles
                .Where(ua => ua.User.Id == author.Id)
                .Include(ua => ua.Article)
                .ThenInclude(a => a.Images)
                .Select(ua => ua.Article)
                .ToList();
            return articles;
        }
        public List<ShumenNewsArticle> GetArticlesByCategoryId(string categoryId, int wordsCount = 0)
        {
            var articles = db.Articles.Where(a => a.CategoryId == categoryId)
                .Where(a => a.IsDeleted == false)
                .Include(a => a.Images)
                .ToList();
            articles = articles.OrderByDescending(a => a.Id).ToList();
            ArticlesWithShortContent(articles, wordsCount);
            return articles;
        }
        public List<ShumenNewsArticle> GetAllArticlesWithShortContent()
        {
            var articles = db.Articles
                .Where(a => a.IsDeleted == false)
                .Include(a => a.Images)
                .Include(a => a.Category)
                .Include(a => a.UserArticles.Where(ua => ua.IsAuthor == true))
                .ThenInclude(ua => ua.User).OrderByDescending(a => a.Id)
                .ToList();
            ArticlesWithShortContent(articles, 5);
            return articles;
        }
        public List<ShumenNewsArticle> ArticlesWithShortContent(List<ShumenNewsArticle> articles, int wordsCount)
        {
            foreach (var article in articles)
            {
                var words = article.Content.Split(" ").Take(wordsCount).ToList();
                string? shortContent = string.Join(" ", words) + "...";
                article.Content = shortContent;
            }
            return articles;
        }
    }
}
