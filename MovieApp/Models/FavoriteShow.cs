namespace MovieApp.Models
{
    public class FavoriteShow
    {
        public int ShowId { get; set; }
        public int UserId { get; set; }
        public Show Show { get; set; }
    }
}
