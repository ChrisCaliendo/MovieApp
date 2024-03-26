using Microsoft.AspNetCore.Components;
using MovieApp.Interfaces;
using MovieApp.Models;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using MovieApp.Dto;
using MovieApp.Repositories;
using MovieApp.Wrapper;
using TMDbLib.Client;
using TMDbLib.Objects.Movies;

namespace MovieApp.Controllers
{

    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    [ApiController]

    public class TMDBController : Controller
    {

        public TMDbClient client;

        public TMDBController()
        {
            client = new TMDbClient("a7d81a1952228d5363bd1016273317c8");
        }

        [HttpGet("byId")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]

        public IActionResult GetShowsWithTag()
        {
            Movie movie = client.GetMovieAsync(1570).Result;
            if(movie == null) return NotFound();
            Console.WriteLine($"Movie name: {movie.Title}");
            return Ok(movie);
        }
    }
}
