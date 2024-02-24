using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Areas;
using ShumenNews.Data;
using ShumenNews.Data.Models;
using ShumenNews.Models.BindingModels;
using ShumenNews.Models.ViewModels;
using ShumenNews.Services;
using System.ComponentModel.DataAnnotations;

namespace ShumenNews.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ShumenNewsDbContext db;
        private readonly UserManager<ShumenNewsUser> userManager;
        private readonly IImageService imageService;
        private readonly IUserService userService;
        private readonly IArticleService articleService;

        public AdminController(ShumenNewsDbContext db, UserManager<ShumenNewsUser> userManager,
            IImageService imageService,
            IUserService userService,
            IArticleService articleService)
        {
            this.db = db;
            this.userManager = userManager;
            this.imageService = imageService;
            this.userService = userService;
            this.articleService = articleService;
        }
        public IActionResult Index()
        {
            var articles = db.Articles.Select(a => new ArticleViewModel
            {
                Id = a.Id,
                Title = a.Title,
                Content = a.Content,
                LikesCount = a.LikesCount,
                DislikesCount = a.DislikesCount,
                ViewsCount = a.ViewsCount,
                CommentsCount = a.CommentsCount,
                PublishedOn = a.PublishedOn,
                MainImage = imageService.GetArticleMainImageUrl(a.MainImageId, a),
                Images = a.Images.Select(a => a.Url),
                Category = a.Category
            }).ToList();
            var authors = db.Users
                .Where(u => u.UserArticles.Any(ua => ua.IsAuthor == true))
                .Select(u => new UserViewModel
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email
                }).ToList();
            var categories = db.Categories
                .Select(c => new CategoryViewModel
                {
                    Id = c.Id,
                    Name = c.Name
                }).ToList();
            var model = new AdminViewModel
            {
                Authors = authors,
                Categories = categories,
                Articles = articles,
            };
            return View(model);
        }
        [HttpPost]
        public IActionResult Index(string email)
        {
            var articles = db.Articles.Select(a => new ArticleViewModel
            {
                Id = a.Id,
                Title = a.Title,
                Content = a.Content,
                LikesCount = a.LikesCount,
                DislikesCount = a.DislikesCount,
                ViewsCount = a.ViewsCount,
                CommentsCount = a.CommentsCount,
                PublishedOn = a.PublishedOn,
                MainImage = imageService.GetArticleMainImageUrl(a.MainImageId, a),
                Images = a.Images.Select(a => a.Url),
                Category = a.Category
            }).ToList();
            var authors = db.Users
                .Where(u => u.UserArticles.Any(ua => ua.IsAuthor == true))
                .Select(u => new UserViewModel
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email
                }).ToList();
            var categories = db.Categories
                .Select(c => new CategoryViewModel
                {
                    Id = c.Id,
                    Name = c.Name
                }).ToList();
            var roles = db.Roles.ToList();
            var adminViewModel = new AdminViewModel
            {
                Authors = authors,
                Categories = categories,
                Articles = articles,
            };
            if (email is not null)
            {
                var user = userService.GetUserByEmail(email);
                var userRoles = userManager.GetRolesAsync(user).Result.ToList();
                var rolesViewModels = new List<RoleViewModel>();
                foreach (var role in roles)
                {
                    if (userRoles.Contains(role.Name))
                    {
                        rolesViewModels.Add(new RoleViewModel
                        {
                            Name = role.Name,
                            IsChecked = true
                        });
                    }
                    else
                    {
                        rolesViewModels.Add(new RoleViewModel
                        {
                            Name = role.Name,
                            IsChecked = false
                        });
                    }
                }
                if (user != null)
                {
                    var userViewModel = new UserViewModel
                    {
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        Roles = rolesViewModels
                    };
                    if (user.UserArticles.Any(ua => ua.IsAuthor))
                    {
                        var authorArticles = articleService.GetArticlesByAuthor(user);
                        var modelWithArticles = new SearchViewModel
                        {
                            IsAuthor = true,
                            User = userViewModel,
                            Articles = authorArticles.Select(a => new ArticleViewModel
                            {
                                Id = a.Id,
                                Title = a.Title,
                                LikesCount = a.LikesCount,
                                ViewsCount = a.ViewsCount,
                                CommentsCount = a.CommentsCount,
                                PublishedOn = a.PublishedOn,
                            }).ToList()
                        };
                        return View(new AdminViewModel
                        {
                            Authors = authors,
                            Articles = articles,
                            Categories = categories,
                            Results = modelWithArticles
                        });
                    }
                    var model = new SearchViewModel
                    {
                        IsAuthor = false,
                        User = userViewModel
                    };
                    return View(new AdminViewModel
                    {
                        Authors = authors,
                        Articles = articles,
                        Categories = categories,
                        Results = model
                    });
                }
            }
            return View(adminViewModel);
        }
        //TODO Roles
    }
}
