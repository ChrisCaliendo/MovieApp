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

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Tag>))]


        public IActionResult GetTags()
        {
            var tags = _mapper.Map<List<TagDto>>(_tagRepository.GetTags());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(tags);
        }

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
