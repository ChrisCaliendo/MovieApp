namespace MovieApp.Models
{
    public class ShowBinge
    {
        public int? ShowId { get; set; }
        public int? BingeId { get; set; }
        public Show? Show { get; set; }
        public Binge? Binge { get; set; }
    }
}
