
using System;
using System.Collections;
using System.Data;

namespace MovieApp.Models
{
    public class Binge
    {

        public ICollection<ShowBinge>? ShowBinges { get; set; }
        public ICollection<Tag>? Tags { get; set; }
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? Timespan { get; set; }
    }
}
