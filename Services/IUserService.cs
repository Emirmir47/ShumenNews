using ShumenNews.Data.Models;

namespace ShumenNews.Services
{
    public interface IUserService
    {
        public AppUser GetUserByUserName(string userName);
    }
}
