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
        private readonly IMapper _mapper;

        public BingeController(IBingeRepository bingeRepository, IMapper mapper)
        {
            _bingeRepository = bingeRepository;
            _mapper = mapper;
        }

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
    }
}
