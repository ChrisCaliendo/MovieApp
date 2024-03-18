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

        public bool doesTagExist(int id)
        {
            return _context.Tags.Any(t => t.id == id);
        }

        public bool doesTagExist(string name)
        {
            return _context.Tags.Any(t => t.name == name);
        }

        public Tag GetTag(int id)
        {
            return _context.Tags.Where(t => t.id == id).FirstOrDefault();
        }

        public Tag GetTag(string name)
        {
            return _context.Tags.Where(t => t.name == name).FirstOrDefault();
        }

        public ICollection<Show> GetShowsWithTag(int tagId)
        {
            return _context.ShowTags.Where(x => x.tagId == tagId).Select(t => t.show).ToList();
        }

        public ICollection<Tag> GetTags()
        {
            return _context.Tags.OrderBy(t => t.id).ToList();
        }
    }
}
