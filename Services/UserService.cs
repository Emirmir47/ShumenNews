using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShumenNews.Data;
using ShumenNews.Data.Models;
using ShumenNews.Models.ViewModels;

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
        public void UpdateUserRoles(UserViewModel user)
        {
            var userDb = GetUserByEmail(user.Email);
            var allRoles = db.Roles.Select(r=>r.Name).ToList();
            foreach (var role in allRoles)
            {
                if (user.Roles.Where(r=>r.IsChecked)
                    .Select(r=>r.Name).Contains(role)
                    && !userManager.IsInRoleAsync(userDb, role).Result)
                {
                    //If this user has (role) in userViewModel
                    //and it is not contained in userDb 
                    //then the (role) will be added!
                    userManager.AddToRoleAsync(userDb, role)
                        .GetAwaiter()
                        .GetResult();
                }
                else if (user.Roles.Where(r => r.IsChecked == false)
                    .Select(r => r.Name).Contains(role)
                    && userManager.IsInRoleAsync(userDb, role).Result)
                {
                    //If the admin set property isChecked to false
                    //of some user's role
                    //then the (role) will be removed!
                    userManager.RemoveFromRoleAsync(userDb, role)
                        .GetAwaiter()
                        .GetResult();
                }
            }
        }
        public void BlockUser(UserViewModel user)
        {
            var userDb = GetUserByEmail(user.Email);
            double blockTime = Convert.ToDouble(user.BlockTime);
            userManager.SetLockoutEndDateAsync(userDb, DateTime.UtcNow.AddDays(blockTime))
                .GetAwaiter().GetResult();
        }
        public void UnblockUser(UserViewModel user)
        {
            var userDb = GetUserByEmail(user.Email);
            userManager.SetLockoutEndDateAsync(userDb, null)
                .GetAwaiter().GetResult();
        }
    }
}
