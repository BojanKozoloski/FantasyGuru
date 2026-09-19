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

        public int MyGameweekPoints { get; set; }
        public int OpponentGameweekPoints { get; set; }


        public string MyActiveChip { get; set; }         // chip in use THIS gameweek, or null
        public string OpponentActiveChip { get; set; }
        public List<string> MyAvailableChips { get; set; }       // chips not yet used this half
        public List<string> OpponentAvailableChips { get; set; }
    }
}