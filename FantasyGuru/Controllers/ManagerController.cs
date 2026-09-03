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
            PickR gwData = fPLTeam.GetGameweekData(id, gw);
            ViewBag.MyGameweekPoints = gwData.entry_history.points;
            ViewBag.CurrentGameweek = gw;

            return View(manager);
        }
        public ActionResult Guru()
        {
            
            return View();
        }
        public ActionResult LeagueC(int managerId, int leagueIndex,int page=1)
        {
            FPLTeam team = new FPLTeam();

            Manager manager = team.GetManager(managerId);

            League league = manager.leagues.classic.ElementAt(leagueIndex);

            //LeagueStandings standings = team.GetLeagueStandings(league.Id);
            LeagueStandings standings = team.GetLeagueStandings(league.Id, page);

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
            PickR myGwData = fpl.GetGameweekData(myid, gw);

            Manager opp = fpl.GetManager(oppid);
            opp.Team = fpl.GetSquad(oppid, gw);
            PickR oppGwData = fpl.GetGameweekData(oppid, gw);

            Compare cmp = new Compare();
            cmp.Me = me;
            cmp.Opponent = opp;
            cmp.MyUniquePlayers = me.Team.Where(p => !opp.Team.Any(o => o.id == p.id)).ToList();
            cmp.OpponentUniquePlayers = opp.Team.Where(p => !me.Team.Any(o => o.id == p.id)).ToList();
            cmp.MyGameweekPoints = myGwData.entry_history.points;
            cmp.OpponentGameweekPoints = oppGwData.entry_history.points;
            ViewBag.CurrentGameweek = gw;

            return View(cmp);
        }
    }
}