namespace MovieApp.Models
{
    public class Tag
    {

        public int id { get; set; }
        public string? name { get; set; }
        public string? description { get; set; }
        public ICollection<ShowTag>? showTags { get; set; }

    }
}
