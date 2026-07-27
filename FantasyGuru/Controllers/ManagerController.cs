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
        public ActionResult League(int id)
        {
            FPLTeam fpl = new FPLTeam();

            Manager manager = fpl.GetManager(id);

            return View(manager);

        }
    }
}