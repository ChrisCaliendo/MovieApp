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
        private readonly IMapper _mapper;

        public BingeController(IBingeRepository bingeRepository, IUserRepository userRepository, IMapper mapper)
        {
            _bingeRepository = bingeRepository;
            _userRepository = userRepository;
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
            if (!_bingeRepository.doesBingeExist(bingeId))
                return NotFound();

            var shows = _mapper.Map<BingeDto>(_bingeRepository.GetBinge(bingeId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(shows);
        }

        [HttpGet("{bingeId}/shows")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Show>))]
        [ProducesResponseType(400)]

        public IActionResult GetShowTags(int bingeId)
        {
            if (!_bingeRepository.doesBingeExist(bingeId))
                return NotFound();

            var tags = _mapper.Map<List<ShowDto>>(_bingeRepository.GetShowsInBinge(bingeId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(tags);
        }

        //Post Requests

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateShow([FromQuery] int userId, [FromBody] BingeDto bingeInfo)
        {

            if (bingeInfo == null)
                return BadRequest(ModelState);
            var user = _bingeRepository.GetPublicBinges()
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

            if (!_bingeRepository.CreateBinge(bingeMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            return Ok("Binge Successfully Created");
        }
    }
}
