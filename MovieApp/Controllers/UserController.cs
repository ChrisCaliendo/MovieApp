﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MovieApp.Dto;
using MovieApp.Interfaces;
using MovieApp.Models;
using MovieApp.Repositories;

namespace MovieApp.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    [ApiController]

    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        //Get Requests

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<User>))]

        public IActionResult GetAllUsers()
        {
            var tags = _mapper.Map<List<UserDto>>(_userRepository.GetAllUsers());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(tags);
        }

        [HttpGet("/userById/{userId}")]
        [ProducesResponseType(200, Type = typeof(Show))]
        [ProducesResponseType(400)]

        public IActionResult getUser(int userId)
        {
            if (!_userRepository.DoesUserExist(userId))
                return NotFound();

            var shows = _mapper.Map<UserDto>(_userRepository.GetUser(userId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(shows);
        }

        [HttpGet("/userByName/{userId}")]
        [ProducesResponseType(200, Type = typeof(User))]
        [ProducesResponseType(400)]

        public IActionResult getUser(string userId)
        {
            if (!_userRepository.DoesUserExist(userId))
                return NotFound();

            var shows = _mapper.Map<UserDto>(_userRepository.GetUser(userId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(shows);
        }

        [HttpGet("/{userId}/binges")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Binge>))]
        [ProducesResponseType(400)]

        public IActionResult getUserBinges(int userId)
        {
            if (!_userRepository.DoesUserExist(userId))
                return NotFound();

            var shows = _mapper.Map<BingeDto>(_userRepository.GetUserBinges(userId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(shows);
        }

        [HttpGet("/{userId}/tags")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Tag>))]
        [ProducesResponseType(400)]

        public IActionResult getFavoriteTags(int userId)
        {
            if (!_userRepository.DoesUserExist(userId))
                return NotFound();

            var shows = _mapper.Map<TagDto>(_userRepository.GetFavoriteTags(userId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(shows);
        }

        [HttpGet("/{userId}/shows")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Show>))]
        [ProducesResponseType(400)]

        public IActionResult getFavoriteShows(int userId)
        {
            if (!_userRepository.DoesUserExist(userId))
                return NotFound();

            var shows = _mapper.Map<ShowDto>(_userRepository.GetFavoriteShows(userId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(shows);
        }

        //Post Requests

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateShow([FromBody] UserDto userInfo)
        {

            if (userInfo == null)
                return BadRequest(ModelState);
            var user = _userRepository.GetAllUsers()
                .Where(u => u.Name.Trim().ToUpper() == userInfo.Name.TrimEnd().ToUpper())
                .FirstOrDefault();

            if (user != null)
            {
                ModelState.AddModelError("", "User already exists");
                return StatusCode(422, ModelState);
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userMap = _mapper.Map<User>(userInfo);
            if (!_userRepository.CreateUser(userMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            return Ok("User Successfully Created");
        }


    }
}
