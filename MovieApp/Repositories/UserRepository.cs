using MovieApp.Interfaces;
using MovieApp.Models;

namespace MovieApp.Repositories
{
    public class UserRepository : IUserRepository
    {
        public bool doesUserExist(string username)
        {
            throw new NotImplementedException();
        }

        public ICollection<Show> GetFavoriteShows(int userId)
        {
            throw new NotImplementedException();
        }

        public ICollection<Tag> GetFavoriteTags(int userId)
        {
            throw new NotImplementedException();
        }

        public ICollection<Binge> GetUserBinges(int userId)
        {
            throw new NotImplementedException();
        }

        public bool isEmailConfirmed(int userId)
        {
            throw new NotImplementedException();
        }
    }
}
