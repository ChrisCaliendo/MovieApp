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
        bool IsShowAFavoriteShowOfUser(int userId, int showId);
        ICollection<Show> GetFavoriteShows(int userId);
        ICollection<FavoriteShow> GetFavoriteShowsRelations(int userId);
        ICollection<Binge> GetUserBinges(int userId);
        FavoriteShow GetFavoriteShow(int userId, int showId);

        //Edit Methods
        bool CreateUser(User user);
        bool UpdateUser(User user);
        bool DeleteUser(User user);
        bool DeleteFavoriteShowList(List<FavoriteShow> favoriteShows);


        bool AddBingeToUser(int userId, Binge binge);
        bool RemoveBingeFromUser(Binge binge);

        bool AddToFavoriteShows(int userId, int showId);
        bool RemoveFromFavoriteShows(FavoriteShow favoriteShow);

        bool Save();

    }
}
