namespace FantasyGuruC.Models
{
    public class Event
    {
        public int id { get; set; }
        public string? name { get; set; }
        public bool is_current { get; set; }
        public bool is_next { get; set; }
        public bool finished { get; set; }
    }
}
