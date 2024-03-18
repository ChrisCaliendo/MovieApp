namespace MovieApp.Models
{
    public class ShowTag
    {
        public int? showId { get; set; }
        public int? tagId { get; set; }
        public Show? show { get; set; }
        public Tag? tag { get; set; }
    }
}
