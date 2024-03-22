using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MovieApp.Dto;
using MovieApp.Interfaces;
using MovieApp.Models;
using MovieApp.Repositories;

namespace MovieApp.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    [ApiController]

    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IShowRepository _showRepository;
        private readonly IBingeRepository _bingeRepository;
        private readonly IMapper _mapper;

        public UserController(IUserRepository userRepository, IShowRepository showRepository, IBingeRepository bingeRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        //Get Requests

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<User>))]

        public IActionResult GetAllUsers()
        {
            var tags = _mapper.Map<List<UserDto>>(_userRepository.GetAllUsers());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(tags);
        }

        [HttpGet("/userById/{userId}")]
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

        [HttpGet("/userByName/{userId}")]
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

        [HttpGet("/{userId}/binges")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Binge>))]
        [ProducesResponseType(400)]

        public IActionResult getUserBinges(int userId)
        {
            if (!_userRepository.DoesUserExist(userId))
                return NotFound();

            var shows = _mapper.Map<BingeDto>(_userRepository.GetUserBinges(userId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(shows);
        }

        [HttpGet("/{userId}/shows")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Show>))]
        [ProducesResponseType(400)]

        public IActionResult getFavoriteShows(int userId)
        {
            if (!_userRepository.DoesUserExist(userId))
                return NotFound();

            var shows = _mapper.Map<ShowDto>(_userRepository.GetFavoriteShows(userId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(shows);
        }

        //Post Requests

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateUser([FromBody] UserDto userInfo)
        {

            if (userInfo == null)
                return BadRequest(ModelState);
            var user = _userRepository.GetAllUsers()
                .Where(u => u.Name.Trim().ToUpper() == userInfo.Name.TrimEnd().ToUpper())
                .FirstOrDefault();

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

        [HttpPut("{userId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]

        public IActionResult UpdateUser(int userId, [FromBody] UserDto updatedUser)
        {
            if (updatedUser == null)
                return BadRequest(ModelState);
            if (userId == updatedUser.Id)
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

            if (!_userRepository.AddBingeToUser(userId, bingeMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            return Ok("Binge successfully created and attributed to User");

        }

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
            var bingesToDelete = _userRepository.GetUserBinges(userId).ToList();
            var userToDelete = _userRepository.GetUser(userId);
            if (!ModelState.IsValid)
                return BadRequest();
            foreach(var binge in bingesToDelete)
            {
                if(!_bingeRepository.)
                {
                    ModelState.AddModelError("", "Something went wrong when deleting binge relations");
                }
            }
            if (!_bingeRepository.DeleteBinges(bingesToDelete))
            {
                ModelState.AddModelError("", "Something went wrong when deleting user's binges");
            }
            if (!_userRepository.DeleteFavoriteShowList(favoriteShowListToDelete.ToList()))
            {
                ModelState.AddModelError("", "Something went wrong when deleting user's binges");
            }
            if (!_userRepository.DeleteUser(userToDelete))
            {
                ModelState.AddModelError("", "Something went wrong when deleting User");
            }
            return Ok("User was successfully deleted");
        }

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
            if (_userRepository.IsShowAFavoriteShowOfUser(userId, showId) == true)
            {
                ModelState.AddModelError("", "Show is already a Favorite Show of User");
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


    }
}
