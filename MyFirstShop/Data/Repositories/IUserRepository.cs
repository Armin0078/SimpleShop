using MyFirstShop.Models;

namespace MyFirstShop.Data.Repositories
{
    public interface IUserRepository
    {
        bool isExistByUserEmail(string email);

        bool isExistByUserName(string userName);
        void addUser(Users user );

        Users GetUserByLogin(string email , string password);
    }

    public class UserRepository : IUserRepository
    {
        private MyFirstShopContext _context;

        public UserRepository(MyFirstShopContext context)
        {
            _context = context;
        }

        public void addUser(Users user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public Users GetUserByLogin(string email, string password)
        {
            return _context.Users.SingleOrDefault(c => c.Email == email && c.Password == password);
        }

        public bool isExistByUserEmail(string email)
        {
            return _context.Users.Any(u => u.Email == email);
        }

        public bool isExistByUserName(string userName)
        {
            return _context.Users.Any( u => u.UserName == userName);
        }
    }
}
