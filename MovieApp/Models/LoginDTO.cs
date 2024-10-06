using Microsoft.AspNetCore.Identity;

namespace MovieApp.Models
{
    public class LoginDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        private Boolean? EmailConfirmed { get; set; }
        public ICollection<FavoriteShow> FavoriteShows { get; set; } = new List<FavoriteShow>();
        public ICollection<Binge> Binges { get; set; } = new List<Binge>();
    }
}
