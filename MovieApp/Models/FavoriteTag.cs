namespace MovieApp.Models
{
    public class FavoriteTag
    {
        public int? TagId { get; set; }
        public int? UserId { get; set; }
        public Tag? Tag { get; set; }
        public User? User { get; set; }
    }
}
