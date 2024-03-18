namespace MovieApp.Models
{
    public class User
    {
        public User(string name, string password)
        {
            this.Name = name;
            this.Password = password;
            this.EmailConfirmed = false;
            FavoriteShows = new List<Show>();
            FavoriteTags = new List<Tag>();
            Binges = new List<Binge>();
        }

        public int Id { get; set; }
        public string? Name { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Password { get; set; }
        private Boolean? EmailConfirmed { get; set; }
        public ICollection<Show> FavoriteShows { get; set; }
        public ICollection<Tag> FavoriteTags { get; set; }
        public ICollection<Binge> Binges { get; set; }
    }
}
