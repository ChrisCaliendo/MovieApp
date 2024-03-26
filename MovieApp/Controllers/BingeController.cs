using Microsoft.AspNetCore.Components;
using MovieApp.Interfaces;
using MovieApp.Models;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using MovieApp.Dto;
using MovieApp.Repositories;

namespace MovieApp.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    [ApiController]

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

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Binge>))]

        public IActionResult GetPublicBinges()
        {
            var tags = _mapper.Map<List<BingeDto>>(_bingeRepository.GetPublicBinges());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(tags);
        }

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

        [HttpPut("{bingeId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]

        public IActionResult UpdateBinge( int bingeId, [FromBody] BingeDto updatedBinge)
        {
            if (updatedBinge == null)
                return BadRequest(ModelState);
            if (bingeId == updatedBinge.Id)
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
