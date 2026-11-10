using FantasyGuruC.Models;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FantasyGuruC.Services
{
    public class FPLTeam
    {
        public HttpClient client = new HttpClient();

        private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };


        public Manager GetManager(int id)
        {
            string url = $"https://fantasy.premierleague.com/api/entry/{id}/";

            var response = client.GetStringAsync(url).Result;

            Manager manager = JsonSerializer.Deserialize<Manager>(response, JsonOptions)!;

            return manager;
        }
        public List<Player> GetSquad(int id, int gameweek = 1)
        {
            List<Player> players = new List<Player>();

            string url = $"https://fantasy.premierleague.com/api/entry/{id}/event/{gameweek}/picks/";
            var response = client.GetStringAsync(url).Result;
            PickR squad = JsonSerializer.Deserialize<PickR>(response, JsonOptions)!;

            string playerUrl = "https://fantasy.premierleague.com/api/bootstrap-static/";
            var playerJson = client.GetStringAsync(playerUrl).Result;
            Bootstrap allplayers = JsonSerializer.Deserialize<Bootstrap>(playerJson, JsonOptions)!;

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
            LeagueStandings standings = JsonSerializer.Deserialize<LeagueStandings>(response, JsonOptions)!;
            return standings;
        }

        public LeagueStandings GetLeagueStandingsPaged(int leagueId, int displayPage, int displaySize = 50)
        {
            // Page 1 tells us how many entries FPL returns per request
            LeagueStandings firstPage = GetLeagueStandings(leagueId, 1);
            int apiPageSize = firstPage.standings.results.Count;

            if (apiPageSize == 0)
                return firstPage;

            int startIndex = (displayPage - 1) * displaySize;
            int apiPage = startIndex / apiPageSize + 1;
            int skip = startIndex % apiPageSize;

            var collected = new List<StandingsResult>();
            bool apiHasNext;

            while (true)
            {
                LeagueStandings current = apiPage == 1 ? firstPage : GetLeagueStandings(leagueId, apiPage);
                collected.AddRange(current.standings.results);
                apiHasNext = current.standings.has_next;

                if (!apiHasNext || collected.Count - skip >= displaySize)
                    break;

                apiPage++;
            }

            var pageResults = collected.Skip(skip).Take(displaySize).ToList();
            bool hasNext = apiHasNext || collected.Count - skip > displaySize;

            return new LeagueStandings
            {
                league = firstPage.league,
                standings = new StandingsTable
                {
                    results = pageResults,
                    has_next = hasNext,
                    page = displayPage
                }
            };
        }

        public PickR GetGameweekData(int id, int gameweek)
        {
            string url = $"https://fantasy.premierleague.com/api/entry/{id}/event/{gameweek}/picks/";
            var response = client.GetStringAsync(url).Result;
            PickR pickData = JsonSerializer.Deserialize<PickR>(response, JsonOptions)!;
            return pickData;
        }
        public int GetCurrentGameweek()
        {
            string url = "https://fantasy.premierleague.com/api/bootstrap-static/";
            var response = client.GetStringAsync(url).Result;
            Bootstrap data = JsonSerializer.Deserialize<Bootstrap>(response, JsonOptions)!;

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
            return JsonSerializer.Deserialize<ManagerHistory>(response, JsonOptions)!;
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

        public int CalculateGameweekPoints(List<Player> team, bool benchBoostActive)
        {
            var counting = benchBoostActive ? team : team.Where(p => p.position <= 11);
            return counting.Sum(p => (p.event_points ?? 0) * p.multiplier);
        }

        public List<Player> GetAllPlayers()
        {
            string playerUrl = "https://fantasy.premierleague.com/api/bootstrap-static/";
            var playerJson = client.GetStringAsync(playerUrl).Result;
            Bootstrap allplayers = JsonSerializer.Deserialize<Bootstrap>(playerJson, JsonOptions);
            return allplayers.elements;
        }

        private double ParseForm(string form)
        {
            double value;
            return double.TryParse(form, System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture, out value) ? value : 0;
        }

        public List<Player> GetTopDifferentials(List<Player> allPlayers, List<Player> myTeam, List<Player> oppTeam, int elementType, int count = 3)
        {
            var ownedIds = myTeam.Select(p => p.id).Union(oppTeam.Select(p => p.id)).ToHashSet();

            return allPlayers
                .Where(p => p.element_type == elementType && !ownedIds.Contains(p.id))
                .OrderByDescending(p => ParseForm(p.form))
                .Take(count)
                .ToList();
        }


    }
}
