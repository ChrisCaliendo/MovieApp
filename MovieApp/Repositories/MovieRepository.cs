using MovieApp.Data;
using MovieApp.Interfaces;
using MovieApp.Models;

namespace MovieApp.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly DataContext _context;

        public MovieRepository(DataContext context) 
        {
            _context = context;
        }

        //Read Methods

        public bool DoesShowExist(int id)
        {
            return _context.Shows.Any(s => s.Id == id);
        }

        public Show GetShow(int id)
        {
            return (Show)_context.Shows.Where(s => s.Id == id).FirstOrDefault();
        }

        public Show GetShow(string title)
        {
            return (Show)_context.Shows.Where(s => s.Title == title).FirstOrDefault();
        }

        public ICollection<Show> GetShows()
        {
            return _context.Shows.OrderBy(s => s.Id).ToList();
        }

        public ICollection<Tag> GetTagsOfShow(int showId)
        {
            return (ICollection<Tag>)_context.ShowTags.Where(x => x.ShowId == showId).Select(t => t.Tag).ToList();
        }

        //Edit Methods

        public bool CreateShow(Show show)
        {
            _context.Add(show);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
