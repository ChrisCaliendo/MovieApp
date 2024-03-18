using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MovieApp.Interfaces;

namespace MovieApp.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    [ApiController]

    public class UserController
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        //[HttpGet]
        //[ProducesResponseType(200, Type = typeof(IEnumerable<Binge>))]
    }
}
