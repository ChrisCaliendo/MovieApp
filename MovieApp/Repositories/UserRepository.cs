using Azure;
using MovieApp.Data;
using MovieApp.Interfaces;
using MovieApp.Models;
using System.Reflection;

namespace MovieApp.Repositories
{
    /// <summary>
    /// Repository class for managing user-related database operations.
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        /// <summary>
        /// Initializes a new instance of the UserRepository class.
        /// </summary>
        /// <param name="context">The database context to interact with the data layer.</param>
        public UserRepository(DataContext context)
        {
            _context = context;
        }

        //Read Methods
        /// <summary>
        /// Checks if a user with the specified username exists in the database.
        /// </summary>
        /// <param name="username">The username to check.</param>
        /// <returns>Returns true if the user exists, otherwise false.</returns>
        public bool DoesUserExist(string username)
        {
            return _context.Users.Any(s => s.Name == username);
        }

        /// <summary>
        /// Checks if a user with the specified ID exists in the database.
        /// </summary>
        /// <param name="id">The user ID to check.</param>
        /// <returns>Returns true if the user exists, otherwise false.</returns>
        public bool DoesUserExist(int id)
        {
            return _context.Users.Any(s => s.Id == id);
        }

        /// <summary>
        /// Checks if a show is a favorite show of a specific user.
        /// </summary>
        /// <param name="userId">The user's ID.</param>
        /// <param name="showId">The show's ID.</param>
        /// <returns>Returns true if the show is in the user's favorite list, otherwise false.</returns>
        public bool IsShowAFavoriteShowOfUser(int userId, int showId)
        {
            return _context.FavoriteShows.Any(s => s.UserId == userId && s.ShowId == showId);
        }

        /// <summary>
        /// Retrieves all users from the database.
        /// </summary>
        /// <returns>A collection of all users ordered by ID.</returns>
        public ICollection<User> GetAllUsers()
        {
            return _context.Users.OrderBy(t => t.Id).ToList();
        }

        /// <summary>
        /// Retrieves a user by their unique identifier.
        /// </summary>
        /// <param name="id">The ID of the user.</param>
        /// <returns>The user if found, otherwise null.</returns>
        public User GetUser(int id)
        {
            return _context.Users.Where(s => s.Id == id).FirstOrDefault();
        }

        /// <summary>
        /// Retrieves a user by their name.
        /// </summary>
        /// <param name="name">The name of the user.</param>
        /// <returns>The user if found, otherwise null.</returns>
        public User GetUser(string name)
        {
            return _context.Users.Where(s => s.Name == name).FirstOrDefault();
        }

        /// <summary>
        /// Retrieves a list of a user's favorite shows.
        /// </summary>
        /// <param name="userId">The user's ID.</param>
        /// <returns>A collection of favorite shows.</returns>
        public ICollection<Show> GetFavoriteShows(int userId)
        {
            return _context.FavoriteShows.Where(u => u.UserId == userId).Select(s => s.Show).ToList();
        }

        /// <summary>
        /// Retrieves a list of binges associated with a user.
        /// </summary>
        /// <param name="userId">The user's ID.</param>
        /// <returns>A collection of binges.</returns>
        public ICollection<Binge> GetUserBinges(int userId)
        {
            return (ICollection<Binge>)_context.Binges.Where(u => u.UserId == userId).ToList();
        }

        /// <summary>
        /// Retrieves a specific favorite show for a user.
        /// </summary>
        /// <param name="userId">The user's ID.</param>
        /// <param name="showId">The show's ID.</param>
        /// <returns>The favorite show if found, otherwise null.</returns>
        public FavoriteShow GetFavoriteShow(int userId, int showId)
        {
            return _context.FavoriteShows.Where(u => u.UserId == userId && u.ShowId == showId).FirstOrDefault();
        }

        /// <summary>
        /// Retrieves all favorite show relations for a user.
        /// </summary>
        /// <param name="userId">The user's ID.</param>
        /// <returns>A collection of favorite show relations.</returns>
        public ICollection<FavoriteShow> GetFavoriteShowsRelations(int userId)
        {
            return _context.FavoriteShows.Where(u => u.UserId == userId).ToList();
        }

        //Edit Methods
        /// <summary>
        /// Creates a new user in the database.
        /// </summary>
        /// <param name="user">The user entity to create.</param>
        /// <returns>Returns true if the operation succeeds, otherwise false.</returns>
        public bool CreateUser(User user)
        {
            _context.Add(user);
            return Save();
        }

        /// <summary>
        /// Saves changes to the database.
        /// </summary>
        /// <returns>Returns true if changes were successfully saved, otherwise false.</returns>
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        /// <summary>
        /// Updates an existing user in the database.
        /// </summary>
        /// <param name="user">The user entity with updated values.</param>
        /// <returns>Returns true if the operation succeeds, otherwise false.</returns>
        public bool UpdateUser(User user)
        {
            _context.Update(user);
            return Save();
        }

        /// <summary>
        /// Adds a binge record for a user.
        /// </summary>
        /// <param name="binge">The binge entity to add.</param>
        /// <returns>Returns true if the operation succeeds, otherwise false.</returns>
        public bool AddBingeToUser(Binge binge)
        {
            _context.Add(binge);
            return Save();
        }

        /// <summary>
        /// Removes a binge record from a user.
        /// </summary>
        /// <param name="binge">The binge entity to remove.</param>
        /// <returns>Returns true if the operation succeeds, otherwise false.</returns>
        public bool RemoveBingeFromUser(Binge binge)
        {
            _context.Remove(binge);
            return Save();
        }

        /// <summary>
        /// Adds a show to the user's favorite list.
        /// </summary>
        /// <param name="userId">The user's ID.</param>
        /// <param name="showId">The show's ID.</param>
        /// <returns>Returns true if the operation succeeds, otherwise false.</returns>
        public bool AddToFavoriteShows(int userId, int showId)
        {
            var show = _context.Shows.Where(i => i.Id == showId).FirstOrDefault();
            var user = _context.Users.Where(i => i.Id == userId).FirstOrDefault();

            var favoriteShow = new FavoriteShow()
            {
                Show = show,
                ShowId = showId,
                UserId = userId
            };

            _context.Add(favoriteShow);
            return Save();
        }

        /// <summary>
        /// Removes a show from the user's favorite list.
        /// </summary>
        /// <param name="favoriteShow">The favorite show entity to remove.</param>
        /// <returns>Returns true if the operation succeeds, otherwise false.</returns>
        public bool RemoveFromFavoriteShows(FavoriteShow favoriteShow)
        {
            _context.Remove(favoriteShow);
            return Save();
        }

        /// <summary>
        /// Deletes a list of favorite shows from the user's favorite list.
        /// </summary>
        /// <param name="favoriteShows">The list of favorite shows to delete.</param>
        /// <returns>Returns true if the operation succeeds, otherwise false.</returns>
        public bool DeleteFavoriteShowList(List<FavoriteShow> favoriteShows)
        {
            _context.RemoveRange(favoriteShows);
            return Save();
        }

        /// <summary>
        /// Deletes a user from the database.
        /// </summary>
        /// <param name="user">The user entity to delete.</param>
        /// <returns>Returns true if the operation succeeds, otherwise false.</returns>
        public bool DeleteUser(User user)
        {
            _context.Remove(user);
            return Save();
        }
    }
}
