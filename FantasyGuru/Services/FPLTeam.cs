using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using Newtonsoft.Json;
using FantasyGuru.Models;
using System.Drawing.Printing;
using System.Web.UI;

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
        public List<Player> GetSquad(int id, int gameweek = 1)
        {
            List<Player> players = new List<Player>();

            string url = $"https://fantasy.premierleague.com/api/entry/{id}/event/{gameweek}/picks/";
            var response = client.GetStringAsync(url).Result;
            PickR squad = JsonConvert.DeserializeObject<PickR>(response);

            string playerUrl = "https://fantasy.premierleague.com/api/bootstrap-static/";
            var playerJson = client.GetStringAsync(playerUrl).Result;
            Bootstrap allplayers = JsonConvert.DeserializeObject<Bootstrap>(playerJson);

            foreach (Pick pick in squad.picks)
            {
                Player player = allplayers.elements.FirstOrDefault(p => p.id == pick.element);
                if (player != null)
                {
                    player.position = pick.position;       
                    player.is_captain = pick.is_captain;
                    player.multiplier = pick.multiplier;
                    players.Add(player);
                }
            }

            return players;
        }
        public LeagueStandings GetLeagueStandings(int leagueId, int page = 1)
        {
            string url = $"https://fantasy.premierleague.com/api/leagues-classic/{leagueId}/standings/?page_standings={page}";
            var response = client.GetStringAsync(url).Result;
            LeagueStandings standings = JsonConvert.DeserializeObject<LeagueStandings>(response);
            return standings;
        }

        public PickR GetGameweekData(int id, int gameweek)
        {
            string url = $"https://fantasy.premierleague.com/api/entry/{id}/event/{gameweek}/picks/";
            var response = client.GetStringAsync(url).Result;
            PickR pickData = JsonConvert.DeserializeObject<PickR>(response);
            return pickData;
        }
        public int GetCurrentGameweek()
        {
            string url = "https://fantasy.premierleague.com/api/bootstrap-static/";
            var response = client.GetStringAsync(url).Result;
            Bootstrap data = JsonConvert.DeserializeObject<Bootstrap>(response);

            Event current = data.events.FirstOrDefault(e => e.is_current);
            if (current != null)
            {
                return current.id;
            }

            Event next = data.events.FirstOrDefault(e => e.is_next);
            return next != null ? next.id : 1;
        }
        public ManagerHistory GetManagerHistory(int id)
        {
            string url = $"https://fantasy.premierleague.com/api/entry/{id}/history/";
            var response = client.GetStringAsync(url).Result;
            return JsonConvert.DeserializeObject<ManagerHistory>(response);
        }

        // Boundary gameweek where chips reset — confirm this still holds for the live season
        private const int ChipResetGameweek = 19;

        private static readonly List<string> AllChipNames = new List<string>
{
    "wildcard", "freehit", "bboost", "3xc"
};

        public (List<string> available, string activeThisWeek) GetChipStatus(int managerId, int currentGameweek)
        {
            ManagerHistory history = GetManagerHistory(managerId);

            bool inSecondHalf = currentGameweek > ChipResetGameweek;

            var usedThisHalf = history.chips
                .Where(c => inSecondHalf ? c.gameweek > ChipResetGameweek : c.gameweek <= ChipResetGameweek)
                .ToList();

            // A chip is "active right now" only if it was used in EXACTLY the gameweek being viewed
            string activeThisWeek = usedThisHalf
                .FirstOrDefault(c => c.gameweek == currentGameweek)?.name;

            var usedNames = usedThisHalf.Select(c => c.name).ToHashSet();
            var available = AllChipNames.Where(n => !usedNames.Contains(n)).ToList();

            return (available, activeThisWeek);
        }


    }
}