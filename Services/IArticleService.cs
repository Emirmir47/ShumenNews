using ShumenNews.Data.Models;

namespace ShumenNews.Services
{
    public interface IArticleService
    {
        public List<Article> GetAllArticlesWithShortContent();
    }
}
