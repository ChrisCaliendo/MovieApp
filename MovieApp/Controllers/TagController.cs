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
    /// A controller for managing tags in the application, including creating, retrieving, updating, and deleting tags.
    /// </summary>
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class TagController : Controller
    {
        private readonly ITagRepository _tagRepository;
        private readonly IMapper _mapper;

        public TagController(ITagRepository tagRepository, IMapper mapper)
        {
            _tagRepository = tagRepository;
            _mapper = mapper;
        }

        //Get Requests
        /// <summary>
        /// Retrieves all tags from the repository.
        /// </summary>
        /// <returns>Returns a list of all tags.</returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Tag>))]


        public IActionResult GetTags()
        {
            var tags = _mapper.Map<List<TagDto>>(_tagRepository.GetTags());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(tags);
        }

        /// <summary>
        /// Retrieves a specific tag by its ID.
        /// </summary>
        /// <param name="tagId">The unique identifier of the tag.</param>
        /// <returns>Returns the tag data, or a NotFound response if the tag does not exist.</returns>
        [HttpGet("byId/{tagId}")]
        [ProducesResponseType(200, Type = typeof(Tag))]
        [ProducesResponseType(400)]

        public IActionResult GetTagById(int tagId)
        {
            if (!_tagRepository.DoesTagExist(tagId))
                return NotFound();

            var shows = _mapper.Map<TagDto>(_tagRepository.GetTag(tagId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(shows);
        }

        /// <summary>
        /// Retrieves a specific tag by its name.
        /// </summary>
        /// <param name="tagName">The name of the tag.</param>
        /// <returns>Returns the tag data, or a NotFound response if the tag does not exist.</returns>
        [HttpGet("byName/{tagName}")]
        [ProducesResponseType(200, Type = typeof(Tag))]
        [ProducesResponseType(400)]

        public IActionResult GetTagByName(string tagName)
        {
            if (!_tagRepository.DoesTagExist(tagName))
                return NotFound();

            var shows = _mapper.Map<TagDto>(_tagRepository.GetTag(tagName));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(shows);
        }

        /// <summary>
        /// Retrieves all shows that have a specific tag.
        /// </summary>
        /// <param name="tagId">The unique identifier of the tag.</param>
        /// <returns>Returns a list of shows with the given tag.</returns>
        [HttpGet("{tagId}/shows")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Show>))]
        [ProducesResponseType(400)]

        public IActionResult GetShowsWithTag(int tagId)
        {
            if (!_tagRepository.DoesTagExist(tagId))
                return NotFound();
            
            var tags = _mapper.Map<List<ShowDto>>(_tagRepository.GetShowsWithTag(tagId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(tags);
        }

        //Post Requests
        /// <summary>
        /// Creates a new tag.
        /// </summary>
        /// <param name="tagInfo">The information for the new tag.</param>
        /// <returns>Returns a success message, or a BadRequest response if the tag already exists or there was an error.</returns>
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateTag([FromBody] TagDto tagInfo)
        {

            if (tagInfo == null)
                return BadRequest(ModelState);
            var tag = _tagRepository.GetTags()
                .Where(t => t.Name.Trim().ToUpper() == tagInfo.Name.TrimEnd().ToUpper())
                .FirstOrDefault();

            if(tag != null)
            {
                ModelState.AddModelError("", "Tag already exists");
                return StatusCode(422, ModelState);
            }
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var tagMap = _mapper.Map<Tag>(tagInfo);
            if (!_tagRepository.CreateTag(tagMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            return Ok("Tag Successfully Created");
        }

        /// <summary>
        /// Updates an existing tag by its ID.
        /// </summary>
        /// <param name="tagId">The unique identifier of the tag.</param>
        /// <param name="updatedTag">The updated tag data.</param>
        /// <returns>Returns a success message, or an error message if there was an issue updating the tag.</returns>
        [HttpPut("{tagId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]

        public IActionResult UpdateTag(int tagId, [FromBody] TagDto updatedTag)
        {
            if (updatedTag == null)
                return BadRequest(ModelState);
            if (tagId != updatedTag.Id)
                return BadRequest(ModelState);
            if (_tagRepository.DoesTagExist(tagId) == false)
                return NotFound();
            if (!ModelState.IsValid)
                return BadRequest();

            var tagMap = _mapper.Map<Tag>(updatedTag);
            if (!_tagRepository.UpdateTag(tagMap))
            {
                ModelState.AddModelError("", "Something went wrong updating tag");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

        //Delete Requests
        /// <summary>
        /// Deletes a tag by its ID.
        /// </summary>
        /// <param name="tagId">The unique identifier of the tag.</param>
        /// <returns>Returns a success message, or an error message if there was an issue deleting the tag.</returns>
        [HttpDelete("{tagId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteTag(int tagId)
        {
            if (!_tagRepository.DoesTagExist(tagId))
            {
                return NotFound();
            }
            var tagToDelete = _tagRepository.GetTag(tagId);
            if (!ModelState.IsValid)
                return BadRequest();
            if (!_tagRepository.RemoveTagFromAllShows(tagId))
            {
                ModelState.AddModelError("", "Something went wrong when removing Tag from all Shows");
            }
            if (!_tagRepository.DeleteTag(tagToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting Tag");
            }
            return Ok("Tag was successfully deleted");
        }

        /// <summary>
        /// Removes a tag from all shows it is associated with.
        /// </summary>
        /// <param name="tagId">The unique identifier of the tag.</param>
        /// <returns>Returns a success message, or an error message if there was an issue removing the tag.</returns>
        [HttpDelete("{tagId}/removeFromAllShows")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult RemoveTagFromAllShows(int tagId)
        {
            if (!_tagRepository.DoesTagExist(tagId))
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
                return BadRequest();
            if (!_tagRepository.RemoveTagFromAllShows(tagId))
            {
                ModelState.AddModelError("", "Something went wrong when removing Tag from all Shows");
            }
            return Ok("All Tags was successfully removed from Show");
        }
    }
}
