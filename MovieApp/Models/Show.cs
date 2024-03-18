namespace MovieApp.Models
{
    public class Show
    {
        public int id { get; set; }
        public string? title { get; set; }
        public string? description { get; set; }
        public int? timespan { get; set; }
        public string? imageUrl { get; set; }
        public ICollection<ShowTag>? showTags { get; set; }
        public ICollection<ShowBinge>? showBinges { get; set;}
    }
}
