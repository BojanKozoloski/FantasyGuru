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

            Manager squad = new Manager();

            squad.Team = new List<Player>();

            squad.Team.Add(new Player { element = 350 }); 
            squad.Team.Add(new Player { element = 200 }); 
            squad.Team.Add(new Player { element = 388 }); 
            squad.Team.Add(new Player { element = 229 });
            squad.Team.Add(new Player { element = 389 });
            squad.Team.Add(new Player { element = 452 });
            squad.Team.Add(new Player { element = 40 });
            squad.Team.Add(new Player { element = 12 });
            squad.Team.Add(new Player { element = 368 });
            squad.Team.Add(new Player { element = 55 });
            squad.Team.Add(new Player { element = 119 });
            squad.Team.Add(new Player { element = 25 });
            squad.Team.Add(new Player { element = 398 });
            squad.Team.Add(new Player { element = 273 });
            squad.Team.Add(new Player { element = 142 });

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


            return players;


        }
        //https://fantasy.premierleague.com/api/entry/{id}/event/38/picks/
        //https://fantasy.premierleague.com/api/entry/{id}/event/1/picks/

    }
}