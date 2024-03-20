using Microsoft.AspNetCore.Mvc;
using TMDbLib.Client;
using TMDbLib.Objects.Movies;

namespace MovieApp.Wrapper
{

    public class TMDBWrapper
    {
        public TMDBWrapper() {
            TMDbClient client = new TMDbClient("APIKey");
            Movie movie = client.GetMovieAsync(47964).Result;

            Console.WriteLine($"Movie name: {movie.Title}");
        }

        

        
    }
}
