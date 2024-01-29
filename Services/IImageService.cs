using ShumenNews.Data.Models;

namespace ShumenNews.Services
{
    public interface IImageService
    {
        public string GetImageById(string imageId);
        public List<ShumenNewsImage> GetAllImages();
        public List<string> GetAllImageNames();
        public List<string> GetAllArticleImageNames(int articleId);
        public List<ShumenNewsImage> GetAllArticleMainImages();
    }
}
