using Microsoft.AspNetCore.Mvc;
using ShumenNews.Data;
using ShumenNews.Data.Models;

namespace ShumenNews.Services
{
    public class ImageService : IImageService
    {
        private readonly ShumenNewsDbContext db;
        public ImageService(ShumenNewsDbContext db)
        {
            this.db = db;
        }
        public ShumenNewsImage GetImageById(string imageId)
        {
            var image = db.Images.FirstOrDefault(i => i.Id == imageId);
            return image;

        }
        public List<ShumenNewsImage> GetAllImages()
        {
            var images = db.Images.ToList();
            return images;
        }
    }
}
