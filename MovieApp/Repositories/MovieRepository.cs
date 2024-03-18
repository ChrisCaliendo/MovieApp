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

        public bool doesShowExist(int id)
        {
            return _context.Shows.Any(s => s.id == id);
        }

        public Show GetShow(int id)
        {
            return _context.Shows.Where(s => s.id == id).FirstOrDefault();
        }

        public Show GetShow(string title)
        {
            return _context.Shows.Where(s => s.title == title).FirstOrDefault();
        }

        public ICollection<Show> GetShows()
        {
            return _context.Shows.OrderBy(s => s.id).ToList();
        }

    }
}
