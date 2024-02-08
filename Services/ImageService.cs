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
        public string GetImageUrlById(string imageId)
        {
            var img = db.Images.FirstOrDefault(i => i.Id == imageId);
            var image = $"/img/{img!.Id}.{img.Extension}";
            return image!;

        }
        public string GetArticleMainImageUrl(string mainImageId, ShumenNewsArticle article)
        {
            var image = article.Images.FirstOrDefault(a=>a.Id == mainImageId);
            var imageUrl = $"/img/{image!.Id}.{image.Extension}";
            return imageUrl;
        }
        public List<ShumenNewsImage> GetAllArticleMainImages()
        {
            var images = db.Articles
                .SelectMany(a => a.Images.Where(i => i.Id == a.MainImageId))
                .Include(i => i.Article)
                .ToList();
            return images;
        }
    }
}
