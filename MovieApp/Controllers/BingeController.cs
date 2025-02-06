using Microsoft.AspNetCore.Components;
using MovieApp.Interfaces;
using MovieApp.Models;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using MovieApp.Dto;
using MovieApp.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace MovieApp.Controllers
{
    /// <summary>
    /// Controller for managing binge-related operations. Handles HTTP requests for creating, updating, retrieving, and deleting binges.
    /// </summary>
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class BingeController : Controller
    {
        private readonly IBingeRepository _bingeRepository;
        private readonly IUserRepository _userRepository;
        private readonly IShowRepository _showRepository;
        private readonly IMapper _mapper;

        public BingeController(IBingeRepository bingeRepository, IUserRepository userRepository, IShowRepository showRepository, IMapper mapper)
        {
            _bingeRepository = bingeRepository;
            _userRepository = userRepository;
            _showRepository = showRepository;
            _mapper = mapper;
        }

        //Get Requests
        /// <summary>
        /// Retrieves a list of public binges.
        /// </summary>
        /// <returns>Returns a list of public binges.</returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Binge>))]

        public IActionResult GetPublicBinges()
        {
            var tags = _mapper.Map<List<BingeDto>>(_bingeRepository.GetPublicBinges());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(tags);
        }

        /// <summary>
        /// Retrieves a specific binge by its unique identifier.
        /// </summary>
        /// <param name="bingeId">The unique identifier of the binge.</param>
        /// <returns>Returns the binge if found, otherwise returns NotFound.</returns>
        [HttpGet("{bingeId}")]
        [ProducesResponseType(200, Type = typeof(Show))]
        [ProducesResponseType(400)]

        public IActionResult GetBinge(int bingeId)
        {
            if (!_bingeRepository.DoesBingeExist(bingeId))
                return NotFound();

            var binge = _mapper.Map<BingeDto>(_bingeRepository.GetBinge(bingeId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(binge);
        }

        /// <summary>
        /// Retrieves detailed information about a specific binge, including timespan and associated shows.
        /// </summary>
        /// <param name="bingeId">The unique identifier of the binge.</param>
        /// <returns>Returns detailed information about the binge.</returns>
        [HttpGet("{bingeId}/fullInfo")]
        [ProducesResponseType(200, Type = typeof(Show))]
        [ProducesResponseType(400)]

        public IActionResult GetBingeFull(int bingeId)
        {
            if (!_bingeRepository.DoesBingeExist(bingeId))
                return NotFound();

            var binge = _mapper.Map<BingeExtDto>(_bingeRepository.GetBinge(bingeId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            binge.Timespan = _bingeRepository.GetBingeTimespan(bingeId);
            binge.IsTimespanAccurate = (_bingeRepository.GetUnknownTimespans(bingeId) > 0) ? false : true;
            binge.ShowCount = _bingeRepository.GetShowCountInBinge(bingeId);

            return Ok(binge);
        }

        /// <summary>
        /// Retrieves the shows associated with a specific binge.
        /// </summary>
        /// <param name="bingeId">The unique identifier of the binge.</param>
        /// <returns>Returns a list of shows associated with the binge.</returns>
        [HttpGet("{bingeId}/shows")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Show>))]
        [ProducesResponseType(400)]

        public IActionResult GetBingeShows(int bingeId)
        {
            if (!_bingeRepository.DoesBingeExist(bingeId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var shows = _mapper.Map<List<ShowDto>>(_bingeRepository.GetShowsInBinge(bingeId));
            
            return Ok(shows);
        }

        /// <summary>
        /// Retrieves the tags associated with a specific binge.
        /// </summary>
        /// <param name="bingeId">The unique identifier of the binge.</param>
        /// <returns>Returns a list of tags associated with the binge.</returns>
        [HttpGet("{bingeId}/tags")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Show>))]
        [ProducesResponseType(400)]

        public IActionResult GetBingeTags(int bingeId)
        {
            if (!_bingeRepository.DoesBingeExist(bingeId))
                return NotFound();

            var tags = _mapper.Map<List<TagDto>>(_bingeRepository.GetTagsInBinge(bingeId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(tags);
        }

        //Post Requests
        /// <summary>
        /// Creates a new binge for a user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user creating the binge.</param>
        /// <param name="bingeInfo">The binge details to be created.</param>
        /// <returns>Returns a success message if the binge was created successfully, otherwise returns an error message.</returns>
        [HttpPost("{userId}/newBinge")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateBinge(int userId, [FromBody] BingeDto bingeInfo)
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

            if (!_bingeRepository.CreateBinge(bingeMap, userId))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            return Ok("Binge Successfully Created");

        }

        /// <summary>
        /// Updates an existing binge by its unique identifier.
        /// </summary>
        /// <param name="bingeId">The unique identifier of the binge to be updated.</param>
        /// <param name="updatedBinge">The updated binge information.</param>
        /// <returns>Returns a success message if the binge was updated successfully, otherwise returns an error message.</returns>
        [HttpPut("{bingeId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]

        public IActionResult UpdateBinge( int bingeId, [FromBody] BingeDto updatedBinge)
        {
            if (updatedBinge == null)
                return BadRequest(ModelState);
            if (bingeId != updatedBinge.Id)
                return BadRequest(ModelState);
            if (_bingeRepository.DoesBingeExist(bingeId) == false)
                return NotFound();
            if (!ModelState.IsValid)
                return BadRequest();

            var bingeMap = _mapper.Map<Binge>(updatedBinge);
            if (!_bingeRepository.UpdateBinge(bingeMap))
            {
                ModelState.AddModelError("", "Something went wrong updating binge");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

        /// <summary>
        /// Adds a show to an existing binge by the specified bingeId and showId.
        /// </summary>
        /// <param name="bingeId">The unique identifier of the binge to which the show will be added.</param>
        /// <param name="showId">The unique identifier of the show to be added to the binge.</param>
        /// <returns>Returns a success message if the show was added to the binge successfully, otherwise returns an error message.</returns>
        [HttpPut("{bingeId}/addShow")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]

        public IActionResult AddShowToBinge(int bingeId, [FromQuery] int showId)
        {
            if (_bingeRepository.DoesBingeExist(bingeId) == false)
                return NotFound();
            if (_showRepository.DoesShowExist(showId) == false)
            {
                ModelState.AddModelError("", "Show doesnt exist");
                return StatusCode(422, ModelState);
            }
            
            if (_bingeRepository.DoesBingeHaveShow(bingeId, showId) == true)
            {
                ModelState.AddModelError("", "Show is already in Binge");
                return StatusCode(422, ModelState);
            }
            if (!ModelState.IsValid)
                return BadRequest();

            if (!_bingeRepository.AddShowToBinge(bingeId, showId))
            {
                ModelState.AddModelError("", "Something went wrong updating binge");
                return StatusCode(500, ModelState);
            }
            return Ok("Show was successfully added to Binge");
        }

        //Delete Requests
        /// <summary>
        /// Deletes a binge and removes all associated shows.
        /// </summary>
        /// <param name="bingeId">The unique identifier of the binge to be deleted.</param>
        /// <returns>Returns a success message if the binge and its associated shows were deleted successfully, otherwise returns an error message.</returns>
        [HttpDelete("{bingeId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteBinge(int bingeId)
        {
            if (!_bingeRepository.DoesBingeExist(bingeId))
            {
                return NotFound();
            }

            var bingeToDelete = _bingeRepository.GetBinge(bingeId);
            if (!ModelState.IsValid)
                return BadRequest();
            if (!_bingeRepository.RemoveAllShowsFromBinge(bingeId))
            {
                ModelState.AddModelError("", "Something went wrong when removing Shows from Binge");
            }
            if (!_bingeRepository.DeleteBinge(bingeToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting Binge");
            }
            return Ok("Binge was successfully deleted");
        }

        /// <summary>
        /// Removes a specific show from a binge.
        /// </summary>
        /// <param name="bingeId">The unique identifier of the binge.</param>
        /// <param name="showId">The unique identifier of the show to be removed.</param>
        /// <returns>Returns a success message if the show was successfully removed, otherwise returns an error message.</returns>
        [HttpDelete("{bingeId}/removeShow")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult RemoveShowFromBinge(int bingeId, [FromQuery] int showId)
        {
            if (!_bingeRepository.DoesBingeExist(bingeId))
            {
                return NotFound();
            }
            if (_bingeRepository.DoesBingeHaveShow(bingeId, showId) == false)
            {
                ModelState.AddModelError("", "Show is not in Binge");
                return StatusCode(422, ModelState);
            }
            if (!ModelState.IsValid)
                return BadRequest();
            if (!_bingeRepository.RemoveShowFromBinge(bingeId, showId))
            {
                ModelState.AddModelError("", "Something went wrong when removing Shows from Binge");
            }
            return Ok("Show was successfully removed from Binge");
        }

        /// <summary>
        /// Removes all shows from a binge while keeping the binge itself intact.
        /// </summary>
        /// <param name="bingeId">The unique identifier of the binge from which all shows will be removed.</param>
        /// <returns>Returns a success message if all shows were removed from the binge successfully, otherwise returns an error message.</returns>
        [HttpDelete("{bingeId}/removeAllShow")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult RemoveAllShowsFromBinge(int bingeId)
        {
            if (!_bingeRepository.DoesBingeExist(bingeId))
            {
                return NotFound();
            }

            var bingeToDelete = _bingeRepository.GetBinge(bingeId);
            if (!ModelState.IsValid)
                return BadRequest();
            if (!_bingeRepository.RemoveAllShowsFromBinge(bingeId))
            {
                ModelState.AddModelError("", "Something went wrong when removing Shows from Binge");
            }

            return Ok("All Shows were successfully removed from Binge");
        }



    }
}
