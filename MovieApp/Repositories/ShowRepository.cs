using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MovieApp.Data;
using MovieApp.Interfaces;
using MovieApp.Models;

namespace MovieApp.Repositories
{
    /// <summary>
    /// Repository class for managing show-related database operations.
    /// </summary>
    public class ShowRepository : IShowRepository
    {
        private readonly DataContext _context;

        public ShowRepository(DataContext context) 
        {
            _context = context;
        }

        //Read Methods
        /// <summary>
        /// Checks if a show exists by its ID.
        /// </summary>
        /// <param name="id">The show's ID.</param>
        /// <returns>True if the show exists, otherwise false.</returns>
        public bool DoesShowExist(int id)
        {
            return _context.Shows.Any(s => s.Id == id);
        }

        /// <summary>
        /// Checks if a show has a specific tag.
        /// </summary>
        /// <param name="showId">The show's ID.</param>
        /// <param name="tagId">The tag's ID.</param>
        /// <returns>True if the show has the tag, otherwise false.</returns>
        public bool DoesShowHaveTag(int showId, int tagId)
        {
            return _context.ShowTags.Any(s => s.TagId == tagId && s.ShowId == showId);
        }

        /// <summary>
        /// Retrieves a show by its ID.
        /// </summary>
        /// <param name="id">The show's ID.</param>
        /// <returns>The show if found, otherwise null.</returns>
        public Show GetShow(int id)
        {
            return (Show)_context.Shows.Where(s => s.Id == id).FirstOrDefault();
        }

        /// <summary>
        /// Retrieves a show by its title.
        /// </summary>
        /// <param name="title">The title of the show.</param>
        /// <returns>The show if found, otherwise null.</returns>
        public Show GetShow(string title)
        {
            return (Show)_context.Shows.Where(s => s.Title == title).FirstOrDefault();
        }

        /// <summary>
        /// Retrieves all shows ordered by ID.
        /// </summary>
        /// <returns>A collection of shows.</returns>
        public ICollection<Show> GetShows()
        {
            return _context.Shows.OrderBy(s => s.Id).ToList();
        }

        /// <summary>
        /// Retrieves all tags associated with a show.
        /// </summary>
        /// <param name="showId">The show's ID.</param>
        /// <returns>A collection of tags.</returns>
        public ICollection<Tag> GetTagsOfShow(int showId)
        {
            return (ICollection<Tag>)_context.ShowTags.Where(x => x.ShowId == showId).Select(t => t.Tag).ToList();
        }

        //Edit Methods
        /// <summary>
        /// Creates a new show record.
        /// </summary>
        /// <param name="show">The show entity to add.</param>
        /// <returns>True if the operation succeeds, otherwise false.</returns>
        public bool CreateShow(Show show)
        {
            _context.Add(show);
            return Save();
        }

        /// <summary>
        /// Saves changes to the database.
        /// </summary>
        /// <returns>True if the operation succeeds, otherwise false.</returns>
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        /// <summary>
        /// Updates an existing show record.
        /// </summary>
        /// <param name="show">The show entity to update.</param>
        /// <returns>True if the operation succeeds, otherwise false.</returns>
        public bool UpdateShow(Show show)
        {
            _context.Update(show);
            return Save();
        }

        /// <summary>
        /// Adds a tag to a show.
        /// </summary>
        /// <param name="showId">The show's ID.</param>
        /// <param name="tagId">The tag's ID.</param>
        /// <returns>True if the operation succeeds, otherwise false.</returns>
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

        /// <summary>
        /// Removes a tag from a show.
        /// </summary>
        /// <param name="showId">The show's ID.</param>
        /// <param name="tagId">The tag's ID.</param>
        /// <returns>True if the operation succeeds, otherwise false.</returns>
        public bool RemoveTagFromShow(int showId, int tagId)
        {
            var showTag = _context.ShowTags.Where(x => x.ShowId == showId && x.TagId == tagId).FirstOrDefault();
            _context.ShowTags.Remove(showTag);
            return Save();
        }

        /// <summary>
        /// Removes all tags from a show.
        /// </summary>
        /// <param name="showId">The show's ID.</param>
        /// <returns>True if the operation succeeds, otherwise false.</returns>
        public bool RemoveAllTagsFromShow(int showId)
        {
            var showTags = _context.ShowTags.Where(x => x.ShowId == showId).ToList();
            _context.RemoveRange(showTags);
            return Save();
        }

        /// <summary>
        /// Removes a show from all binge records.
        /// </summary>
        /// <param name="showId">The show's ID.</param>
        /// <returns>True if the operation succeeds, otherwise false.</returns>
        public bool RemoveShowFromAllBinges(int showId)
        {
            var showBinges = _context.ShowBinges.Where(x => x.ShowId == showId).ToList();
            _context.RemoveRange(showBinges);
            return Save();
        }

        /// <summary>
        /// Deletes a show record along with its related tags and binge records.
        /// </summary>
        /// <param name="show">The show entity to delete.</param>
        /// <returns>True if the operation succeeds, otherwise false.</returns>
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
