using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using Newtonsoft.Json;
using FantasyGuru.Models;

namespace FantasyGuru.Services
{
    public class FPLTeam
    {
        public HttpClient client = new HttpClient();


        public Manager GetManager(int id)
        {
            string url = $"https://fantasy.premierleague.com/api/entry/{id}/";

            var response = client.GetStringAsync(url).Result;

            Manager manager = JsonConvert.DeserializeObject<Manager>(response);

            return manager;
        }
        public List<Player> GetSquad(int id)
        {
            
            List<Player> players = new List<Player>();

            
            string url = $"https://fantasy.premierleague.com/api/entry/{id}/event/38/picks/";

            //var response = client.GetStringAsync(url).Result;

            //PickR squad = JsonConvert.DeserializeObject<PickR>(response);

            List<int> elementIds;

            if (id == 1)
            {
                elementIds = new List<int> { 109, 200, 356, 388, 426, 427, 397, 452, 368, 379, 106, 171, 39, 277, 316 };
            }
            else 
            {
                elementIds = new List<int> { 350, 200, 388, 229, 389, 452, 40, 12, 368, 55, 411, 25, 398, 273, 142 };
            }

            Manager squad = new Manager();

            squad.Team = new List<Player>();

            
            //87561
            
            string playerUrl = "https://fantasy.premierleague.com/api/bootstrap-static/";
            var playerid = client.GetStringAsync(playerUrl).Result;
            Bootstrap allplayers= JsonConvert.DeserializeObject<Bootstrap>(playerid);

            foreach (Player pick in squad.Team)
            {

                foreach (Player player in allplayers.elements)
                {
                    if (player.id == pick.element)
                    {
                        players.Add(player);
                        break;
                    }
                }

            }
            foreach (int elementId in elementIds)
            {
                foreach (Player player in allplayers.elements)
                {
                    if (player.id == elementId)
                    {
                        players.Add(player);
                        break;
                    }
                }
            }


            return players;


        }
        public LeagueStandings GetLeagueStandings(int leagueId)
        {
            string url = $"https://fantasy.premierleague.com/api/leagues-classic/{leagueId}/standings/";
            var response = client.GetStringAsync(url).Result;
            LeagueStandings standings = JsonConvert.DeserializeObject<LeagueStandings>(response);
            return standings;
        }

        //https://fantasy.premierleague.com/api/entry/{id}/event/38/picks/
        //https://fantasy.premierleague.com/api/entry/{id}/event/1/picks/
        //https://fantasy.premierleague.com/api/leagues-classic/{leagueId}/standings/ //get all info about standings

        public LeagueStandings GetLeagueStandingsFake(int leagueId, string leagueName)
        {
            LeagueStandings standings = new LeagueStandings();

            standings.league = new League
            {
                Id = leagueId,
                Name = leagueName
            };

            standings.standings = new StandingsTable
            {
                results = new List<StandingsResult>
        {
            new StandingsResult { entry = 1, rank = 1, player_name = "Alice Smith", entry_name = "Alice's Aces", total = 2145 },
            new StandingsResult { entry = 2, rank = 2, player_name = "Bob Jones",   entry_name = "Bob's Bombers", total = 2090 },
            new StandingsResult { entry = 3, rank = 3, player_name = "Charlie Lee", entry_name = "Charlie's XI", total = 2050 },
            new StandingsResult { entry = 4, rank = 4, player_name = "Alice Smith the 2nd", entry_name = " Aces", total = 2789 },
            new StandingsResult { entry = 5, rank = 5, player_name = "Bob Jones 3rd",   entry_name = "AROWANA", total = 2444 },
            new StandingsResult { entry = 6, rank = 6, player_name = "Bruce Lee", entry_name = "A team", total = 2121 },
            new StandingsResult { entry = 7, rank = 7, player_name = "Alice Smith", entry_name = "Alice's Aces", total = 2145 },
            new StandingsResult { entry = 8, rank = 8, player_name = "Bob Jones",   entry_name = "Bob's Bombers", total = 2090 },
            new StandingsResult { entry = 9, rank = 9, player_name = "Charlie Lee", entry_name = "Charlie's XI", total = 2050 },
            new StandingsResult { entry = 10, rank = 10, player_name = "Alice Smith the 2nd", entry_name = " Aces", total = 2789 },
            new StandingsResult { entry = 11, rank = 11, player_name = "Bob Jones 3rd",   entry_name = "AROWANA", total = 2444 },
            new StandingsResult { entry = 12, rank = 12, player_name = "Bruce Lee", entry_name = "A team", total = 2121 },
            new StandingsResult { entry = 13, rank = 13, player_name = "Alice Smith", entry_name = "Alice's Aces", total = 2145 },
            new StandingsResult { entry = 14, rank = 14, player_name = "Bob Jones",   entry_name = "Bob's Bombers", total = 2090 },
            new StandingsResult { entry = 15, rank = 15, player_name = "Charlie Lee", entry_name = "Charlie's XI", total = 2050 },
            new StandingsResult { entry = 16, rank = 16, player_name = "Alice Smith the 2nd", entry_name = " Aces", total = 2789 },
            new StandingsResult { entry = 17, rank = 17, player_name = "Bob Jones 3rd",   entry_name = "AROWANA", total = 2444 },
            new StandingsResult { entry = 18, rank = 18, player_name = "Bruce Lee", entry_name = "A team", total = 2121 },
            new StandingsResult { entry = 19, rank = 19, player_name = "Alice Smith", entry_name = "Alice's Aces", total = 2145 },
            new StandingsResult { entry = 20, rank = 20, player_name = "Bob Jones",   entry_name = "Bob's Bombers", total = 2090 },
            new StandingsResult { entry = 21, rank = 21, player_name = "Charlie Lee", entry_name = "Charlie's XI", total = 2050 },
            new StandingsResult { entry = 22, rank = 22, player_name = "Alice Smith the 2nd", entry_name = " Aces", total = 2789 },
            new StandingsResult { entry = 23, rank = 23, player_name = "Bob Jones 3rd",   entry_name = "AROWANA", total = 2444 },
            new StandingsResult { entry = 24, rank = 24, player_name = "Bruce Lee", entry_name = "A team", total = 2121 },
        }
            };

            return standings;
        }

    }
}