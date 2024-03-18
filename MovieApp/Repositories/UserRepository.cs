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

        public bool doesUserExist(string username)
        {
            return _context.Users.Any(s => s.name == username);
        }

        public bool doesUserExist(int id)
        {
            return _context.Users.Any(s => s.id == id);
        }

        public ICollection<User> GetAllUsers()
        {
            return _context.Users.OrderBy(t => t.id).ToList();
        }

        public User GetUser(int id)
        {
            return _context.Users.Where(s => s.id == id).FirstOrDefault();
        }

        public User GetUser(string name)
        {
            return _context.Users.Where(s => s.name == name).FirstOrDefault();
        }

        public ICollection<Show> GetFavoriteShows(int userId)
        {
            return _context.Users.Where(u => u.id == userId).Select(s => s.favoriteShows).FirstOrDefault();
        }

        public ICollection<Tag> GetFavoriteTags(int userId)
        {
            return _context.Users.Where(u => u.id == userId).Select(t => t.favoriteTags).FirstOrDefault();
        }

        public ICollection<Binge> GetUserBinges(int userId)
        {
            return _context.Users.Where(u => u.id == userId).Select(b => b.binges).FirstOrDefault();
        }
    }
}
