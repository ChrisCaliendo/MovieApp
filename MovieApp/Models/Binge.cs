
using System;
using System.Collections;
using System.Data;

namespace MovieApp.Models
{
    public class Binge
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public User? Author { get; set; }
        public int UserId { get; set; }
        public ICollection<ShowBinge> ShowBinges { get; set; } = new List<ShowBinge>();

    }
}
