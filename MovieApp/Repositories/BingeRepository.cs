using Microsoft.IdentityModel.Tokens;
using MovieApp.Data;
using MovieApp.Interfaces;
using MovieApp.Models;
using System.Reflection;

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

        public bool DoesBingeExist(int bingeId)
        {
            return _context.Binges.Any(s => s.Id == bingeId);
        }

        public bool DoesBingeHaveShow(int bingeId, int showId)
        {
            return _context.ShowBinges.Any(s => s.BingeId == bingeId && s.ShowId == showId);
        }
        public bool DoesUserHaveBinge(int userId, int bingeId)
        {
            return _context.Binges.Any(s => s.UserId == userId && s.Id == bingeId);
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

        public ICollection<Show> GetShowRelationsInBinge(int bingeId)
        {
            return (ICollection<Show>)_context.ShowBinges.Where(x => x.BingeId == bingeId).Select(t => t.Show).ToList();
        }

        public ICollection<Tag> GetTagsInBinge(int bingeId)
        {
            return (ICollection<Tag>)_context.ShowBinges.Where(x => x.BingeId == bingeId).SelectMany(b => b.Show.ShowTags).Select(s => s.Tag).Distinct().ToList();

        }

        public int GetUnknownTimespans(int bingeId)
        {
            return _context.ShowBinges.Where(s => s.BingeId == bingeId && (s.Show.Timespan < 0 || s.Show.Timespan == null)).Select(t => t.Show).ToList().Count();
        }

        public bool IsShowInBinge(int bingeId, int showId)
        {
            return _context.ShowBinges.Any(u => u.BingeId == bingeId && u.ShowId == showId);
        }

        //Edit Methods

        public bool CreateBinge(Binge binge, int authorId)
        {
            var user = _context.Users.Where(i => i.Id == authorId).FirstOrDefault();
            user.Binges.Add(binge);

            _context.Add(binge);
            _context.Update(user);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
        
        public bool UpdateBinge(Binge binge)
        {
            _context.Update(binge);
            return Save();
        }

        public bool AddShowToBinge(int bingeId, int showId)
        {
            var show = _context.Shows.Where(i => i.Id == showId).FirstOrDefault();
            var binge = _context.Binges.Where(i => i.Id == bingeId).FirstOrDefault();

            var showBinge = new ShowBinge()
            {
                Show = show,
                Binge = binge,
                ShowId = showId,
                BingeId = bingeId
            };

            _context.Add(showBinge);
            return Save();
        }

        public bool RemoveShowFromBinge(int bingeId, int showId)
        {
            var showBinge = _context.ShowBinges.Where(x => x.ShowId == showId && x.BingeId == bingeId).FirstOrDefault();
            _context.Remove(showBinge);
            return Save();
        }

        public bool RemoveAllShowsFromBinge(int bingeId)
        {
            var showBinges = _context.ShowBinges.Where(x => x.BingeId == bingeId).ToList();
            if (showBinges.IsNullOrEmpty()) return true;
            _context.RemoveRange(showBinges);
            return Save();
        }

        public bool DeleteBinge(Binge binge)
        {
            _context.Remove(binge);
            return Save();
        }

        public bool DeleteBinges(List<Binge> binges)
        {
            _context.RemoveRange(binges);
            return Save();
        }
    }
}
