using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MovieApp.Dto;
using MovieApp.Interfaces;
using MovieApp.Models;
using MovieApp.Repositories;
using MovieApp.Infastructure;
using Microsoft.AspNetCore.Authorization;

namespace MovieApp.Controllers
{
    /// <summary>
    /// Controller for managing user operations including login, creation, and retrieval.
    /// </summary>
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    [ApiController]
    [Authorize]



    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IShowRepository _showRepository;
        private readonly IBingeRepository _bingeRepository;
        private readonly IMapper _mapper;
        private readonly TokenProvider _tokenProvider;

        public UserController(IUserRepository userRepository, IShowRepository showRepository, IBingeRepository bingeRepository, IMapper mapper, TokenProvider tokenProvider)
        {
            _userRepository = userRepository;
            _bingeRepository = bingeRepository;
            _showRepository = showRepository;
            _mapper = mapper;
            _tokenProvider = tokenProvider;
        }

        //Get Requests
        /// <summary>
        /// Retrieves all users from the system.
        /// </summary>
        /// <returns>Returns a list of all users.</returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<User>))]
        
        public IActionResult GetAllUsers()
        {
            var tags = _mapper.Map<List<UserDto>>(_userRepository.GetAllUsers());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(tags);
        }

        /// <summary>
        /// Retrieves a user by their unique identifier.
        /// </summary>
        /// <param name="userId">The unique identifier of the user to retrieve.</param>
        /// <returns>Returns the user data if found, otherwise returns NotFound.</returns>
        [HttpGet("byId/{userId}")]
        [ProducesResponseType(200, Type = typeof(Show))]
        [ProducesResponseType(400)]

