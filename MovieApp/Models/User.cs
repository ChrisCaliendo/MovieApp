namespace MovieApp.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        private Boolean? EmailConfirmed { get; set; }
        public ICollection<FavoriteShow> FavoriteShows { get; set; }
        public ICollection<FavoriteTag> FavoriteTags { get; set; }
        public ICollection<Binge> Binges { get; set; }
    }
}
