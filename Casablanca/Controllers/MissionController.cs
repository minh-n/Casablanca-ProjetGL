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
			//------------Background identity check-------------//
			if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
				return Redirect("/Home/Index");

			Collaborator coll = dal.GetCollaborator(System.Web.HttpContext.Current.User.Identity.Name);
			if ((HelperModel.CheckCDS(coll) == false)) //if not CDS
				return Redirect("/Home/Index");
			//--------------------------------------------------//

			List<Mission> model = new List<Mission>();

			foreach (Mission mi in dal.GetMissions())
			{
				if (mi.ChiefId == coll.Id)
					model.Add(mi);
			}

			return View(model);
        }

		public ActionResult ProcessMission(int id)
		{
			//------------Background identity check-------------//
			if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
				return Redirect("/Home/Index");

			Collaborator coll = dal.GetCollaborator(System.Web.HttpContext.Current.User.Identity.Name);
			if ((HelperModel.CheckCDS(coll) == false)) //if not CDS
				return Redirect("/Home/Index");
			//--------------------------------------------------//
			
			return View(dal.GetMission(id));
		}

		[HttpPost] // Backend call of ProcessMission page
		public ActionResult ProcessMission(Mission model, int id)
		{          
			//------------Background identity check-------------//
			if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
				return Redirect("/Home/Index");

			Collaborator coll = dal.GetCollaborator(System.Web.HttpContext.Current.User.Identity.Name);
			if ((HelperModel.CheckCDS(coll) == false)) //if not CDS
				return Redirect("/Home/Index");
			//--------------------------------------------------//


			//TODO : check model state validity ?
			dal.GetMission(id).Name = model.Name;
			dal.GetMission(id).StartDate = model.StartDate;
			dal.GetMission(id).EndDate = model.EndDate;

			dal.SaveChanges();

			return Redirect("/Mission/Index");
		}


		[HttpPost] // Backend call of Index page
		public ActionResult CreateMission()
		{
			//------------Background identity check-------------//
			if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
				return Redirect("/Home/Index");

			Collaborator coll = dal.GetCollaborator(System.Web.HttpContext.Current.User.Identity.Name);
			if ((HelperModel.CheckCDS(coll) == false)) //if not CDS
				return Redirect("/Home/Index");
			//--------------------------------------------------//


			// Create the Mission
			int returnedId = dal.CreateMission(coll.Id);
			string redirectString = "/Mission/ProcessMission/?id=" + returnedId;

			//redirect to the right page, displaying the newly created Mission
			return Redirect(redirectString);
		}

	}
}