using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public List<ShumenNewsImage> GetAllArticleMainImages()
        {
            var images = db.Articles
                .SelectMany(a=>a.Images.Where(i=>i.Id == a.MainImageId))
                .Include(i=>i.Article)
                .ToList();
            return images;
        }
        public List<ShumenNewsImage> GetAllImages()
        {
            var images = db.Images.Include(i=>i.Article).ToList();
            return images;
        }
        public List<string> GetAllImageNames()
        {
            var imgs = db.Images.ToList();
            var images = imgs.Select(i => $"/img/{i.Id}.{i.Extension}").ToList();
            return images;
        }
        public List<string> GetAllArticleImageNames(int articleId)
        {
            var imgs = db.Images.Where(i=>i.ArticleId == articleId).ToList();
            var images = imgs.Select(i => $"/img/{i.Id}.{i.Extension}").ToList();
            return images;
        }
    }
}
