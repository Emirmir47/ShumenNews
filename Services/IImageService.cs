using ShumenNews.Data.Models;

namespace ShumenNews.Services
{
    public interface IImageService
    {
        public string GetImageById(string imageId);
        public List<string> GetAllImages();
    }
}
