using ShumenNews.Data.Models;

namespace ShumenNews.Services
{
    public interface IUserService
    {
        public ShumenNewsUser GetUserByUserName(string userName);
    }
}
