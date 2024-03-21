namespace MovieApp.Models
{
    public class Show
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int? Timespan { get; set; }
        public string? ImageUrl { get; set; }
        public ICollection<ShowTag> ShowTags { get; set; } = new List<ShowTag>();
        public ICollection<ShowBinge> ShowBinges { get; set; } = new List<ShowBinge>();
    }
}
