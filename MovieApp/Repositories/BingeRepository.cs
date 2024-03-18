using MovieApp.Data;
using MovieApp.Interfaces;
using MovieApp.Models;

namespace MovieApp.Repositories
{
    public class BingeRepository : IBingeRepository
    {
        private readonly DataContext _context;

        public BingeRepository(DataContext context)
        {
            _context = context;
        }

        public bool doesBingeExist(int bingeId)
        {
            return _context.Binges.Any(s => s.id == bingeId);
        }

        public Binge GetBinge(int bingeId)
        {
            return _context.Binges.Where(s => s.id == bingeId).FirstOrDefault();
        }

        public int GetBingeTimespan(int bingeId)
        {
            var ts = _context.Binges.Where(s => s.id == bingeId);
            if (ts.Count() <= 0) return 0;
            return (int)ts.Sum(t => t.timespan);
        }

        public ICollection<Binge> GetPublicBinges()
        {
            return _context.Binges.OrderBy(t => t.id).ToList();
        }

        public ICollection<Show> GetShowsInBinge(int bingeId)
        {
            return _context.ShowBinges.Where(x => x.bingeId == bingeId).Select(t => t.show).ToList();
        }

        public int GetUnknownTimespans(int bingeId)
        {
            var ts = _context.Binges.Where(s => s.id == bingeId && (s.timespan < 0 || s.timespan == null));
            return ts.Count();
        }

    }
}
