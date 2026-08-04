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

        public ActionResult Squad(int id)
        {
            FPLTeam fPLTeam = new FPLTeam();

            Manager manager = fPLTeam.GetManager(id);

            manager.Team = fPLTeam.GetSquad(id);

            return View(manager);
        }
        public ActionResult Guru()
        {
            
            return View();
        }
        public ActionResult LeagueC(int managerId, int leagueIndex)
        {
            FPLTeam team = new FPLTeam();

            Manager manager = team.GetManager(managerId);

            League league = manager.leagues.classic.ElementAt(leagueIndex);

            //LeagueStandings standings = team.GetLeagueStandings(league.Id);
            LeagueStandings standings = team.GetLeagueStandingsFake(league.Id,league.Name);

            ViewBag.ManagerId = managerId;

            return View(standings);

        }
        public ActionResult Compare(int myid,int oppid)
        {
            FPLTeam fpl = new FPLTeam();

            Manager me = fpl.GetManager(myid);
            me.Team = fpl.GetSquad(myid);

            Manager opp = fpl.GetManager(oppid);
            opp.Team = fpl.GetSquad(oppid);

            Compare cmp = new Compare();

            cmp.Me = me;
            cmp.Opponent = opp;

            cmp.MyUniquePlayers = me.Team.Where(p => !opp.Team.Any(o => o.id == p.id)).ToList();
            cmp.OpponentUniquePlayers = opp.Team.Where(p => !me.Team.Any(o => o.id == p.id)).ToList();

            return View(cmp);
        }
    }
}