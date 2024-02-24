using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShumenNews.Data;
using ShumenNews.Data.Models;

namespace ShumenNews.Services
{
    public class UserService : IUserService
    {
        private readonly ShumenNewsDbContext db;
        private readonly UserManager<ShumenNewsUser> userManager;

        public UserService(ShumenNewsDbContext db, UserManager<ShumenNewsUser> userManager)
        {
            this.db = db;
            this.userManager = userManager;
        }
        public void SetAuthorRoles()
        {
            var authors = db.Users.Where(u => u.UserArticles.Any(ua => ua.IsAuthor)).ToList();
            foreach (var author in authors)
            {
                userManager.AddToRoleAsync(author, "Author").GetAwaiter();
            }
            var a = 0;
        }
        public ShumenNewsUser GetUserByUserName(string userName)
        {
            var user = db.Users.SingleOrDefault(u => u.Email == userName)!;
            return user;
        }
        public ShumenNewsUser GetUserByEmail(string email)
        {
            var user = db.Users
                .Include(u => u.UserArticles)
                .SingleOrDefault(u => u.Email == email)!;
            return user;
        }
        public List<string> GetUserRoles(ShumenNewsUser user)
        {
            var userRoles = db.UserRoles.Where(ur => ur.UserId == user.Id)
                .Select(ur => ur.RoleId)
                .ToList();
            return userRoles;
        }
    }
}
