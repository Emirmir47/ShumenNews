﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShumenNews.Data;
using ShumenNews.Data.Models;
using ShumenNews.Models.ViewModels;
using ShumenNews.Services;
using System.Data;

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
            var data = LoadViewModels();
            var adminViewModel = CreateAdminViewModel(data);
            return View(adminViewModel);
        }
        [HttpPost]
        public IActionResult Index(string email)
        {
            var data = LoadViewModels();
            //AdminViewModel
            var adminViewModel = CreateAdminViewModel(data);
            adminViewModel.Email = email;

            if (email is not null)
            {
                var user = userService.GetUserByEmail(email);
                if (user != null)
                {
                    var userRoles = userManager.GetRolesAsync(user).Result.ToList();

                    //UserViewModel
                    var userViewModel = new UserViewModel
                    {
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                    };
                    if (userManager.IsLockedOutAsync(user).Result)
                    {
                        userViewModel.IsBlocked = true;
                    }

                    //SearchViewModel
                    var searchViewModel = new SearchViewModel
                    {
                        IsAuthor = false,
                        User = userViewModel
                    };

                    if (user.UserArticles.Any(ua => ua.IsAuthor))
                    {
                        var authorArticles = articleService.GetArticlesByAuthor(user);
                        {
                            searchViewModel.IsAuthor = true;
                            searchViewModel.Articles = authorArticles.Select(a => new ArticleViewModel
                            {
                                Id = a.Id,
                                Title = a.Title,
                                LikesCount = a.LikesCount,
                                ViewsCount = a.ViewsCount,
                                CommentsCount = a.CommentsCount,
                                PublishedOn = a.PublishedOn,
                            }).ToList();
                        };
                    }
                    adminViewModel.Results = searchViewModel;
                }
            }
            return View(adminViewModel);
        }

        public IActionResult Details(string id)
        {
            var email = id;
            if (email is not null)
            {
                var user = userService.GetUserByEmail(email);
                var roles = db.Roles.ToList();
                if (user != null)
                {
                    var userRoles = userManager.GetRolesAsync(user).Result.ToList();

                    //RoleViewModels
                    var roleViewModels = new List<RoleViewModel>();
                    foreach (var role in roles)
                    {
                        if (userRoles.Contains(role.Name))
                        {
                            roleViewModels.Add(new RoleViewModel
                            {
                                Name = role.Name,
                                IsChecked = true
                            });
                        }
                        else
                        {
                            roleViewModels.Add(new RoleViewModel
                            {
                                Name = role.Name,
                                IsChecked = false
                            });
                        }
                    }

                    //UserViewModel
                    var userViewModel = new UserViewModel
                    {
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        Roles = roleViewModels,
                        RolesInStr = string.Join(",", roleViewModels.Select(r=>$"{r.Name}={r.IsChecked}"))
                    };

                    if (userManager.IsLockedOutAsync(user).Result)
                    {
                        userViewModel.IsBlocked = true;
                    }

                    //SearchViewModel
                    var searchViewModel = new SearchViewModel
                    {
                        User = userViewModel
                    };
                    if (user.UserArticles.Any(ua => ua.IsAuthor))
                    {
                        var authorArticles = articleService.GetArticlesByAuthor(user);

                        searchViewModel.IsAuthor = true;
                        searchViewModel.Articles = authorArticles.Select(a => new ArticleViewModel
                        {
                            Id = a.Id,
                            Title = a.Title,
                            LikesCount = a.LikesCount,
                            ViewsCount = a.ViewsCount,
                            CommentsCount = a.CommentsCount,
                            PublishedOn = a.PublishedOn,
                        }).ToList();
                    }
                    return View(searchViewModel);
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Details(SearchViewModel searchViewModel)
        {
            var formData = Request.Form;
            var user = searchViewModel.User;
            var userRoles = user.RolesInStr.Split(",").ToList();
            user.Roles = userRoles.Select(ur => new RoleViewModel
            {
                Name = ur.Split("=")[0],
                IsChecked = Convert.ToBoolean(ur.Split("=")[1])
            }).ToList();
            SetUserProps(searchViewModel.User);
            return RedirectToAction("Index");
        }
        private void SetUserProps(UserViewModel user)
        {
            //TODO AdminViewModel properties are null! Fix it!

            //userService.BlockUser(new UserViewModel { Email = "ivi677@gmail.com", BlockTime = 99 });
            //userService.UnblockUser(new UserViewModel { Email = "ivi677@gmail.com" });
            //userService.UpdateUserRoles(
            //    new UserViewModel
            //    {
            //        Email = "god@gmail.com",
            //        Roles = new List<RoleViewModel>
            //    {
            //            new RoleViewModel
            //            {
            //                Name = "Admin",
            //                IsChecked = true,
            //            },
            //            new RoleViewModel
            //            {
            //                Name = "Author",
            //                IsChecked = true
            //            },
            //            new RoleViewModel
            //            {
            //                Name = "Moderator",
            //                IsChecked = false
            //            },
            //            new RoleViewModel
            //            {
            //                Name = "Bay Ganio",
            //                IsChecked = true
            //            }
            //    }
            //    });

            if (user.IsBlocked)
            {
                userService.BlockUser(user);
            }
            else if (user.IsUnblocked)
            {
                userService.UnblockUser(user);
            }
            if (user.HasUpdatedRoles)
            {
                userService.UpdateUserRoles(user);
            }
        }
        private Dictionary<string, object> LoadViewModels()
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
            return new Dictionary<string, object>
            {
                { "Articles", articles },
                { "Authors", authors },
                { "Categories", categories }
            };
        }
        private AdminViewModel CreateAdminViewModel(Dictionary<string, object> data)
        {
            var adminViewModel = new AdminViewModel
            {
                Articles = (List<ArticleViewModel>)data["Articles"],
                Authors = (List<UserViewModel>)data["Authors"],
                Categories = (List<CategoryViewModel>)data["Categories"],
            };
            return adminViewModel;
        }
    }
}
