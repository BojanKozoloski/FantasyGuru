using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FantasyGuru.Models
{
    public class Manager
    {
        public int Id { get; set; }

        public string player_first_name { get; set; }

        public string player_last_name { get; set; }

        public int? summary_overall_points { get; set; }

        public List<Player> Team { get; set; }

        public Leagues leagues { get; set; }

        public Manager()
        {
            Team = new List<Player>();
            
        }

        

    }
}