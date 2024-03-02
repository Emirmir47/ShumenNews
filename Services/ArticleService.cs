using Microsoft.EntityFrameworkCore;
using ShumenNews.Data;
using ShumenNews.Data.Models;
using ShumenNews.Models.ViewModels;
using System.Net.WebSockets;

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
            var article = db.Articles
                .Include(a => a.Category)
                .Include(a => a.Images)
                .Include(a => a.UserArticles)
                .ThenInclude(ua => ua.User)
                .Include(a => a.Comments)
                .ThenInclude(a => a.User)
                .FirstOrDefault(a => a.Id == id)!;
            return article;
        }
        public int GetLastArticleId()
        {
            return db.Articles.OrderBy(a => a.Id).Select(a => a.Id)
                .LastOrDefault();
        }
        public UserArticleViewModel GetUserArticleAsDTOByUsername(string username, ShumenNewsArticle article)
        {
            var userArticle = article.UserArticles
                .Where(u => u.User.UserName == username)
                .FirstOrDefault()!;
            if (userArticle is not null)
            {
                var userArticleViewModel = new UserArticleViewModel
                {
                    Id = userArticle.Id,
                    Attitude = userArticle.Attitude,
                };
                return userArticleViewModel!;
            }
            return null!;
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
                .Where(ua => ua.IsAuthor == true)
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
        public void CreateUserArticle(ArticleViewModel articleViewModel, ShumenNewsUser user)
        {
            var article = GetArticleById(articleViewModel.Id);
            var userArticle = new ShumenNewsUserArticle
            {
                Article = article,
                User = user,
                Attitude = articleViewModel.UserAttitude
            };
            db.UserArticles.Add(userArticle);
            db.SaveChanges();
        }
        public void SetAttitudeToArticle(ArticleViewModel articleViewModel)
        {
            var userArticles = db.UserArticles
                .FirstOrDefault(ua => ua.Id == articleViewModel.UserArticle.Id);
            if (userArticles!.Attitude == articleViewModel.UserArticle.Attitude)
            {
                userArticles!.Attitude = null;
            }
            else
            {
                userArticles!.Attitude = articleViewModel.UserArticle.Attitude;
            }
            db.SaveChanges();
        }
        public void AddCommentToArticle(ArticleViewModel articleViewModel)
        {

        }
        public void DeleteCommentFromArticle(ArticleViewModel articleViewModel)
        {

        }
    }
}