        public IActionResult getUser(int userId)
        {
            if (!_userRepository.DoesUserExist(userId))
                return NotFound();

            var shows = _mapper.Map<UserDto>(_userRepository.GetUser(userId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(shows);
        }

        /// <summary>
        /// Retrieves a user by their unique username.
        /// </summary>
        /// <param name="userId">The unique username of the user to retrieve.</param>
        /// <returns>Returns the user data if found, otherwise returns NotFound.</returns>
        [HttpGet("byName/{userId}")]
        [ProducesResponseType(200, Type = typeof(User))]
        [ProducesResponseType(400)]

        public IActionResult getUser(string userId)
        {
            if (!_userRepository.DoesUserExist(userId))
                return NotFound();

            var shows = _mapper.Map<UserDto>(_userRepository.GetUser(userId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(shows);
        }

        /// <summary>
        /// Retrieves all binges associated with a user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user to retrieve binges for.</param>
        /// <returns>Returns a list of all binges associated with the user.</returns>
        [HttpGet("{userId}/binges")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Binge>))]
        [ProducesResponseType(400)]

        public IActionResult getUserBinges(int userId)
        {
            if (!_userRepository.DoesUserExist(userId))
                return NotFound();

            var shows = _mapper.Map<List<BingeDto>>(_userRepository.GetUserBinges(userId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(shows);
        }

        /// <summary>
        /// Retrieves all favorite shows of a user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user to retrieve favorite shows for.</param>
        /// <returns>Returns a list of all favorite shows of the user.</returns>
        [HttpGet("{userId}/favoriteShows")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Show>))]
        [ProducesResponseType(400)]

        public IActionResult getFavoriteShows(int userId)
        {
            if (!_userRepository.DoesUserExist(userId))
                return NotFound();

            var shows = _mapper.Map<List<ShowDto>>(_userRepository.GetFavoriteShows(userId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(shows);
        }

        //Post Requests
        /// <summary>
        /// Creates a new user with the provided login details.
        /// </summary>
        /// <param name="userInfo">The login details of the new user.</param>
        /// <returns>Returns Ok if the user is successfully created, otherwise returns an error.</returns>
        [HttpPut("newUser")]
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [AllowAnonymous]
        public IActionResult CreateUser([FromBody] LoginDto userInfo)
        {

            if (userInfo == null)
                return BadRequest(ModelState);
            var user = _userRepository.GetAllUsers()
                .Where(u => u.Name.Trim().ToUpper() == userInfo.Name.TrimEnd().ToUpper())
                .FirstOrDefault();

            if (userInfo.Name == null || userInfo.Password == null)
            {
                ModelState.AddModelError("", "Username or Password is null");
                return StatusCode(422, ModelState);
            }

            if (user != null)
            {
                ModelState.AddModelError("", "User already exists");
                return StatusCode(422, ModelState);
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userMap = _mapper.Map<User>(userInfo);
            if (!_userRepository.CreateUser(userMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            return Ok("User Successfully Created");
        }

        /// <summary>
        /// Logs a user into the system using their provided login credentials.
        /// </summary>
        /// <param name="loginInfo">The login credentials of the user.</param>
        /// <returns>Returns Ok with a success message or Unauthorized if the credentials are incorrect.</returns>
        [HttpPost("login")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [AllowAnonymous]
        public IActionResult LoginUser( [FromBody] LoginDto loginInfo)
        {
            if (_userRepository.GetUser(loginInfo.Name).Password.Trim().ToUpper() == loginInfo.Password.Trim().ToUpper())
            {
                var token = _tokenProvider.Create(loginInfo);
                return Ok(new { Token = token });
            }
            return Unauthorized(new { Message = "Invalid credentials!" });
        }

        /// <summary>
        /// Updates a user's information.
        /// </summary>
        /// <param name="userId">The unique identifier of the user to update.</param>
        /// <param name="updatedUser">The updated user details.</param>
        /// <returns>Returns Ok if the user is successfully updated, otherwise returns an error.</returns>
        [HttpPost("{userId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]

        public IActionResult UpdateUser(int userId, [FromBody] UserDto updatedUser)
        {
            if (updatedUser == null)
                return BadRequest(ModelState);
            if (userId != updatedUser.Id)
                return BadRequest(ModelState);
            if (_userRepository.DoesUserExist(userId) == false)
                return NotFound();
            if (!ModelState.IsValid)
                return BadRequest();

            var userMap = _mapper.Map<User>(updatedUser);
            if (!_userRepository.UpdateUser(userMap))
            {
                ModelState.AddModelError("", "Something went wrong updating user");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

        /// <summary>
        /// Adds a new binge to a user's list of binges.
        /// </summary>
        /// <param name="userId">The unique identifier of the user to add the binge to.</param>
        /// <param name="bingeInfo">The binge details to be added.</param>
        /// <returns>Returns Ok if the binge is successfully created and attributed to the user, otherwise returns an error.</returns>
        [HttpPost("{userId}/newBinge")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult AddBingetoUser(int userId, [FromBody] BingeDto bingeInfo)
        {

            if (bingeInfo == null)
                return BadRequest(ModelState);
            var user = _userRepository.GetAllUsers()
                .Where(u => u.Name.Trim().ToUpper() == bingeInfo.Name.TrimEnd().ToUpper())
                .FirstOrDefault();

            if (user != null)
            {
                ModelState.AddModelError("", "Binge already exists");
                return StatusCode(422, ModelState);
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var bingeMap = _mapper.Map<Binge>(bingeInfo);

            if (_userRepository.DoesUserExist(userId) == false)
            {
                ModelState.AddModelError("", "User doesnt exist, Cannot attribute Binge to nonexistant User");
                return StatusCode(422, ModelState);
            }
            bingeMap.UserId = userId;
            bingeMap.Author = _userRepository.GetUser(userId);

            if (!_userRepository.AddBingeToUser(bingeMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            return Ok("Binge successfully created and attributed to User");

        }

        /// <summary>
        /// Adds a show to the user's list of favorite shows.
        /// </summary>
        /// <param name="userId">The unique identifier of the user to add the show to.</param>
        /// <param name="showId">The unique identifier of the show to add as a favorite.</param>
        /// <returns>Returns Ok if the show is successfully added to the user's favorite shows, otherwise returns an error.</returns>
        [HttpPut("{userId}/newFavoriteShow")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]

        public IActionResult AddFavoriteShowToUser(int userId, [FromQuery] int showId)
        {
            if (_userRepository.DoesUserExist(userId) == false)
                return NotFound();
            if (_showRepository.DoesShowExist(showId) == false)
            {
                ModelState.AddModelError("", "Show doesnt exist");
                return StatusCode(422, ModelState);
            }

            if (_userRepository.IsShowAFavoriteShowOfUser(userId, showId) == true)
            {
                ModelState.AddModelError("", "Show is already a Favorite Show of User");
                return StatusCode(422, ModelState);
            }
            if (!ModelState.IsValid)
                return BadRequest();

            if (!_userRepository.AddToFavoriteShows(userId, showId))
            {
                ModelState.AddModelError("", "Something went wrong updating binge");
                return StatusCode(500, ModelState);
            }
            return Ok("Show was added to User's Favorite Shows");

        }

        //Delete Requests
        /// <summary>
        /// Deletes a user from the system by their unique identifier.
        /// </summary>
        /// <param name="userId">The unique identifier of the user to delete.</param>
        /// <returns>Returns Ok if the user is successfully deleted, otherwise returns an error.</returns>
        [HttpDelete("{userId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteUser(int userId)
        {
            if (!_userRepository.DoesUserExist(userId))
            {
                return NotFound();
            }
            var favoriteShowListToDelete = _userRepository.GetFavoriteShowsRelations(userId);
            var bingesToDelete = _userRepository.GetUserBinges(userId);
            var userToDelete = _userRepository.GetUser(userId);
            if (!ModelState.IsValid)
                return BadRequest();
            if (!bingesToDelete.IsNullOrEmpty())
            {
                foreach (var binge in bingesToDelete)
                {
                    if (_bingeRepository.GetShowsInBinge(binge.Id).IsNullOrEmpty()) continue;
                    if (!_bingeRepository.RemoveAllShowsFromBinge(binge.Id))
                    {
                        ModelState.AddModelError("", "Something went wrong when removing Shows from one of User's Binges");
                    }
                }
                if (!_bingeRepository.DeleteBinges(bingesToDelete.ToList()))
                {
                    ModelState.AddModelError("", "Something went wrong when deleting user's binges");
                }
            }
            if (!favoriteShowListToDelete.IsNullOrEmpty())
            {
                if (!_userRepository.DeleteFavoriteShowList(favoriteShowListToDelete.ToList()))
                {
                    ModelState.AddModelError("", "Something went wrong when deleting user's binges");
                }
            }
            if (!_userRepository.DeleteUser(userToDelete))
            {
                ModelState.AddModelError("", "Something went wrong when deleting User");
            }
            return Ok("User was successfully deleted");
        }

        /// <summary>
        /// Deletes a specific favorite show from the user's favorites list.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="showId">The unique identifier of the show to remove.</param>
        /// <returns>Returns Ok if the favorite show is successfully removed, otherwise an error.</returns>
        [HttpDelete("{userId}/removeFavoriteShow")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteFavoriteShowFromUser(int userId, [FromQuery] int showId)
        {
            if (!_userRepository.DoesUserExist(userId))
            {
                return NotFound();
            }
            if (_showRepository.DoesShowExist(showId) == false)
            {
                ModelState.AddModelError("", "Show doesnt exist");
                return StatusCode(422, ModelState);
            }
            if (_userRepository.IsShowAFavoriteShowOfUser(userId, showId) == false)
            {
                ModelState.AddModelError("", "Show is not a Favorite Show of User");
                return StatusCode(422, ModelState);
            }
            var favoriteShowToDelete = _userRepository.GetFavoriteShow(userId, showId);
            if (!ModelState.IsValid)
                return BadRequest();

            if (!_userRepository.RemoveFromFavoriteShows(favoriteShowToDelete))
            {
                ModelState.AddModelError("", "Something went wrong when removing show from User's Favorite Shows");
            }
            return Ok("Favorite Show was successfully removed");
        }

        /// <summary>
        /// Removes all favorite shows from a user's list of favorite shows.
        /// </summary>
        /// <param name="userId">The unique identifier of the user whose favorite shows will be removed.</param>
        /// <returns>Returns Ok if all favorite shows are successfully removed, otherwise returns an error.</returns>
        [HttpDelete("{userId}/removeAllFavoriteShow")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteFavoriteShowListFromUser(int userId)
        {
            if (!_userRepository.DoesUserExist(userId))
            {
                return NotFound();
            }
            var favoriteShowToDelete = _userRepository.GetFavoriteShowsRelations(userId);
            if (!ModelState.IsValid)
                return BadRequest();

            if (!_userRepository.DeleteFavoriteShowList(favoriteShowToDelete.ToList()))
            {
                ModelState.AddModelError("", "Something went wrong when removing all Favorite Shows");
            }
            return Ok("All Favorite Shows were successfully removed");
        }

        /// <summary>
        /// Deletes a binge from the user's list of binges.
        /// </summary>
        /// <param name="userId">The unique identifier of the user to delete the binge from.</param>
        /// <param name="bingeId">The unique identifier of the binge to delete.</param>
        /// <returns>Returns Ok if the binge is successfully deleted, otherwise returns an error.</returns>
        [HttpDelete("{userId}/deleteBinge")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteBingeFromUser(int userId, [FromQuery] int bingeId)
        {
            if (!_userRepository.DoesUserExist(userId))
            {
                return NotFound();
            }
            if (!_bingeRepository.DoesBingeExist(bingeId))
            {
                ModelState.AddModelError("", "Binge doesnt exist");
                return StatusCode(422, ModelState);
            }
            if (!_bingeRepository.DoesUserHaveBinge(userId, bingeId))
            {
                ModelState.AddModelError("", "Binge doesnt belong to User");
                return StatusCode(422, ModelState);
            }
            var bingeToDelete = _bingeRepository.GetBinge(bingeId);
            if (!ModelState.IsValid)
                return BadRequest();
            if (!_bingeRepository.DeleteBinge(bingeToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting Binge");
            }
            return Ok("Binge was successfully deleted");
        }

        /// <summary>
        /// Deletes all binges from the user's list of binges.
        /// </summary>
        /// <param name="userId">The unique identifier of the user whose binges will be deleted.</param>
        /// <returns>Returns Ok if all binges are successfully deleted, otherwise returns an error.</returns>
        [HttpDelete("{userId}/DeleteAllBinges")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteAllUserBinges(int userId)
        {
            if (!_userRepository.DoesUserExist(userId))
            {
                return NotFound();
            }
            var bingesToDelete = _userRepository.GetUserBinges(userId).ToList();
            if (!ModelState.IsValid)
                return BadRequest();
            foreach (var binge in bingesToDelete)
            {
                if (!_bingeRepository.RemoveAllShowsFromBinge(binge.Id))
                {
                    ModelState.AddModelError("", "Something went wrong when removing Shows from one of User's Binges");
                }
            }
            if (!_bingeRepository.DeleteBinges(bingesToDelete))
            {
                ModelState.AddModelError("", "Something went wrong when deleting user's binges");
            }
            return Ok("All of User's Binges were successfully deleted");
        }

    }
}
