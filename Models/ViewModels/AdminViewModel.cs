using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ShumenNews.Models.ViewModels
{
    public class AdminViewModel
    {
        public IEnumerable<UserViewModel> Authors { get; set; }
        public IEnumerable<CategoryViewModel> Categories { get; set; }
        public IEnumerable<ArticleViewModel> Articles { get; set; }
        [EmailAddress(ErrorMessage = "Въведете вярно написан имейл адрес!")]
        public string? Email { get; set; }
        public SearchViewModel Results { get; set; }
    }
}
