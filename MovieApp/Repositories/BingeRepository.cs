using Microsoft.IdentityModel.Tokens;
using MovieApp.Data;
using MovieApp.Interfaces;
using MovieApp.Models;


namespace MovieApp.Repositories
{
    /// <summary>
    /// Repository class for managing binge-related database operations.
    /// </summary>
    public class BingeRepository : IBingeRepository
    {
        private readonly DataContext _context;

        /// <summary>
        /// Initializes a new instance of the BingeRepository class.
        /// </summary>
        /// <param name="context">The database context to interact with the data layer.</param>
        public BingeRepository(DataContext context)
        {
            _context = context;
        }

        //Read Methods
        /// <summary>
        /// Checks if a binge exists in the database.
        /// </summary>
        /// <param name="bingeId">The ID of the binge.</param>
        /// <returns>True if the binge exists, otherwise false.</returns>
        public bool DoesBingeExist(int bingeId)
        {
            return _context.Binges.Any(s => s.Id == bingeId);
        }

        /// <summary>
        /// Checks if a binge contains a specific show.
        /// </summary>
        /// <param name="bingeId">The binge ID.</param>
        /// <param name="showId">The show ID.</param>
        /// <returns>True if the show is in the binge, otherwise false.</returns>
        public bool DoesBingeHaveShow(int bingeId, int showId)
        {
            return _context.ShowBinges.Any(s => s.BingeId == bingeId && s.ShowId == showId);
        }

        /// <summary>
        /// Checks if a user owns a specific binge.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="bingeId">The binge ID.</param>
        /// <returns>True if the user owns the binge, otherwise false.</returns>
        public bool DoesUserHaveBinge(int userId, int bingeId)
        {
            return _context.Binges.Any(s => s.UserId == userId && s.Id == bingeId);
        }

        /// <summary>
        /// Retrieves a binge by its ID.
        /// </summary>
        /// <param name="bingeId">The binge ID.</param>
        /// <returns>The binge if found, otherwise null.</returns>
        public Binge GetBinge(int bingeId)
        {
            return _context.Binges.Where(s => s.Id == bingeId).FirstOrDefault();
        }

        /// <summary>
        /// Calculates the total timespan of all shows in a binge.
        /// </summary>
        /// <param name="bingeId">The binge ID.</param>
        /// <returns>The total timespan of the binge.</returns>
        public int GetBingeTimespan(int bingeId)
        {
            var ts = _context.ShowBinges.Where(s => s.BingeId == bingeId).Select(t => t.Show).ToList();
            if (ts.Count() <= 0) return 0;
            return (int)ts.Sum(t => t.Timespan);
        }

        /// <summary>
        /// Retrieves all public binges.
        /// </summary>
        /// <returns>A collection of public binges.</returns>
        public ICollection<Binge> GetPublicBinges()
        {
            return _context.Binges.OrderBy(t => t.Id).ToList();
        }

        /// <summary>
        /// Retrieves all shows in a binge.
        /// </summary>
        /// <param name="bingeId">The binge ID.</param>
        /// <returns>A collection of shows in the binge.</returns>
        public ICollection<Show> GetShowsInBinge(int bingeId)
        {
            return (ICollection<Show>)_context.ShowBinges.Where(x => x.BingeId == bingeId).Select(t => t.Show).ToList();
        }

        /// <summary>
        /// Retrieves all the shows associated with a specific binge by its unique identifier.
        /// </summary>
        /// <param name="bingeId">The unique identifier of the binge.</param>
        /// <returns>Returns a collection of Show objects related to the specified binge.</returns>
        public ICollection<Show> GetShowRelationsInBinge(int bingeId)
        {
            return (ICollection<Show>)_context.ShowBinges.Where(x => x.BingeId == bingeId).Select(t => t.Show).ToList();
        }

        /// <summary>
        /// Retrieves all tags associated with shows in a binge.
        /// </summary>
        /// <param name="bingeId">The binge ID.</param>
        /// <returns>A collection of tags related to the binge.</returns>
        public ICollection<Tag> GetTagsInBinge(int bingeId)
        {
            return (ICollection<Tag>)_context.ShowBinges.Where(x => x.BingeId == bingeId).SelectMany(b => b.Show.ShowTags).Select(s => s.Tag).Distinct().ToList();

        }

