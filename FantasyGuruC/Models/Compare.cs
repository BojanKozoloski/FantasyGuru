using System.Numerics;

namespace FantasyGuruC.Models
{
    public class Compare
    {
        public Manager? Me { get; set; }

        public Manager? Opponent { get; set; }

        public List<Player>? MyUniquePlayers { get; set; }

        public List<Player>? OpponentUniquePlayers { get; set; }

        public int MyGameweekPoints { get; set; }
        public int OpponentGameweekPoints { get; set; }


        public string? MyActiveChip { get; set; }        
        public string? OpponentActiveChip { get; set; }
        public List<string>? MyAvailableChips { get; set; }       
        public List<string>? OpponentAvailableChips { get; set; }



        public List<Player> DifferentialGK { get; set; }
        public List<Player> DifferentialDEF { get; set; }
        public List<Player> DifferentialMID { get; set; }
        public List<Player> DifferentialFWD { get; set; }
    }
}
