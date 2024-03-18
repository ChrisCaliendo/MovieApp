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

    public class BingeController
    {
        private readonly IBingeRepository _bingeRepository;
        private readonly IMapper _mapper;

        public BingeController(IBingeRepository bingeRepository, IMapper mapper)
        {
            _bingeRepository = bingeRepository;
            _mapper = mapper;
        }

        //[HttpGet]
        //[ProducesResponseType(200, Type = typeof(IEnumerable<Binge>))]

    }
}
