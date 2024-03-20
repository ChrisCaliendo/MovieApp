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

        //Read Methods

        public bool doesBingeExist(int bingeId)
        {
            return _context.Binges.Any(s => s.Id == bingeId);
        }

        public Binge GetBinge(int bingeId)
        {
            return _context.Binges.Where(s => s.Id == bingeId).FirstOrDefault();
        }

        public int GetBingeTimespan(int bingeId)
        {
            var ts = _context.ShowBinges.Where(s => s.BingeId == bingeId).Select(t => t.Show).ToList();
            if (ts.Count() <= 0) return 0;
            return (int)ts.Sum(t => t.Timespan);
        }

        public ICollection<Binge> GetPublicBinges()
        {
            return _context.Binges.OrderBy(t => t.Id).ToList();
        }

        public ICollection<Show> GetShowsInBinge(int bingeId)
        {
            return (ICollection<Show>)_context.ShowBinges.Where(x => x.BingeId == bingeId).Select(t => t.Show).ToList();
        }

        public int GetUnknownTimespans(int bingeId)
        {
            var ts = _context.ShowBinges.Where(s => s.BingeId == bingeId && (s.Show.Timespan < 0 || s.Show.Timespan == null)).Select(t => t.Show).ToList();
            return ts.Count();
        }

        //Edit Methods

        public bool CreateBinge(Binge binge)
        {
            _context.Add(binge);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
