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
            var image = $"/img/{img!.Id}.{img.Location}";
            return image!;

        }
        public List<Image> GetAllImages()
        {
            var images = db.Images.Include(i => i.Article).ToList();
            return images;
        }
        public List<Image> GetAllArticleMainImages()
        {
            //var images = db.Articles
            //    .SelectMany(a => a.Images.Where(i => i.Id == a.MainImageId))
            //    .Include(i => i.Article)
            //    .ToList();
            //return images;
            return null;
        }
        public string GetArticleMainImageUrl(string mainImageId, Article article)
        {
            var image = article.Images.FirstOrDefault(a=>a.Id == mainImageId);
            var imageUrl = $"/img/{image!.Id}.{image.Location}";
            return imageUrl;
        }
        public List<string> GetAllArticleImageUrls(Article article = null)
        {
            var imageUrls = new List<string>();
            imageUrls = article.Images
            .Select(i => $"/img/{i.Id}.{i.Location}").ToList();
            return imageUrls;
        }
        public List<string> GetAllImageUrls()
        {
            var imgs = db.Images.ToList();
            var images = imgs.Select(i => $"/img/{i.Id}.{i.Location}").ToList();
            return images;
        }
    }
}
