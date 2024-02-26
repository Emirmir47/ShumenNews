using ShumenNews.Data.Models;
using ShumenNews.Models.ViewModels;

namespace ShumenNews.Services
{
    public interface IUserService
    {
        public ShumenNewsUser GetUserByUserName(string userName);
        public ShumenNewsUser GetUserByEmail(string email);
        public List<string> GetUserRoles(ShumenNewsUser user);
        public void UpdateUserRoles(UserViewModel user);
        public void BlockUser(UserViewModel user);
        public void UnblockUser(UserViewModel user);
    }
}
