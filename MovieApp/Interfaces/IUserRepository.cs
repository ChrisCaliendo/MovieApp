using MovieApp.Models;

namespace MovieApp.Interfaces
{
    public interface IUserRepository
    {
        
        bool doesUserExist(string username);

        bool isEmailConfirmed(int userId);

        ICollection<Show> GetFavoriteShows(int userId);

        ICollection<Tag> GetFavoriteTags(int userId);

        ICollection<Binge> GetUserBinges(int userId);

    }
}
