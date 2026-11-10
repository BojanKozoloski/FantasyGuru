namespace FantasyGuruC.Models
{
    public class StandingsTable
    {
        public List<StandingsResult>? results { get; set; }
        public bool has_next { get; set; }
        public int page { get; set; }

    }
}
