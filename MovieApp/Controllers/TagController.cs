using Microsoft.AspNetCore.Components;
using MovieApp.Interfaces;
using MovieApp.Models;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using MovieApp.Dto;

namespace MovieApp.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    [ApiController]

    public class TagController : Controller
    {
        private readonly ITagRepository _tagRepository;
        private readonly IMapper _mapper;

        public TagController(ITagRepository tagRepository, IMapper mapper)
        {
            _tagRepository = tagRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Tag>))]


        public IActionResult GetTags()
        {
            var tags = _mapper.Map<List<TagDto>>(_tagRepository.GetTags());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(tags);
        }

        [HttpGet("tagById/{tagId}")]
        [ProducesResponseType(200, Type = typeof(Tag))]
        [ProducesResponseType(400)]

        public IActionResult GetTags(int tagId)
        {
            if (!_tagRepository.doesTagExist(tagId))
                return NotFound();

            var shows = _mapper.Map<TagDto>(_tagRepository.GetTag(tagId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(shows);
        }

        [HttpGet("tagByName/{tagName}")]
        [ProducesResponseType(200, Type = typeof(Tag))]
        [ProducesResponseType(400)]

        public IActionResult GetTags(string tagName)
        {
            if (!_tagRepository.doesTagExist(tagName))
                return NotFound();

            var shows = _mapper.Map<TagDto>(_tagRepository.GetTag(tagName));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(shows);
        }

        [HttpGet("/{tagId}/shows")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Show>))]
        [ProducesResponseType(400)]

        public IActionResult GetShowsWithTag(int tagId)
        {

            if (!_tagRepository.doesTagExist(tagId))
                return NotFound();

            var tags = _mapper.Map<List<TagDto>>(_tagRepository.GetTags());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(tags);
        }
    }
}
