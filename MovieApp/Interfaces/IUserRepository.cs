using MovieApp.Models;

namespace MovieApp.Interfaces
{
    public interface IUserRepository
    {
        //Read Methods
        ICollection<User> GetAllUsers();
        User GetUser(int id);
        User GetUser(string name);
        bool DoesUserExist(int id);
        bool DoesUserExist(string username);
        ICollection<Show> GetFavoriteShows(int userId);
        ICollection<Tag> GetFavoriteTags(int userId);
        ICollection<Binge> GetUserBinges(int userId);

        //Edit Methods
        bool CreateUser(User user);
        bool Save();

    }
}
