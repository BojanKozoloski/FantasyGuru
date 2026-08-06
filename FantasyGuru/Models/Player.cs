using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FantasyGuru.Models
{
    public class Player
    {
        public int id { get; set; }

        public string first_name { get; set; }

        public string second_name { get; set; }

        public int? total_points { get; set; }

        public string web_name { get; set; }    

        public int element_type { get; set; } // GK:1, DEF:2, MID:3, FWD:4

        public int position { get; set; } // starting order/bench 1-11 start,12-15 bench

        public int? event_points { get; set; }


        public int element { get; set; }

        public bool is_captain { get; set; }

        public int team_code { get; set; }

    }
}