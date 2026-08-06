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

        public ActionResult Squad(int id,int gameweek =1)
        {
            FPLTeam fPLTeam = new FPLTeam();

            Manager manager = fPLTeam.GetManager(id);

            manager.Team = fPLTeam.GetSquad(id);

            PickR gwData = fPLTeam.GetGameweekDataFake(id, gameweek);

            ViewBag.MyGameweekPoints = gwData.entry_history.points;

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
            LeagueStandings standings = team.GetLeagueStandingsFake(league.Id,league.Name,page);

            ViewBag.ManagerId = managerId;
            ViewBag.LeagueIndex = leagueIndex;

            
            return View(standings);

        }
        public ActionResult Compare(int myid,int oppid,int gameweek = 1)
        {
            FPLTeam fpl = new FPLTeam();

            Manager me = fpl.GetManager(myid);
            me.Team = fpl.GetSquad(myid);
            PickR myGwData = fpl.GetGameweekDataFake(myid, gameweek);

            Manager opp = fpl.GetManager(oppid);
            opp.Team = fpl.GetSquad(oppid);
            PickR oppGwData = fpl.GetGameweekDataFake(oppid, gameweek);

            Compare cmp = new Compare();

            cmp.Me = me;
            cmp.Opponent = opp;

            cmp.MyUniquePlayers = me.Team.Where(p => !opp.Team.Any(o => o.id == p.id)).ToList();
            cmp.OpponentUniquePlayers = opp.Team.Where(p => !me.Team.Any(o => o.id == p.id)).ToList();

            cmp.MyGameweekPoints = myGwData.entry_history.points;
            cmp.OpponentGameweekPoints = oppGwData.entry_history.points;

            return View(cmp);
        }
    }
}