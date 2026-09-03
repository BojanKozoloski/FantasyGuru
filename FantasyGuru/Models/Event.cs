using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FantasyGuru.Models
{
    public class Event
    {
        public int id { get; set; }
        public string name { get; set; }
        public bool is_current { get; set; }
        public bool is_next { get; set; }
        public bool finished { get; set; }
    }
}