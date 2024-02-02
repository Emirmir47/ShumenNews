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
        public AppUser GetUserByUserName(string userName)
        {
            var user = db.Users.SingleOrDefault(u => u.Email == userName)!;
            return user;
        }
    }
}
