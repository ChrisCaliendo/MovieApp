using MovieApp.Data;
using MovieApp.Interfaces;
using MovieApp.Models;

namespace MovieApp.Repositories
{
    /// <summary>
    /// Provides data access methods for managing tags in the database, including CRUD operations.
    /// </summary>
    public class TagRepository : ITagRepository
    {
        private readonly DataContext _context;

        public TagRepository(DataContext context)
        {
            _context = context;
        }

        //Read Methods
        /// <summary>
        /// Checks if a tag with the specified unique identifier exists in the database.
        /// </summary>
        /// <param name="id">The unique identifier of the tag.</param>
        /// <returns>Returns true if the tag exists, otherwise false.</returns>
        public bool DoesTagExist(int id)
        {
            return _context.Tags.Any(t => t.Id == id);
        }

        /// <summary>
        /// Checks if a tag with the specified name exists in the database.
        /// </summary>
        /// <param name="name">The name of the tag.</param>
        /// <returns>Returns true if the tag exists, otherwise false.</returns>
        public bool DoesTagExist(string name)
        {
            return _context.Tags.Any(t => t.Name == name);
        }

        /// <summary>
        /// Retrieves a tag based on its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the tag.</param>
        /// <returns>Returns the tag with the specified id, or null if not found.</returns>
        public Tag GetTag(int id)
        {
            return _context.Tags.Where(t => t.Id == id).FirstOrDefault();
        }

        /// <summary>
        /// Retrieves a tag based on its name.
        /// </summary>
        /// <param name="name">The name of the tag.</param>
        /// <returns>Returns the tag with the specified name, or null if not found.</returns>
        public Tag GetTag(string name)
        {
            return _context.Tags.Where(t => t.Name == name).FirstOrDefault();
        }

        /// <summary>
        /// Retrieves a collection of shows associated with a specific tag.
        /// </summary>
        /// <param name="tagId">The unique identifier of the tag.</param>
        /// <returns>Returns a collection of show objects associated with the tag, or an empty collection if none are found.</returns>
        public ICollection<Show> GetShowsWithTag(int tagId)
        {
            return (ICollection<Show>)_context.ShowTags.Where(x => x.TagId == tagId).Select(t => t.Show).ToList();
        }

        /// <summary>
        /// Retrieves a collection of all tags in the database, ordered by their identifier.
        /// </summary>
        /// <returns>Returns a collection of all tags, ordered by their Id.</returns>
        public ICollection<Tag> GetTags()
        {
            return _context.Tags.OrderBy(t => t.Id).ToList();
        }

        //Edit Methods
        /// <summary>
        /// Creates a new tag and saves it to the database.
        /// </summary>
        /// <param name="tag">The tag object to be created.</param>
        /// <returns>Returns true if the tag was successfully created; otherwise false.</returns>
        public bool CreateTag(Tag tag)
        {
            _context.Add(tag);
            return Save();
        }

        /// <summary>
        /// Updates an existing tag in the database.
        /// </summary>
        /// <param name="tag">The tag object with updated values.</param>
        /// <returns>Returns true if the tag was successfully updated; otherwise false.</returns>
        public bool UpdateTag(Tag tag)
        {
            _context.Update(tag);
            return Save();
        }

        /// <summary>
        /// Deletes an existing tag from the database.
        /// </summary>
        /// <param name="tag">The tag object to be deleted.</param>
        /// <returns>Returns true if the tag was successfully deleted; otherwise false.</returns>
        public bool DeleteTag(Tag tag)
        {
            _context.Remove(tag);
            return Save();
        }

        /// <summary>
        /// Removes a specific tag from all shows it is associated with.
        /// </summary>
        /// <param name="tagId">The unique identifier of the tag to be removed from all shows.</param>
        /// <returns>Returns true if the tag was successfully removed from all shows; otherwise false.</returns>
        public bool RemoveTagFromAllShows(int tagId)
        {
            var showTags = _context.ShowTags.Where(x => x.TagId == tagId).ToList();
            _context.RemoveRange(showTags);
            return Save();
        }

        /// <summary>
        /// Saves any changes made to the context (database).
        /// </summary>
        /// <returns>Returns true if changes were successfully saved; otherwise false.</returns>
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        
    }
}
