using ShumenNews.Data.Models;

namespace ShumenNews.Services
{
    public interface IImageService
    {
        public string GetImageUrlById(string imageId);
        public List<ShumenNewsImage> GetAllImages();
        public List<ShumenNewsImage> GetAllArticleMainImages();
        public string GetArticleMainImageUrl(string mainImageId, ShumenNewsArticle article);
        public List<string> GetAllImageUrls();
        public List<string> GetAllArticleImageUrls(ShumenNewsArticle article = null);
    }
}
