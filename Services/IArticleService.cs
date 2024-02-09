using ShumenNews.Data.Models;

namespace ShumenNews.Services
{
    public interface IArticleService
    {
        public ShumenNewsArticle GetArticleById(int id);
        public int GetLastArticleId();
        public List<ShumenNewsArticle> GetArticlesByAuthor(ShumenNewsUser author);
        List<ShumenNewsArticle> GetArticlesByCategoryId(string categoryId);
        public List<ShumenNewsArticle> GetAllArticlesWithShortContent();
        public ShumenNewsUser GetArticleAuthor(ShumenNewsArticle article);
    }
}
