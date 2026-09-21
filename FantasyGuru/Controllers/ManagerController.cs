using FantasyGuru.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FantasyGuru.Services;
using System.Web.Services.Description;


namespace FantasyGuru.Controllers
{
    public class ManagerController : Controller
    {
        // GET: Manager
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Squad(int id, int? gameweek = null)
        {
            FPLTeam fPLTeam = new FPLTeam();
            int gw = gameweek ?? fPLTeam.GetCurrentGameweek();

            Manager manager = fPLTeam.GetManager(id);
            manager.Team = fPLTeam.GetSquad(id, gw);

            var (myAvailable, myActive) = fPLTeam.GetChipStatus(id, gw);
            bool benchBoostActive = myActive == "bboost";

            ViewBag.MyGameweekPoints = fPLTeam.CalculateGameweekPoints(manager.Team, benchBoostActive);
            ViewBag.CurrentGameweek = gw;
            ViewBag.MyActiveChip = myActive;
            ViewBag.MyAvailableChips = myAvailable;

            return View(manager);
        }
        public ActionResult Guru()
        {
            
            return View();
        }
        public ActionResult LeagueC(int managerId, int leagueIndex, int page = 1)
        {
            FPLTeam team = new FPLTeam();
            Manager manager = team.GetManager(managerId);
            League league = manager.leagues.classic.ElementAt(leagueIndex);
            LeagueStandings standings = team.GetLeagueStandings(league.Id, page);

            standings.standings.results = standings.standings.results
                .OrderByDescending(r => r.event_total)
                .ToList();

            ViewBag.ManagerId = managerId;
            ViewBag.LeagueIndex = leagueIndex;

            return View(standings);
        }
        public ActionResult Compare(int myid, int oppid, int? gameweek = null)
        {
            FPLTeam fpl = new FPLTeam();
            int gw = gameweek ?? fpl.GetCurrentGameweek();

            Manager me = fpl.GetManager(myid);
            me.Team = fpl.GetSquad(myid, gw);

            Manager opp = fpl.GetManager(oppid);
            opp.Team = fpl.GetSquad(oppid, gw);

            var (myAvailable, myActive) = fpl.GetChipStatus(myid, gw);
            var (oppAvailable, oppActive) = fpl.GetChipStatus(oppid, gw);

            bool myBenchBoost = myActive == "bboost";
            bool oppBenchBoost = oppActive == "bboost";

            Compare cmp = new Compare();
            cmp.Me = me;
            cmp.Opponent = opp;
            cmp.MyUniquePlayers = me.Team.Where(p => !opp.Team.Any(o => o.id == p.id && o.is_captain == p.is_captain)).ToList();
            cmp.OpponentUniquePlayers = opp.Team.Where(p => !me.Team.Any(o => o.id == p.id && o.is_captain == p.is_captain)).ToList();

            cmp.MyGameweekPoints = fpl.CalculateGameweekPoints(me.Team, myBenchBoost);
            cmp.OpponentGameweekPoints = fpl.CalculateGameweekPoints(opp.Team, oppBenchBoost);

            cmp.MyAvailableChips = myAvailable;
            cmp.MyActiveChip = myActive;
            cmp.OpponentAvailableChips = oppAvailable;
            cmp.OpponentActiveChip = oppActive;

            return View(cmp);
        }
    }
}