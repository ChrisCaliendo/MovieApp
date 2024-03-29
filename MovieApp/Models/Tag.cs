﻿namespace MovieApp.Models
{
    public class Tag
    {

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public ICollection<ShowTag> ShowTags { get; set; } = new List<ShowTag>();

    }
}
