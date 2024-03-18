
using System;
using System.Collections;
using System.Data;

namespace MovieApp.Models
{
    public class Binge
    {

        public ICollection<ShowBinge>? showBinges { get; set; }
        public ICollection<Tag>? tags { get; set; }
        public int id { get; set; }
        public string? name { get; set; }
        public string? description { get; set; }
        public int? timespan { get; set; }
    }
}
