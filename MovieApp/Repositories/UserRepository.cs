using MovieApp.Data;
using MovieApp.Interfaces;
using MovieApp.Models;

namespace MovieApp.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }

        //Read Methods

        public bool DoesUserExist(string username)
        {
            return _context.Users.Any(s => s.Name == username);
        }

        public bool DoesUserExist(int id)
        {
            return _context.Users.Any(s => s.Id == id);
        }

        public ICollection<User> GetAllUsers()
        {
            return _context.Users.OrderBy(t => t.Id).ToList();
        }

        public User GetUser(int id)
        {
            return _context.Users.Where(s => s.Id == id).FirstOrDefault();
        }

        public User GetUser(string name)
        {
            return _context.Users.Where(s => s.Name == name).FirstOrDefault();
        }

        public ICollection<Show> GetFavoriteShows(int userId)
        {
            return _context.FavoriteShows.Where(u => u.UserId == userId).Select(s => s.Show).ToList();
        }

        public ICollection<Tag> GetFavoriteTags(int userId)
        {
            return _context.FavoriteTags.Where(u => u.TagId == userId).Select(t => t.Tag).ToList();
        }

        public ICollection<Binge> GetUserBinges(int userId)
        {
            return _context.Users.Where(u => u.Id == userId).Select(b => b.Binges).FirstOrDefault();
        }

        //Edit Methods

        public bool CreateUser(User user)
        {
            _context.Add(user);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
