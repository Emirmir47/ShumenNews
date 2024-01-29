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
        public string GetImageById(string imageId)
        {
            var img = db.Images.FirstOrDefault(i => i.Id == imageId);
            var image = $"/img/{img!.Id}.{img.Extension}";
            return image!;

        }
        public List<string> GetAllImages()
        {
            var imgs = db.Images.ToList();
            var images = imgs.Select(i => $"/img/{i.Id}.{i.Extension}").ToList();
            return images;
        }
        public List<string> GetAllArticleImages(int articleId)
        {
            var imgs = db.Images.Where(i=>i.ArticleId == articleId).ToList();
            var images = imgs.Select(i => $"/img/{i.Id}.{i.Extension}").ToList();
            return images;
        }
    }
}
