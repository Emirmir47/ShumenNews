using ShumenNews.Data.Models;

namespace ShumenNews.Services
{
    public interface IArticleService
    {
        List<ShumenNewsArticle> GetAllArticlesByAuthor(ShumenNewsUser author);
        public List<ShumenNewsArticle> GetAllArticlesWithShortContent();
        ShumenNewsUser GetArticleAuthor(ShumenNewsArticle article);
        ShumenNewsArticle GetArticleById(int id);
        public int GetLastArticleId();
    }
}
