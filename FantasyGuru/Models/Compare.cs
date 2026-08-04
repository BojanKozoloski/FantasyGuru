using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FantasyGuru.Models
{
    public class Compare
    {
        public Manager Me { get; set; }

        public Manager Opponent { get; set; }

        public List<Player> MyUniquePlayers { get; set; }

        public List<Player> OpponentUniquePlayers { get; set; }
    }
}