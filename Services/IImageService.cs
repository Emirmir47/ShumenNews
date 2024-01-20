using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShumenNews.Data.Models;
using System.Net.Mime;

namespace ShumenNews.Services
{
    public interface IImageService
    {
        public ShumenNewsImage GetImageById(string imageId);
        public List<ShumenNewsImage> GetAllImages();
    }
}