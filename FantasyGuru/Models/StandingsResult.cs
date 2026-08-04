using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FantasyGuru.Models
{
    public class StandingsResult
    {
        public int entry { get; set; }
        public string entry_name { get; set; }
        public string player_name { get; set; }
        public int rank { get; set; }
        public int total { get; set; }
    }
}