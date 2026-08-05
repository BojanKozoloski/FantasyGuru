using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FantasyGuru.Models
{
    public class StandingsTable
    {
        
        public List<StandingsResult> results { get; set; }
        public bool has_next { get; set; }
        public int page { get; set; }

    }
}