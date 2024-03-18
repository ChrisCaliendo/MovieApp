namespace MovieApp.Models
{
    public class ShowTag
    {
        public int? ShowId { get; set; }
        public int? TagId { get; set; }
        public Show? Show { get; set; }
        public Tag? Tag { get; set; }
    }
}
