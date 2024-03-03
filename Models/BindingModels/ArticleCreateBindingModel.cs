using Microsoft.AspNetCore.Mvc.Rendering;
using ShumenNews.Data.Models;
using ShumenNews.Models.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace ShumenNews.Models.BindingModels
{
    public class ArticleCreateBindingModel
    {
        [Required(ErrorMessage = "Това поле е задължително!")]
        //[Range(10, 150, ErrorMessage = "Заглавието трябва да включва поне 10 символа до максимум 150")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Това поле е задължително!")]
        //[Range(50, 15000, ErrorMessage = "Заглавието трябва да включва поне 100 символа до максимум 5000")]
        public string Content { get; set; }
        public string CategoryId { get; set; }
        public List<SelectListItem> Categories { get; set; } = new List<SelectListItem>();
        [Required(ErrorMessage = "Добавете поне една снимка!")]
        public virtual IEnumerable<IFormFile> Images { get; set; }
    }
}
