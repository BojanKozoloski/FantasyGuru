
using System.Text.Json.Serialization;

namespace FantasyGuruC.Models
{
    public class Chip
    {
        internal int events;

        public string? name { get; set; }   // "wildcard", "freehit", "bboost", "3xc"
        [JsonPropertyName("event")]
        public int gameweek { get; set; }     // gameweek it was played
        public string? time { get; set; }
    }
}
