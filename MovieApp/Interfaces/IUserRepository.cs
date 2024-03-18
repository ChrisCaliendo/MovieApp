using MovieApp.Models;

namespace MovieApp.Interfaces
{
    public interface IUserRepository
    {

        ICollection<User> GetAllUsers();
        
        User GetUser(int id);

        User GetUser(string name);

        bool doesUserExist(int id);

        bool doesUserExist(string username);

        ICollection<Show> GetFavoriteShows(int userId);

        ICollection<Tag> GetFavoriteTags(int userId);

        ICollection<Binge> GetUserBinges(int userId);

    }
}
