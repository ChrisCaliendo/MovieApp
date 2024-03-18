using MovieApp.Models;

namespace MovieApp.Interfaces
{
    public interface IMovieRepository
    {
        //Read Methods
        ICollection<Show> GetShows();
        Show GetShow(int id);
        Show GetShow(string title);
        bool DoesShowExist(int id);
        ICollection<Tag> GetTagsOfShow(int id);

        //Edit Methods
        bool CreateShow(Show show);
        bool Save();
    }
}
