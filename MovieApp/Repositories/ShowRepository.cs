using MovieApp.Data;
using MovieApp.Interfaces;
using MovieApp.Models;

namespace MovieApp.Repositories
{
    public class ShowRepository : IShowRepository
    {
        private readonly DataContext _context;

        public ShowRepository(DataContext context) 
        {
            _context = context;
        }

        //Read Methods

        public bool DoesShowExist(int id)
        {
            return _context.Shows.Any(s => s.Id == id);
        }

        public bool DoesShowHaveTag(int showId, int tagId)
        {
            return _context.ShowTags.Any(s => s.TagId == tagId && s.ShowId == showId);
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

        public bool UpdateShow(Show show)
        {
            _context.Update(show);
            return Save();
        }

        public bool AddTagToShow(int showId, int tagId)
        {
            var show = _context.Shows.Where(i => i.Id == showId).FirstOrDefault();
            var tag = _context.Tags.Where(i => i.Id == tagId).FirstOrDefault();

            var showTag = new ShowTag()
            {
                Show = show,
                Tag = tag,
                ShowId = showId,
                TagId = tagId
            };

            _context.Add(showTag);
            return Save();
        }

        public bool RemoveTagFromShow(ShowTag showTag)
        {
            _context.ShowTags.Remove(showTag);
            return Save();
        }

        public bool DeleteShow(Show show)
        {
            var showBinges = _context.ShowBinges.Where(x => x.ShowId == show.Id).ToList();
            var showTags = _context.ShowTags.Where(x => x.ShowId == show.Id).ToList();
            _context.RemoveRange(showTags);
            _context.RemoveRange(showBinges);
            _context.Remove(show);
            return Save();
        }
    }
}
