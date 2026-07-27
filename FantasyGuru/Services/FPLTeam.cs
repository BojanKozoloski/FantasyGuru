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

            PickR squad = new PickR();

            squad.picks = new List<Pick>();

            squad.picks.Add(new Pick { element = 350 }); 
            squad.picks.Add(new Pick { element = 200 }); 
            squad.picks.Add(new Pick { element = 388 }); 
            squad.picks.Add(new Pick { element = 229 });
            squad.picks.Add(new Pick { element = 389 });
            squad.picks.Add(new Pick { element = 452 });
            squad.picks.Add(new Pick { element = 40 });
            squad.picks.Add(new Pick { element = 12 });
            squad.picks.Add(new Pick { element = 368 });
            squad.picks.Add(new Pick { element = 55 });
            squad.picks.Add(new Pick { element = 119 });
            squad.picks.Add(new Pick { element = 25 });
            squad.picks.Add(new Pick { element = 398 });
            squad.picks.Add(new Pick { element = 273 });
            squad.picks.Add(new Pick { element = 142 });

            string playerUrl = "https://fantasy.premierleague.com/api/bootstrap-static/";

            var playerid = client.GetStringAsync(playerUrl).Result;

            Bootstrap allplayers= JsonConvert.DeserializeObject<Bootstrap>(playerid);

            foreach (Pick pick in squad.picks)
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