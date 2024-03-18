namespace MovieApp.Models
{
    public class ShowBinge
    {
        public int? showId { get; set; }
        public int? bingeId { get; set; }
        public Show? show { get; set; }
        public Binge? binge { get; set; }
    }
}
