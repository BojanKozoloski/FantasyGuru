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

        //https://fantasy.premierleague.com/api/entry/{id}/event/38/picks/
        //https://fantasy.premierleague.com/api/entry/{id}/event/1/picks/
        //https://fantasy.premierleague.com/api/leagues-classic/{leagueId}/standings/ //get all info about standings


        public PickR GetGameweekData(int id, int gameweek)
        {
            string url = $"https://fantasy.premierleague.com/api/entry/{id}/event/{gameweek}/picks/";
            var response = client.GetStringAsync(url).Result;
            PickR pickData = JsonConvert.DeserializeObject<PickR>(response);



            return pickData;
        }
        

    }
}