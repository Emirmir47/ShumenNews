using ShumenNews.Data.Models;

namespace ShumenNews.Services
{
    public interface IUserService
    {
        public ShumenNewsUser GetUserByUserName(string userName);
        public ShumenNewsUser GetUserByEmail(string email);
        public List<string> GetUserRoles(ShumenNewsUser user);
        public void SetAuthorRoles();
    }
}
