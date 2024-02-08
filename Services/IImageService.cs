using ShumenNews.Data.Models;

namespace ShumenNews.Services
{
    public interface IImageService
    {
        public string GetImageUrlById(string imageId);
        public List<ShumenNewsImage> GetAllArticleMainImages();
        public string GetArticleMainImageUrl(string mainImageId, ShumenNewsArticle article);
    }
}
