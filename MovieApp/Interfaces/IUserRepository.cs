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
        ICollection<Binge> GetUserBinges(int userId);

        //Edit Methods
        bool CreateUser(User user);
        bool UpdateUser(User user);

        bool AddBingeToUser(int userId, Binge binge);
        bool RemoveBingeFromUser(int userId, Binge binge);

        bool AddToFavoriteShows(int userId, int showId);
        bool RemoveFromFavoriteShows(int userId, int showId);

        bool Save();

    }
}
