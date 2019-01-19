using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Casablanca.Models.Database;
using Casablanca.Models;

namespace Casablanca.Controllers
{
    public class MissionController : Controller
    {
		private IDal dal;

		public MissionController() : this(new Dal())
		{

		}

		private MissionController(IDal dal)
		{
			this.dal = dal;
		}

		// GET: Mission
		public ActionResult Index()
        {
			//ne retourner les missions concernant QUE ce CDS
            return View(dal.GetMissions());
        }

		public ActionResult ProcessMission(int id)
		{
			return View(dal.GetMission(id));
		}
	}
}