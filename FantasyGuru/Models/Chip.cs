using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FantasyGuru.Models
{
    public class Chip
    {
        internal int events;

        public string name { get; set; }   // "wildcard", "freehit", "bboost", "3xc"
        [JsonProperty("event")]
        public int gameweek { get; set; }     // gameweek it was played
        public string time { get; set; }
    }
}