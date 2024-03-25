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

    public class ShowController : Controller
    {
        private readonly IShowRepository _showRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IMapper _mapper;

        public ShowController(IShowRepository showRepository, ITagRepository tagRepository, IMapper mapper)
        {
            _showRepository = showRepository;
            _tagRepository = tagRepository;
            _mapper = mapper;
        }

        //Get Requests

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Show>))]

        public IActionResult GetShows()
        {
            var shows = _mapper.Map<List<ShowDto>>(_showRepository.GetShows());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(shows);
        }

        [HttpGet("{showId}")]
        [ProducesResponseType(200, Type = typeof(Show))]
        [ProducesResponseType(400)]

        public IActionResult GetShow(int showId)
        {
            if (!_showRepository.DoesShowExist(showId))
                return NotFound();

            var shows = _mapper.Map<ShowDto>(_showRepository.GetShow(showId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(shows);
        }

        [HttpGet("{showId}/tags")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Tag>))]
        [ProducesResponseType(400)]

        public IActionResult GetShowTags(int showId)
        {
            if (!_showRepository.DoesShowExist(showId))
                return NotFound();

            var tags = _mapper.Map<List<TagDto>>(_showRepository.GetTagsOfShow(showId));

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
            var show = _showRepository.GetShows()
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
            if (!_showRepository.CreateShow(showMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            return Ok("Show Successfully Created");
        }

        [HttpPut("{showId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]

        public IActionResult UpdateShow( int showId, [FromBody] ShowDto updatedShow)
        {
            if (updatedShow == null)
                return BadRequest(ModelState);
            if (showId == updatedShow.Id)
                return BadRequest(ModelState);
            if (_showRepository.DoesShowExist(showId) == false)
                return NotFound();
            if (!ModelState.IsValid)
                return BadRequest();

            var showMap = _mapper.Map<Show>(updatedShow);
            if (!_showRepository.UpdateShow(showMap))
            {
                ModelState.AddModelError("", "Something went wrong updating show");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

        [HttpPut("{showId}/newTag")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]

        public IActionResult AddTagToShow( int showId, [FromQuery] int tagId)
        {
            if (_tagRepository.DoesTagExist(showId) == false)
                return NotFound();
            if (_showRepository.DoesShowExist(tagId) == false)
            {
                ModelState.AddModelError("", "Tag doesnt exist");
                return StatusCode(422, ModelState);
            }

            if (_showRepository.DoesShowHaveTag(showId, tagId) == true)
            {
                ModelState.AddModelError("", "Show already has this Tag");
                return StatusCode(422, ModelState);
            }
            if (!ModelState.IsValid)
                return BadRequest();


            if (!_showRepository.AddTagToShow(showId, tagId))
            {
                ModelState.AddModelError("", "Something went wrong when adding Tag to Show");
                return StatusCode(500, ModelState);
            }
            return Ok("Tag Successfully Added to Show");
        }

        //Delete Requests

        [HttpDelete("{showId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteShow(int showId)
        {
            if (!_showRepository.DoesShowExist(showId))
            {
                return NotFound();
            }
            var showToDelete = _showRepository.GetShow(showId);
            if (!ModelState.IsValid)
                return BadRequest();
            if (!_showRepository.RemoveAllTagsFromShow(showId))
            {
                ModelState.AddModelError("", "Something went wrong removing all Tags from Show");
            }
            if (!_showRepository.RemoveShowFromAllBinges(showId))
            {
                ModelState.AddModelError("", "Something went wrong removing Show from all Binges");
            }
            if (!_showRepository.DeleteShow(showToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting Show");
            }
            return Ok("Show was successfully deleted");
        }

        [HttpDelete("{showId}/removeTag")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult RemoveTagFromShow(int showId, [FromQuery]int tagId)
        {
            if (!_showRepository.DoesShowExist(showId))
            {
                return NotFound();
            }
            if (!_tagRepository.DoesTagExist(tagId))
            {
                ModelState.AddModelError("", "Tag doesnt exist");
                return StatusCode(422, ModelState);
            }
            if (!_showRepository.DoesShowHaveTag(showId, tagId))
            {
                ModelState.AddModelError("", "Show isnt associated with Tag");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest();
            if (!_showRepository.RemoveTagFromShow(showId, tagId))
            {
                ModelState.AddModelError("", "Something went wrong when removing Tag from Show");
            }
            return Ok("Tag was successfully removed from Show");
        }

        [HttpDelete("{showId}/removeAllTags")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult RemoveAllTagFromShow(int showId)
        {
            if (!_showRepository.DoesShowExist(showId))
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
                return BadRequest();
            if (!_showRepository.RemoveAllTagsFromShow(showId))
            {
                ModelState.AddModelError("", "Something went wrong when removing Tag from Show");
            }
            return Ok("All Tags was successfully removed from Show");
        }
    }

    
}
