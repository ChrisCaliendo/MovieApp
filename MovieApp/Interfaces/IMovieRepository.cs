using MovieApp.Models;

namespace MovieApp.Interfaces
{
    public interface IMovieRepository
    {
        ICollection<Show> GetShows();
        Show GetShow(int id);
        Show GetShow(string title);
        bool doesShowExist(int id);

    }
}
