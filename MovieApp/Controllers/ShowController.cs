using Microsoft.AspNetCore.Components;
using MovieApp.Interfaces;
using MovieApp.Models;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using MovieApp.Dto;
using MovieApp.Repositories;

namespace MovieApp.Controllers
{
    /// <summary>
    /// A controller for managing shows in the application, including creating, retrieving, updating, and deleting shows.
    /// </summary>
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
        /// <summary>
        /// Retrieves all shows from the repository.
        /// </summary>
        /// <returns>Returns a list of all shows.</returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Show>))]

        public IActionResult GetShows()
        {
            var shows = _mapper.Map<List<ShowDto>>(_showRepository.GetShows());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(shows);
        }

        /// <summary>
        /// Retrieves a specific show by its ID.
        /// </summary>
        /// <param name="showId">The unique identifier of the show.</param>
        /// <returns>Returns the show data, or a NotFound response if the show does not exist.</returns>
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

        /// <summary>
        /// Retrieves tags associated with a specific show.
        /// </summary>
        /// <param name="showId">The unique identifier of the show.</param>
        /// <returns>Returns a list of tags associated with the show.</returns>
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
        /// <summary>
        /// Creates a new show.
        /// </summary>
        /// <param name="showInfo">The information for the new show.</param>
        /// <returns>Returns a success message or an error if the show already exists or there is an issue creating the show.</returns>
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateShow([FromBody] ShowDto showInfo)
        {

            if (showInfo == null)
                return BadRequest(ModelState);

            if(showInfo.Id == null)
            {
                ModelState.AddModelError("", "Addition of new Show requires a unique Id");
                return StatusCode(422, ModelState);
            }

            if (_showRepository.DoesShowExist(showInfo.Id))
            {
                ModelState.AddModelError("", "A Show with this Id already exists");
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

        /// <summary>
        /// Updates an existing show by its ID.
        /// </summary>
        /// <param name="showId">The unique identifier of the show.</param>
        /// <param name="updatedShow">The updated show data.</param>
        /// <returns>Returns a success message, or an error message if there was an issue updating the show.</returns>
        [HttpPut("{showId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]

        public IActionResult UpdateShow( int showId, [FromBody] ShowDto updatedShow)
        {
            if (updatedShow == null)
                return BadRequest(ModelState);
            if (showId != updatedShow.Id)
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

        /// <summary>
        /// Adds a new tag to a show.
        /// </summary>
        /// <param name="showId">The unique identifier of the show.</param>
        /// <param name="tagId">The unique identifier of the tag.</param>
        /// <returns>Returns a success message or an error message if the tag already exists for the show.</returns>
        [HttpPut("{showId}/newTag")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]

        public IActionResult AddTagToShow( int showId, [FromQuery] int tagId)
        {
            if (_showRepository.DoesShowExist(showId) == false)
                return NotFound();
            if (_tagRepository.DoesTagExist(tagId) == false)
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
        /// <summary>
        /// Deletes a show by its ID.
        /// </summary>
        /// <param name="showId">The unique identifier of the show.</param>
        /// <returns>Returns a success message, or an error message if there was an issue deleting the show.</returns>
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

        /// <summary>
        /// Removes a specific tag from a show.
        /// </summary>
        /// <param name="showId">The unique identifier of the show.</param>
        /// <param name="tagId">The unique identifier of the tag.</param>
        /// <returns>Returns a success message, or an error message if the tag is not associated with the show.</returns>
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

        /// <summary>
        /// Removes all tags from a specific show.
        /// </summary>
        /// <param name="showId">The unique identifier of the show.</param>
        /// <returns>Returns a success message or an error if something goes wrong.</returns>
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
