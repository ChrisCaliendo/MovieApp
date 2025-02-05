using Microsoft.AspNetCore.Components;
using MovieApp.Interfaces;
using MovieApp.Models;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using MovieApp.Dto;
using MovieApp.Repositories;
using TMDbLib.Client;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;

namespace MovieApp.Controllers
{
    /// <summary>
    /// A controller for interacting with The Movie Database (TMDB) API to retrieve and process movie data.
    /// </summary>
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    [ApiController]

    public class TMDBController : Controller
    {

        public TMDbClient client;

        public TMDBController()
        {
            client = new TMDbClient("a7d81a1952228d5363bd1016273317c8");
        }

        /// <summary>
        /// Gets the raw data for a movie by its TMDB ID.
        /// </summary>
        /// <param name="tmdbID">The unique identifier for the movie from TMDB.</param>
        /// <returns>Returns raw movie data if found, or a NotFound response if the movie is not found.</returns>
        [HttpGet("getById/{tmdbID}/rawData")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]

        public IActionResult GetRawShowData(int tmdbID)
        {
            Movie movie = client.GetMovieAsync(tmdbID).Result;
            if(movie == null) return NotFound();
            return Ok(movie);
        }

        /// <summary>
        /// Gets filtered movie data by TMDB ID.
        /// </summary>
        /// <param name="tmdbID">The unique identifier for the movie from TMDB.</param>
        /// <returns>Returns filtered movie data if found, or a NotFound response if the movie is not found.</returns>
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

        /// <summary>
        /// Searches for a movie by name and returns filtered data for the first result.
        /// </summary>
        /// <param name="tmdbName">The name of the movie to search for.</param>
        /// <returns>Returns filtered movie data for the first result, or a NotFound response if no movie is found.</returns>
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

        /// <summary>
        /// Gets a list of popular movies with filtered data.
        /// </summary>
        /// <returns>Returns a list of filtered popular movie data.</returns>
        [HttpGet("getPopularMovies/filteredData")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]

        public IActionResult GetPopularShowData()
        {
            SearchContainer<SearchMovie> results = client.GetMoviePopularListAsync().Result;
            if (results == null || results.Results.Count < 1) return NotFound();

            List<TMDBShowDto> tMDBShows = new List<TMDBShowDto>();
            foreach (var movie in results.Results)
            {
                TMDBShowDto show = new TMDBShowDto();

                show.Id = movie.Id;
                show.Title = movie.Title;
                show.Description = movie.Overview;
                show.Timespan = null;
                show.ImageUrl = movie.PosterPath;
                tMDBShows.Add(show);
            }
           
            //use before extension: https://image.tmdb.org/t/p/w220_and_h330_face

            return Ok(tMDBShows);
        }
    }
}
