using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FantasyGuru.Models
{
    public class Leagues
    {
        public List<League> classic { get; set; } = new List<League>();

        public List<League> h2h { get; set; } = new List<League>();
    }
}