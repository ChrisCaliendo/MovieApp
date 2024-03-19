namespace MovieApp.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        private Boolean? EmailConfirmed { get; set; }
        public ICollection<Show> FavoriteShows { get; set; }
        public ICollection<Tag> FavoriteTags { get; set; }
        public ICollection<Binge> Binges { get; set; }
    }
}