        /// <summary>
        /// Calculates the number of shows associated with a specific binge that have unknown or invalid timespan values.
        /// </summary>
        /// <param name="bingeId">The unique identifier of the binge.</param>
        /// <returns>Returns the count of shows with invalid or missing timespan information.</returns>
        public int GetUnknownTimespans(int bingeId)
        {
            return _context.ShowBinges.Where(s => s.BingeId == bingeId && (s.Show.Timespan < 0 || s.Show.Timespan == null)).Select(t => t.Show).ToList().Count();
        }

        /// <summary>
        /// Returns the total number of shows associated with a specific binge by its unique identifier.
        /// </summary>
        /// <param name="bingeId">The unique identifier of the binge.</param>
        /// <returns>Returns the total count of shows in the specified binge.</returns>
        public int GetShowCountInBinge(int bingeId)
        {
            return _context.ShowBinges.Where(x => x.BingeId == bingeId).Select(t => t.Show).Count();
        }

        /// <summary>
        /// Checks if a specific show is already associated with a binge by its unique identifiers.
        /// </summary>
        /// <param name="bingeId">The unique identifier of the binge.</param>
        /// <param name="showId">The unique identifier of the show.</param>
        /// <returns>Returns true if the show is associated with the binge, otherwise false.</returns>
        public bool IsShowInBinge(int bingeId, int showId)
        {
            return _context.ShowBinges.Any(u => u.BingeId == bingeId && u.ShowId == showId);
        }



        //Edit Methods
        /// <summary>
        /// Creates a new binge and associates it with an author.
        /// </summary>
        /// <param name="binge">The binge entity.</param>
        /// <param name="authorId">The author ID.</param>
        /// <returns>True if the operation succeeds, otherwise false.</returns>
        public bool CreateBinge(Binge binge, int authorId)
        {
            var user = _context.Users.Where(i => i.Id == authorId).FirstOrDefault();
            user.Binges.Add(binge);

            _context.Add(binge);
            _context.Update(user);
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
        /// Updates an existing binge.
        /// </summary>
        /// <param name="binge">The binge entity.</param>
        /// <returns>True if the operation succeeds, otherwise false.</returns>
        public bool UpdateBinge(Binge binge)
        {
            _context.Update(binge);
            return Save();
        }

        /// <summary>
        /// Adds a show to a binge, associating it with the specified binge and show identifiers.
        /// </summary>
        /// <param name="bingeId">The unique identifier of the binge.</param>
        /// <param name="showId">The unique identifier of the show to add.</param>
        /// <returns>Returns true if the show was successfully added, otherwise false.</returns>
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

        /// <summary>
        /// Removes a show from a binge by its unique identifiers.
        /// </summary>
        /// <param name="bingeId">The unique identifier of the binge.</param>
        /// <param name="showId">The unique identifier of the show to remove.</param>
        /// <returns>Returns true if the show was successfully removed, otherwise false.</returns>
        public bool RemoveShowFromBinge(int bingeId, int showId)
        {
            var showBinge = _context.ShowBinges.Where(x => x.ShowId == showId && x.BingeId == bingeId).FirstOrDefault();
            _context.Remove(showBinge);
            return Save();
        }

        /// <summary>
        /// Removes all shows associated with a specific binge by its unique identifier.
        /// </summary>
        /// <param name="bingeId">The unique identifier of the binge.</param>
        /// <returns>Returns true if all shows were successfully removed, otherwise false.</returns>
        public bool RemoveAllShowsFromBinge(int bingeId)
        {
            var showBinges = _context.ShowBinges.Where(x => x.BingeId == bingeId).ToList();
            if (showBinges.IsNullOrEmpty()) return true;
            _context.RemoveRange(showBinges);
            return Save();
        }

        /// <summary>
        /// Deletes a binge.
        /// </summary>
        /// <param name="binge">The binge entity to delete.</param>
        /// <returns>True if the operation succeeds, otherwise false.</returns>
        public bool DeleteBinge(Binge binge)
        {
            _context.Remove(binge);
            return Save();
        }

        /// <summary>
        /// Deletes a list of binges from the database.
        /// </summary>
        /// <param name="binges">The list of binge objects to delete.</param>
        /// <returns>Returns true if the binges were successfully deleted, otherwise false.</returns>
        public bool DeleteBinges(List<Binge> binges)
        {
            _context.RemoveRange(binges);
            return Save();
        }
    }
}
