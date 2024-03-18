namespace MovieApp.Models
{
    public class User
    {
        public User(string name, string password)
        {
            this.name = name;
            this.password = password;
            this.emailConfirmed = false;
            favoriteShows = new List<Show>();
            favoriteTags = new List<Tag>();
            binges = new List<Binge>();
        }

        public int id { get; set; }
        public string? name { get; set; } = string.Empty;
        public string? email { get; set; }
        public string? password { get; set; }
        private Boolean? emailConfirmed { get; set; }
        public ICollection<Show> favoriteShows { get; set; }
        public ICollection<Tag> favoriteTags { get; set; }
        public ICollection<Binge> binges { get; set; }
    }
}
