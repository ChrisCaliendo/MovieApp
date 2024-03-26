namespace MovieApp.Dto
{
    public class BingeExtDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? Timespan { get; set; }
        public int? ShowCount { get; set; }
        public bool IsTimespanAccurate { get; set; }
    }
}
