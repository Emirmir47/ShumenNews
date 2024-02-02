using ShumenNews.Data.Models;

namespace ShumenNews.Services
{
    public interface IImageService
    {
        public string GetImageUrlById(string imageId);
        public List<Image> GetAllImages();
        public List<Image> GetAllArticleMainImages();
        public string GetArticleMainImageUrl(string mainImageId, Article article);
        public List<string> GetAllImageUrls();
        public List<string> GetAllArticleImageUrls(Article article = null);
    }
}
