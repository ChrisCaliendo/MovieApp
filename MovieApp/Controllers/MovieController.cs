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

    public class MovieController : Controller
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IMapper _mapper;

        public MovieController(IMovieRepository movieRepository, IMapper mapper)
        {
            _movieRepository = movieRepository;
            _mapper = mapper;
        }

        //Get Requests

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Show>))]

        public IActionResult GetShows()
        {
            var shows = _mapper.Map<List<ShowDto>>(_movieRepository.GetShows());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(shows);
        }

        [HttpGet("{showId}")]
        [ProducesResponseType(200, Type = typeof(Show))]
        [ProducesResponseType(400)]

        public IActionResult GetShow(int showId)
        {
            if (!_movieRepository.DoesShowExist(showId))
                return NotFound();

            var shows = _mapper.Map<ShowDto>(_movieRepository.GetShow(showId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(shows);
        }

        [HttpGet("{showId}/tags")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Tag>))]
        [ProducesResponseType(400)]

        public IActionResult GetShowTags(int showId)
        {
            if (!_movieRepository.DoesShowExist(showId))
                return NotFound();

            var tags = _mapper.Map<List<TagDto>>(_movieRepository.GetTagsOfShow(showId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(tags);
        }

        //Post Requests

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateShow([FromBody] ShowDto showInfo)
        {

            if (showInfo == null)
                return BadRequest(ModelState);
            var show = _movieRepository.GetShows()
                .Where(s => s.Title.Trim().ToUpper() == showInfo.Title.TrimEnd().ToUpper())
                .FirstOrDefault();

            if (show != null)
            {
                ModelState.AddModelError("", "Show already exists");
                return StatusCode(422, ModelState);
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var showMap = _mapper.Map<Show>(showInfo);
            if (!_movieRepository.CreateShow(showMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            return Ok("Show Successfully Created");
        }


    }

    
}
