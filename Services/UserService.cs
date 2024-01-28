using ShumenNews.Data;
using ShumenNews.Data.Models;

namespace ShumenNews.Services
{
    public class UserService : IUserService
    {
        private readonly ShumenNewsDbContext db;

        public UserService(ShumenNewsDbContext db)
        {
            this.db = db;
        }
        public ShumenNewsUser GetUserByUserName(string userName)
        {
            var user = db.Users.SingleOrDefault(u => u.Email == userName)!;
            return user;
        }
    }
}
