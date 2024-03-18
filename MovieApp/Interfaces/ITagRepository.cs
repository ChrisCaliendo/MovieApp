using MovieApp.Models;

namespace MovieApp.Interfaces
{
    public interface ITagRepository
    {
        ICollection<Tag> GetTags();
        Tag GetTag(int id);
        Tag GetTag(string name);
        bool doesTagExist(int id);
        bool doesTagExist(string name);
        ICollection<Show> GetShowsWithTag(int tagId);

    }
}
