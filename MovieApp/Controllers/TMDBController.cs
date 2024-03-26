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
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;

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

        [HttpGet("getById/{tmdbID}/rawData")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]

        public IActionResult GetRawShowData(int tmdbID)
        {
            Movie movie = client.GetMovieAsync(tmdbID).Result;
            if(movie == null) return NotFound();
            return Ok(movie);
        }

        [HttpGet("getById/{tmdbID}/filteredData")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]

        public IActionResult GetFilteredShowData(int tmdbID)
        {
            Movie movie = client.GetMovieAsync(tmdbID).Result;
            if (movie == null) return NotFound();
            TMDBShowDto show = new TMDBShowDto();

            show.Id = movie.Id;
            show.Title = movie.Title;
            show.Description = movie.Overview;
            show.Timespan = movie.Runtime;
            show.ImageUrl = movie.PosterPath;

            return Ok(show);
        }

        [HttpGet("SearchbyName/{tmdbName}/firstOrDefault/filteredData")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]

        public IActionResult GetFilteredShowData(string tmdbName)
        {
            SearchContainer<SearchMovie> results = client.SearchMovieAsync(tmdbName).Result;
            if (results == null || results.Results.Count < 1) return NotFound();

            TMDBShowDto show = new TMDBShowDto();

            show.Id = results.Results[0].Id;
            show.Title = results.Results[0].Title;
            show.Description = results.Results[0].Overview;
            show.Timespan = null;
            show.ImageUrl = results.Results[0].PosterPath;
            //use before extension: https://image.tmdb.org/t/p/w220_and_h330_face

            return Ok(show);
        }
    }
}
