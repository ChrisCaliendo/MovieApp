using MovieApp.Data;
using MovieApp.Interfaces;
using MovieApp.Models;

namespace MovieApp.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly DataContext _context;

        public TagRepository(DataContext context)
        {
            _context = context;
        }

        //Read Methods

        public bool DoesTagExist(int id)
        {
            return _context.Tags.Any(t => t.Id == id);
        }

        public bool DoesTagExist(string name)
        {
            return _context.Tags.Any(t => t.Name == name);
        }

        public Tag GetTag(int id)
        {
            return _context.Tags.Where(t => t.Id == id).FirstOrDefault();
        }

        public Tag GetTag(string name)
        {
            return _context.Tags.Where(t => t.Name == name).FirstOrDefault();
        }

        public ICollection<Show> GetShowsWithTag(int tagId)
        {
            return (ICollection<Show>)_context.ShowTags.Where(x => x.TagId == tagId).Select(t => t.Show).ToList();
        }

        public ICollection<Tag> GetTags()
        {
            return _context.Tags.OrderBy(t => t.Id).ToList();
        }

        //Edit Methods

        public bool CreateTag(Tag tag)
        {
            _context.Add(tag);
            return Save();
        }

        public bool UpdateTag(Tag tag)
        {
            _context.Update(tag);
            return Save();
        }

        public bool DeleteTag(Tag tag)
        {
            var showTags = _context.ShowTags.Where(x => x.TagId == tag.Id).ToList();
            _context.RemoveRange(showTags);
            _context.Remove(tag);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        
    }
}
